using DummyLang.LexicalAnalysis;
using DummyLang.LexicalAnalysis.Extensions;
using DummyLang.SyntacticAnalysis.Expressions;
using DummyLang.SyntacticAnalysis.Extensions;
using DummyLang.SyntacticAnalysis.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DummyLang.SyntacticAnalysis.Parsers;

internal static class ExpressionParser
{
    public static Expression Parse(ref int index, in Token[] tokens, OperatorPrecedence previousPrecedence = 0)
    {
        var left = ParseExpressionBasedOnCurrentToken(ref index, in tokens);

        while (index < tokens.Length && tokens[index].IsBinaryOperator())
        {
            var currentPrecedence = tokens[index].GetOperatorPrecedence();
            if (currentPrecedence == OperatorPrecedence.None || currentPrecedence <= previousPrecedence)
                break;

            var op    = GetAndMoveToNext(ref index, in tokens);
            var right = Parse(ref index, in tokens, currentPrecedence);

            left = new BinaryExpression(left, op, right);
        }

        return left;
    }

    private static Token GetAndMoveToNext(ref int index, in Token[] tokens) =>
        index >= 0 && index < tokens.Length ? tokens[index++] : Token.None;

    private static Expression ParseExpressionBasedOnCurrentToken(ref int index, in Token[] tokens)
    {
        Debug.Assert(index < tokens.Length);

        // ReSharper disable once ConvertSwitchStatementToSwitchExpression
        // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
        switch (tokens[index].Type)
        {
            case TokenType.Plus:
            case TokenType.Minus:
            case TokenType.PlusPlus:
            case TokenType.MinusMinus:
            case TokenType.Bang:
            case TokenType.Tilde:
            case TokenType.Star:
            case TokenType.Ampersand:
                return ParseUnaryExpression(ref index, in tokens);
            case TokenType.LeftParenthesis:
                return ParseParenthesisedExpression(ref index, in tokens);
            case TokenType.RightParenthesis:
                return new InvalidExpression(GetAndMoveToNext(ref index, in tokens));
            case TokenType.Identifier:
                return ParseIdentifierRelatedExpressions(ref index, in tokens);
            case TokenType.Integer:
            case TokenType.Real:
                return ParseNumberExpression(ref index, in tokens);
            case TokenType.Character:
                return ParseCharacterExpression(ref index, in tokens);
            case TokenType.String:
                return ParseStringExpression(ref index, in tokens);
            default:
                return new InvalidExpression(GetAndMoveToNext(ref index, in tokens));
        }
    }

    private static UnaryExpression ParseUnaryExpression(ref int index, in Token[] tokens)
    {
        var         unaryOperator = GetAndMoveToNext(ref index, in tokens);
        Expression? expression    = default;

        if (tokens[index].Type == TokenType.Identifier)
            expression = new IdentifierExpression(GetAndMoveToNext(ref index, in tokens));

        if (unaryOperator.IsAdditiveOperator() && tokens[index].IsNumber())
            expression = ParseNumberExpression(ref index, in tokens);

        return new UnaryExpression(
            unaryOperator,
            expression ?? new InvalidExpression(GetAndMoveToNext(ref index, in tokens)));
    }

    private static Expression ParseParenthesisedExpression(ref int index, in Token[] tokens)
    {
        var leftParen  = GetAndMoveToNext(ref index, in tokens);
        var expression = Parse(ref index, in tokens);

        Debug.Assert(index < tokens.Length);
        if (tokens[index].Type == TokenType.RightParenthesis)
            return new ParenthesisedExpression(
                leftParen,
                expression,
                GetAndMoveToNext(ref index, in tokens));

        return new InvalidExpression(
            typeof(ParenthesisedExpression),
            Token.ExpectedAt(tokens[index].Position, TokenType.RightParenthesis));
    }

    private static Expression ParseIdentifierRelatedExpressions(ref int index, in Token[] tokens)
    {
        var        identifierToken = GetAndMoveToNext(ref index, in tokens);
        Expression expression;

        Debug.Assert(index < tokens.Length);
        // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
        switch (tokens[index].Type)
        {
            case TokenType.LeftParenthesis:
            {
                expression = ParseFunctionCallExpression(ref index, in tokens, in identifierToken);
                break;
            }
            case TokenType.LeftBracket:
            {
                expression = ParseIndexerExpression(ref index, in tokens, in identifierToken);
                break;
            }
            default:
            {
                expression = new IdentifierExpression(identifierToken);

                if (tokens[index].Type != TokenType.PlusPlus && tokens[index].Type != TokenType.MinusMinus)
                    break;

                expression = new PrimaryExpression(expression, GetAndMoveToNext(ref index, in tokens));

                while (index < tokens.Length && tokens[index].Type is TokenType.PlusPlus or TokenType.MinusMinus)
                    expression = new PrimaryExpression(expression, GetAndMoveToNext(ref index, in tokens));

                break;
            }
        }

        if (index >= tokens.Length)
            return expression;

        // ReSharper disable once ConvertSwitchStatementToSwitchExpression
        // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
        switch (tokens[index].Type)
        {
            case TokenType.PointerAccess:
            case TokenType.Dot:
                return new MemberAccessExpression(
                    expression,
                    GetAndMoveToNext(ref index, in tokens),
                    Parse(ref index, in tokens));
            case TokenType.DoubleDot:
                return new RangeExpression(
                    expression,
                    GetAndMoveToNext(ref index, in tokens),
                    Parse(ref index, in tokens));
            default:
                return expression;
        }
    }

    private static Expression ParseFunctionCallExpression(ref int index, in Token[] tokens, in Token identifier)
    {
        Token leftParenthesis;
        Token rightParenthesis;
        var   expressions = new List<Expression>();
        var   commas      = new List<Token>();

        if (ParsingUtilities.TryGetBalancedBrackets(in tokens, index, out var endIndex))
        {
            leftParenthesis  = GetAndMoveToNext(ref index, in tokens);
            rightParenthesis = tokens[endIndex];

            while (index < endIndex)
            {
                expressions.Add(Parse(ref index, in tokens));

                if (tokens[index].Type == TokenType.Comma)
                    commas.Add(GetAndMoveToNext(ref index, in tokens));
            }

            // Ensuring correct indexing
            index = endIndex + 1;

            return new PrimaryExpression(
                new FunctionCallExpression(identifier, leftParenthesis, rightParenthesis, expressions, commas));
        }

        if (index == endIndex - 1 && tokens[index].Type == TokenType.LeftParenthesis)
            return new InvalidExpression(
                typeof(FunctionCallExpression),
                Token.ExpectedAt(tokens[index].Position, TokenType.RightParenthesis));

        leftParenthesis = GetAndMoveToNext(ref index, in tokens);
        rightParenthesis = tokens[endIndex].Type == TokenType.RightParenthesis
            ? tokens[endIndex]
            : Token.None;

        while (index < endIndex)
        {
            expressions.Add(Parse(ref index, in tokens));

            if (tokens[index].Type == TokenType.Comma)
                commas.Add(GetAndMoveToNext(ref index, in tokens));
        }

        // Ensuring correct indexing
        index = endIndex;

        return rightParenthesis == Token.None
            ? new InvalidExpression(
                new FunctionCallExpression(identifier, leftParenthesis, rightParenthesis, expressions, commas))
            : new InvalidExpression(
                new FunctionCallExpression(identifier, leftParenthesis, rightParenthesis, expressions, commas),
                Token.ExpectedAt(tokens[endIndex].Position, TokenType.RightParenthesis));
    }

    private static Expression ParseIndexerExpression(ref int index, in Token[] tokens, in Token identifier)
    {
        Token       leftBracket;
        Token       rightBracket;
        Expression? expression;

        if (ParsingUtilities.TryGetBalancedBrackets(in tokens, index, out var endIndex))
        {
            leftBracket  = GetAndMoveToNext(ref index, in tokens);
            expression   = Parse(ref index, in tokens);
            rightBracket = tokens[endIndex];

            // Ensuring correct indexing
            index = endIndex + 1;

            return new PrimaryExpression(new IndexerExpression(identifier, leftBracket, rightBracket, expression));
        }

        if (index == endIndex - 1 && tokens[index].Type == TokenType.LeftBracket)
            return new InvalidExpression(
                typeof(IndexerExpression),
                Token.ExpectedAt(tokens[index].Position, TokenType.RightBracket));

        leftBracket = GetAndMoveToNext(ref index, in tokens);
        expression  = Parse(ref index, in tokens);
        rightBracket = tokens[endIndex].Type == TokenType.RightBracket
            ? tokens[endIndex]
            : Token.None;

        // Ensuring correct indexing
        index = endIndex;

        return rightBracket == Token.None
            ? new InvalidExpression(
                new IndexerExpression(identifier, leftBracket, rightBracket, expression))
            : new InvalidExpression(
                new IndexerExpression(identifier, leftBracket, rightBracket, expression),
                Token.ExpectedAt(tokens[endIndex].Position, TokenType.RightParenthesis));
    }

    private static NumberLiteralExpression ParseNumberExpression(ref int index, in Token[] tokens)
    {
        Debug.Assert(index < tokens.Length);
        var number = tokens[index].Value;

        if (tokens[index].Type == TokenType.Integer)
        {
            var integerType = NumberType.Integer;

            if (number.StartsWith("0x"))
                integerType = NumberType.Hexadecimal;
            else if (number.StartsWith("0b"))
                integerType = NumberType.Binary;
            else if (number.EndsWith("ul", StringComparison.OrdinalIgnoreCase))
                integerType = NumberType.UnsignedLong;
            else if (number.EndsWith("u", StringComparison.OrdinalIgnoreCase))
                integerType = NumberType.UnsignedInteger;
            else if (number.EndsWith("l", StringComparison.OrdinalIgnoreCase))
                integerType = NumberType.Long;

            return new NumberLiteralExpression(GetAndMoveToNext(ref index, in tokens), integerType);
        }

        var realType = NumberType.Double;

        if (number.EndsWith("f", StringComparison.OrdinalIgnoreCase))
            realType = NumberType.Float;
        else if (number.EndsWith("d", StringComparison.OrdinalIgnoreCase))
            realType = NumberType.Double;
        else if (number.EndsWith("m", StringComparison.OrdinalIgnoreCase))
            realType = NumberType.Decimal;
        else if (number.Contains('e', StringComparison.OrdinalIgnoreCase))
            realType = NumberType.WithExponent;

        return new NumberLiteralExpression(GetAndMoveToNext(ref index, in tokens), realType);
    }

    private static Expression ParseCharacterExpression(ref int index, in Token[] tokens)
    {
        var characterToken             = GetAndMoveToNext(ref index, in tokens);
        var characterValue             = characterToken.Value;
        var characterLiteralExpression = new CharacterLiteralExpression(characterToken);

        var diagnosticMessage = string.Empty;

        if (!characterValue.HasValidCharacterLength())
            diagnosticMessage = CharacterLiteralExpression.ShouldBeOfCertainLength;
        else if (!characterValue.IsSurroundedBySingleQuotes())
            diagnosticMessage = CharacterLiteralExpression.ShouldStartEndWithSingleQuote;
        else if (characterValue.IsUnescapedSingleQuoteOrBackslash())
            diagnosticMessage = CharacterLiteralExpression.ShouldBeEscaped;
        else if (characterValue[1] == '\\')
        {
            if (characterValue[2] == 'x')
            {
                // ReSharper disable once InvertIf
                if (!characterValue.IsValidHexadecimalCharacter())
                    diagnosticMessage = CharacterLiteralExpression.InvalidEscapedCharacter;
            }
            else if (!characterValue.IsValidEscapedCharacter())
                diagnosticMessage = CharacterLiteralExpression.InvalidHexadecimalCharacter;
        }

        return string.IsNullOrWhiteSpace(diagnosticMessage) switch
        {
            false => new InvalidExpression(characterLiteralExpression, characterToken),
            _     => characterLiteralExpression
        };
    }

    private static Expression ParseStringExpression(ref int index, in Token[] tokens)
    {
        var stringToken             = GetAndMoveToNext(ref index, in tokens);
        var stringValue             = stringToken.Value;
        var stringLiteralExpression = new StringLiteralExpression(stringToken);

        var diagnosticsMessage = string.Empty;

        if (!stringValue.IsValidLength() || !stringValue.IsSurroundedByDoubleQuotes())
            diagnosticsMessage = StringLiteralExpression.ShouldBeSurroundedByDoubleQuote;
        else if (stringValue.EscapesLastDoubleQuote())
            diagnosticsMessage = StringLiteralExpression.ShouldNotEscapeLastDoubleQuote;
        else if (stringValue.HasInvalidEscapedCharacters())
            diagnosticsMessage = StringLiteralExpression.InvalidEscapedCharacter;

        if (string.IsNullOrWhiteSpace(diagnosticsMessage))
            return stringLiteralExpression;

        return new InvalidExpression(stringLiteralExpression, stringToken);
    }
}