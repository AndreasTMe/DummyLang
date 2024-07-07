using DummyLang.LexicalAnalysis;
using DummyLang.LexicalAnalysis.Extensions;
using DummyLang.SyntacticAnalysis.Abstractions;
using DummyLang.SyntacticAnalysis.Expressions;
using DummyLang.SyntacticAnalysis.Extensions;
using DummyLang.SyntacticAnalysis.Utilities;
using System.Collections.Generic;
using System.Diagnostics;

namespace DummyLang.SyntacticAnalysis.Parsers;

internal static class ExpressionParser
{
    public static IExpression Parse(ref int index, in Token[] tokens, OperatorPrecedence previousPrecedence = 0)
    {
        var left = ParseExpressionBasedOnCurrentToken(ref index, in tokens);

        while (index < tokens.Length && TokenAt(index, in tokens).IsBinaryOperator())
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

    public static ITypeExpression ParseType(ref int index, in Token[] tokens, OperatorPrecedence previousPrecedence = 0)
    {
        var left = ParseTypeExpressionBasedOnCurrentToken(ref index, in tokens);

        while (index < tokens.Length && TokenAt(index, in tokens).IsBitwiseOperator())
        {
            var currentPrecedence = tokens[index].GetOperatorPrecedence();
            if (currentPrecedence == OperatorPrecedence.None || currentPrecedence <= previousPrecedence)
                break;

            var op    = GetAndMoveToNext(ref index, in tokens);
            var right = ParseType(ref index, in tokens, currentPrecedence);

            left = new TypeBinaryExpression(left, op, right);
        }

        return left;
    }

    private static Token GetAndMoveToNext(ref int index, in Token[] tokens) =>
        index >= 0 && index < tokens.Length ? tokens[index++] : Token.None;

    private static TokenType TypeAt(int index, in Token[] tokens) =>
        index >= 0 && index < tokens.Length ? tokens[index].Type : TokenType.None;

    private static Token TokenAt(int index, in Token[] tokens) =>
        index >= 0 && index < tokens.Length ? tokens[index] : Token.None;

    private static IExpression ParseExpressionBasedOnCurrentToken(ref int index, in Token[] tokens)
    {
        Debug.Assert(index < tokens.Length);

        return tokens[index].Type switch
        {
            TokenType.Plus
                or TokenType.Minus
                or TokenType.PlusPlus
                or TokenType.MinusMinus
                or TokenType.Bang
                or TokenType.Tilde
                or TokenType.Star
                or TokenType.Ampersand => ParseUnaryExpression(ref index, in tokens),
            TokenType.LeftParenthesis           => ParseParenthesizedExpression(ref index, in tokens),
            TokenType.Identifier                => ParseIdentifierRelatedExpressions(ref index, in tokens),
            TokenType.Integer or TokenType.Real => ParseNumberExpression(ref index, in tokens),
            TokenType.Character                 => ParseCharacterExpression(ref index, in tokens),
            TokenType.String                    => ParseStringExpression(ref index, in tokens),
            _                                   => new UnexpectedTokenExpression(GetAndMoveToNext(ref index, in tokens))
        };
    }

    private static ITypeExpression ParseTypeExpressionBasedOnCurrentToken(ref int index, in Token[] tokens)
    {
        Debug.Assert(index < tokens.Length);

        return tokens[index].Type switch
        {
            TokenType.LeftParenthesis           => ParseParenthesizedExpression(ref index, in tokens, true),
            TokenType.Identifier                => ParseTypeIdentifierExpression(ref index, in tokens),
            TokenType.Integer or TokenType.Real => ParseNumberExpression(ref index, in tokens),
            TokenType.Character                 => ParseCharacterExpression(ref index, in tokens),
            TokenType.String                    => ParseStringExpression(ref index, in tokens),
            _                                   => new UnexpectedTokenExpression(GetAndMoveToNext(ref index, in tokens))
        };
    }

    private static UnaryExpression ParseUnaryExpression(ref int index, in Token[] tokens)
    {
        var unaryOperator = GetAndMoveToNext(ref index, in tokens);
        var expression    = ParseExpressionBasedOnCurrentToken(ref index, in tokens);

        return new UnaryExpression(unaryOperator, expression);
    }

    private static ParenthesisedExpression ParseParenthesizedExpression(ref int index,
                                                                        in Token[] tokens,
                                                                        bool isType = false)
    {
        var leftParen  = GetAndMoveToNext(ref index, in tokens);
        var rightParen = Token.None;

        if (isType)
        {
            var typeExpression = ParseType(ref index, in tokens);

            if (TypeAt(index, in tokens) == TokenType.RightParenthesis)
                rightParen = GetAndMoveToNext(ref index, in tokens);

            return new ParenthesisedExpression(leftParen, typeExpression, rightParen);
        }

        var expression = Parse(ref index, in tokens);

        if (TypeAt(index, in tokens) == TokenType.RightParenthesis)
            rightParen = GetAndMoveToNext(ref index, in tokens);

        return new ParenthesisedExpression(leftParen, expression, rightParen);
    }

    private static IExpression ParseIdentifierRelatedExpressions(ref int index, in Token[] tokens)
    {
        var         identifierToken = GetAndMoveToNext(ref index, in tokens);
        IExpression expression;

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
                var access = GetAndMoveToNext(ref index, in tokens);
                return new MemberAccessExpression(
                    expression,
                    access,
                    ParseIdentifierRelatedExpressions(ref index, in tokens));
            case TokenType.DoubleDot:
                var range = GetAndMoveToNext(ref index, in tokens);
                return new RangeExpression(
                    expression,
                    range,
                    Parse(ref index, in tokens));
            default:
                return expression;
        }
    }

    private static ITypeExpression ParseTypeIdentifierExpression(ref int index, in Token[] tokens)
    {
        var identifierToken = GetAndMoveToNext(ref index, in tokens);

        if (TypeAt(index, in tokens) != TokenType.LessThan)
            return new TypeIdentifierExpression(identifierToken);

        Token                         leftGeneric;
        Token                         rightGeneric;
        List<TypeArgumentExpression>? arguments = null;

        TypeGenericExpression expression;

        if (ParsingUtilities.TryGetBalancedTypeBrackets(in tokens, index, out var endIndex))
        {
            leftGeneric  = GetAndMoveToNext(ref index, in tokens);
            rightGeneric = tokens[endIndex];

            if (index < endIndex)
                arguments = new List<TypeArgumentExpression>();

            while (index < endIndex)
            {
                var argument = ParseType(ref index, in tokens);
                var comma = TypeAt(index, in tokens) == TokenType.Comma
                    ? GetAndMoveToNext(ref index, in tokens)
                    : Token.None;
                arguments!.Add(new TypeArgumentExpression(argument, comma));
            }

            // Ensuring correct indexing
            index      = endIndex + 1;
            expression = new TypeGenericExpression(identifierToken, leftGeneric, rightGeneric, arguments);
        }
        else
        {
            if (index == endIndex - 1 && TypeAt(index, in tokens) == TokenType.LessThan)
            {
                leftGeneric = GetAndMoveToNext(ref index, in tokens);
                expression  = new TypeGenericExpression(identifierToken, leftGeneric, Token.None);
            }
            else
            {
                leftGeneric = GetAndMoveToNext(ref index, in tokens);
                rightGeneric = tokens[endIndex].Type == TokenType.GreaterThan
                    ? tokens[endIndex]
                    : Token.None;

                if (index < endIndex)
                    arguments = new List<TypeArgumentExpression>();

                while (index < endIndex)
                {
                    var argument = ParseType(ref index, in tokens);
                    var comma = TypeAt(index, in tokens) == TokenType.Comma
                        ? GetAndMoveToNext(ref index, in tokens)
                        : Token.None;
                    arguments!.Add(new TypeArgumentExpression(argument, comma));
                }

                expression = new TypeGenericExpression(identifierToken, leftGeneric, rightGeneric, arguments);
            }

            // Ensuring correct indexing
            index = endIndex;
        }

        return expression;
    }

    private static PrimaryExpression ParseFunctionCallExpression(ref int index, in Token[] tokens, in Token identifier)
    {
        Token                     leftParen;
        Token                     rightParen;
        List<ArgumentExpression>? arguments = null;

        FunctionCallExpression functionCallExpression;

        if (ParsingUtilities.TryGetBalancedBrackets(in tokens, index, out var endIndex))
        {
            leftParen  = GetAndMoveToNext(ref index, in tokens);
            rightParen = tokens[endIndex];

            if (index < endIndex)
                arguments = new List<ArgumentExpression>();

            while (index < endIndex)
            {
                var argument = Parse(ref index, in tokens);
                var comma = TypeAt(index, in tokens) == TokenType.Comma
                    ? GetAndMoveToNext(ref index, in tokens)
                    : Token.None;

                arguments!.Add(new ArgumentExpression(argument, comma));
            }

            index                  = endIndex + 1;
            functionCallExpression = new FunctionCallExpression(identifier, leftParen, rightParen, arguments);
        }
        else
        {
            if (index == endIndex - 1 && TypeAt(index, in tokens) == TokenType.LeftParenthesis)
            {
                leftParen              = GetAndMoveToNext(ref index, in tokens);
                functionCallExpression = new FunctionCallExpression(identifier, leftParen, Token.None);
            }
            else
            {
                leftParen = GetAndMoveToNext(ref index, in tokens);
                rightParen = tokens[endIndex].Type == TokenType.RightParenthesis
                    ? tokens[endIndex]
                    : Token.None;

                if (index < endIndex)
                    arguments = new List<ArgumentExpression>();

                while (index < endIndex)
                {
                    var argument = Parse(ref index, in tokens);
                    var comma = TypeAt(index, in tokens) == TokenType.Comma
                        ? GetAndMoveToNext(ref index, in tokens)
                        : Token.None;

                    arguments!.Add(new ArgumentExpression(argument, comma));
                }

                functionCallExpression = new FunctionCallExpression(identifier, leftParen, rightParen, arguments);
            }

            index = endIndex;
        }

        return new PrimaryExpression(functionCallExpression);
    }

    private static PrimaryExpression ParseIndexerExpression(ref int index, in Token[] tokens, in Token identifier)
    {
        Token        leftBracket;
        Token        rightBracket;
        IExpression? expression = null;

        IndexerExpression indexerExpression;

        if (ParsingUtilities.TryGetBalancedBrackets(in tokens, index, out var endIndex))
        {
            leftBracket = GetAndMoveToNext(ref index, in tokens);

            if (TypeAt(index, in tokens) != TokenType.RightBracket)
                expression = Parse(ref index, in tokens);

            rightBracket = tokens[endIndex];

            // Ensuring correct indexing
            index             = endIndex + 1;
            indexerExpression = new IndexerExpression(identifier, leftBracket, rightBracket, expression);
        }
        else
        {
            if (index == endIndex - 1 && TypeAt(index, in tokens) == TokenType.LeftBracket)
            {
                leftBracket       = GetAndMoveToNext(ref index, in tokens);
                indexerExpression = new IndexerExpression(identifier, leftBracket, Token.None, expression);
            }
            else
            {
                leftBracket = GetAndMoveToNext(ref index, in tokens);
                expression  = Parse(ref index, in tokens);
                rightBracket = tokens[endIndex].Type == TokenType.RightBracket
                    ? tokens[endIndex]
                    : Token.None;

                // Ensuring correct indexing
                index             = endIndex;
                indexerExpression = new IndexerExpression(identifier, leftBracket, rightBracket, expression);
            }
        }

        return new PrimaryExpression(indexerExpression);
    }

    private static NumberLiteralExpression ParseNumberExpression(ref int index, in Token[] tokens) =>
        new(GetAndMoveToNext(ref index, in tokens));

    private static CharacterLiteralExpression ParseCharacterExpression(ref int index, in Token[] tokens) =>
        new(GetAndMoveToNext(ref index, in tokens));

    private static StringLiteralExpression ParseStringExpression(ref int index, in Token[] tokens) =>
        new(GetAndMoveToNext(ref index, in tokens));
}