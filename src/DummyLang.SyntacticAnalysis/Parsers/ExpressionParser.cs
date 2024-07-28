using DummyLang.LexicalAnalysis;
using DummyLang.LexicalAnalysis.Extensions;
using DummyLang.SyntacticAnalysis.Abstractions;
using DummyLang.SyntacticAnalysis.Expressions;
using DummyLang.SyntacticAnalysis.Extensions;
using DummyLang.SyntacticAnalysis.Utilities;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace DummyLang.SyntacticAnalysis.Parsers;

internal static class ExpressionParser
{
    public static IExpression Parse(ref int index, in Token[] tokens, OperatorPrecedence previousPrecedence = 0)
    {
        var left = ParseExpressionBasedOnCurrentToken(ref index, in tokens);

        while (Parser.TokenAt(index, in tokens).IsBinaryOperator())
        {
            var currentPrecedence = tokens[index].GetOperatorPrecedence();
            if (currentPrecedence == OperatorPrecedence.None || currentPrecedence <= previousPrecedence)
                break;

            var op    = Parser.GetAndMoveToNext(ref index, in tokens);
            var right = Parse(ref index, in tokens, currentPrecedence);

            left = new BinaryExpression(left, op, right);
        }

        return left;
    }

    public static ITypeExpression ParseType(ref int index, in Token[] tokens, OperatorPrecedence previousPrecedence = 0)
    {
        var left = ParseTypeExpressionBasedOnCurrentToken(ref index, in tokens);

        while (Parser.TokenAt(index, in tokens).IsBitwiseOperator())
        {
            var currentPrecedence = tokens[index].GetOperatorPrecedence();
            if (currentPrecedence == OperatorPrecedence.None || currentPrecedence <= previousPrecedence)
                break;

            var op    = Parser.GetAndMoveToNext(ref index, in tokens);
            var right = ParseType(ref index, in tokens, currentPrecedence);

            left = new TypeBinaryExpression(left, op, right);
        }

        return left;
    }

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
            TokenType.DoubleDot => ParseRangeExpression(ref index, in tokens),
            TokenType.LeftParenthesis => ParseParenthesizedExpression(ref index, in tokens),
            TokenType.Identifier => ParseIdentifierRelatedExpressions(ref index, in tokens),
            TokenType.Integer or TokenType.Real => ParseNumberExpression(ref index, in tokens),
            TokenType.Character => ParseCharacterExpression(ref index, in tokens),
            TokenType.String => ParseStringExpression(ref index, in tokens),
            _ => new UnexpectedTokenExpression(Parser.GetAndMoveToNext(ref index, in tokens))
        };
    }

    private static ITypeExpression ParseTypeExpressionBasedOnCurrentToken(ref int index, in Token[] tokens)
    {
        Debug.Assert(index < tokens.Length);

        return tokens[index].Type switch
        {
            TokenType.LeftParenthesis => IsFunctionType(index, in tokens)
                ? ParseFunctionTypeExpression(ref index, in tokens)
                : ParseParenthesizedExpression(ref index, in tokens, true),
            TokenType.Identifier => ParseTypeIdentifierExpression(ref index, in tokens),
            TokenType.Integer or TokenType.Real => ParseNumberExpression(ref index, in tokens),
            TokenType.Character => ParseCharacterExpression(ref index, in tokens),
            TokenType.String => ParseStringExpression(ref index, in tokens),
            _ => new UnexpectedTokenExpression(Parser.GetAndMoveToNext(ref index, in tokens))
        };
    }

    private static bool IsFunctionType(int index, in Token[] tokens)
    {
        while (Parser.TokenAt(index, in tokens).IsAllowedInTypes())
            index++;

        return Parser.TypeAt(index, in tokens) == TokenType.LambdaAssign;
    }

    private static UnaryExpression ParseUnaryExpression(ref int index, in Token[] tokens)
    {
        var unaryOperator = Parser.GetAndMoveToNext(ref index, in tokens);
        var expression    = ParseExpressionBasedOnCurrentToken(ref index, in tokens);

        return new UnaryExpression(unaryOperator, expression);
    }

    private static RangeExpression ParseRangeExpression(ref int index,
                                                        in Token[] tokens,
                                                        IExpression? startExpression = null)
    {
        var range = Parser.GetAndMoveToNext(ref index, in tokens);

        IExpression? endExpression = null;
        if (Parser.TypeAt(index, in tokens) != TokenType.RightBracket)
            endExpression = Parse(ref index, in tokens);

        return new RangeExpression(startExpression, range, endExpression);
    }

    private static ParenthesisedExpression ParseParenthesizedExpression(ref int index,
                                                                        in Token[] tokens,
                                                                        bool isType = false)
    {
        var   leftParen = Parser.GetAndMoveToNext(ref index, in tokens);
        Token rightParen;

        if (isType)
        {
            var typeExpression = ParseType(ref index, in tokens);
            rightParen = Parser.GetAndMoveToNextOrDefault(TokenType.RightParenthesis, ref index, in tokens);

            return new ParenthesisedExpression(leftParen, typeExpression, rightParen);
        }

        var expression = Parse(ref index, in tokens);
        rightParen = Parser.GetAndMoveToNextOrDefault(TokenType.RightParenthesis, ref index, in tokens);

        return new ParenthesisedExpression(leftParen, expression, rightParen);
    }

    private static IExpression ParseIdentifierRelatedExpressions(ref int index, in Token[] tokens)
    {
        var         identifierToken = Parser.GetAndMoveToNext(ref index, in tokens);
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

                expression = new PrimaryExpression(expression, Parser.GetAndMoveToNext(ref index, in tokens));

                while (Parser.TokenAt(index, in tokens) is { Type: TokenType.PlusPlus or TokenType.MinusMinus })
                    expression = new PrimaryExpression(expression, Parser.GetAndMoveToNext(ref index, in tokens));

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
                var access = Parser.GetAndMoveToNext(ref index, in tokens);
                return new MemberAccessExpression(
                    expression,
                    access,
                    ParseIdentifierRelatedExpressions(ref index, in tokens));
            case TokenType.DoubleDot:
                return ParseRangeExpression(ref index, in tokens, expression);
            default:
                return expression;
        }
    }

    private static TypeFunctionExpression ParseFunctionTypeExpression(ref int index, in Token[] tokens)
    {
        var leftParenthesis = Parser.GetAndMoveToNextOrDefault(TokenType.LeftParenthesis, ref index, in tokens);

        List<TypeParameterExpression>? inputTypes = null;
        if (Parser.TypeAt(index, in tokens) == TokenType.Identifier)
        {
            var parameter = Parser.GetAndMoveToNext(ref index, in tokens);
            var colon     = Parser.GetAndMoveToNextOrDefault(TokenType.Colon, ref index, in tokens);

            ITypeExpression? typeValue = null;
            if (Parser.TypeAt(index, in tokens) != TokenType.Comma)
                typeValue = ParseTypeExpressionBasedOnCurrentToken(ref index, in tokens);

            var comma = Parser.GetAndMoveToNextOrDefault(TokenType.Comma, ref index, in tokens);
            inputTypes = new List<TypeParameterExpression> { new(parameter, colon, typeValue, comma) };

            while (comma.Type == TokenType.Comma)
            {
                parameter = Parser.GetAndMoveToNext(ref index, in tokens);
                colon     = Parser.GetAndMoveToNextOrDefault(TokenType.Colon, ref index, in tokens);

                typeValue = null;
                if (Parser.TypeAt(index, in tokens) != TokenType.Comma)
                    typeValue = ParseTypeExpressionBasedOnCurrentToken(ref index, in tokens);

                comma = Parser.GetAndMoveToNextOrDefault(TokenType.Comma, ref index, in tokens);

                inputTypes.Add(new TypeParameterExpression(parameter, colon, typeValue, comma));
            }
        }

        var rightParenthesis = Parser.GetAndMoveToNextOrDefault(TokenType.RightParenthesis, ref index, in tokens);
        var lambdaAssign     = Parser.GetAndMoveToNextOrDefault(TokenType.LambdaAssign, ref index, in tokens);

        List<TypeArgumentExpression>? outputTypes = null;
        if (Parser.TokenAt(index, in tokens).IsIdentifierOrLiteral())
        {
            var outputType = ParseType(ref index, in tokens);
            var comma      = Parser.GetAndMoveToNextOrDefault(TokenType.Comma, ref index, in tokens);

            outputTypes = new List<TypeArgumentExpression> { new(outputType, comma) };

            while (comma.Type == TokenType.Comma)
            {
                outputType = ParseType(ref index, in tokens);
                comma      = Parser.GetAndMoveToNextOrDefault(TokenType.Comma, ref index, in tokens);

                outputTypes.Add(new TypeArgumentExpression(outputType, comma));
            }
        }

        return new TypeFunctionExpression(
            leftParenthesis,
            inputTypes,
            rightParenthesis,
            lambdaAssign,
            outputTypes);
    }

    private static ITypeExpression ParseTypeIdentifierExpression(ref int index, in Token[] tokens)
    {
        var identifierToken = Parser.GetAndMoveToNext(ref index, in tokens);

        if (Parser.TypeAt(index, in tokens) != TokenType.LessThan)
            return new TypeIdentifierExpression(identifierToken);

        Token                         leftGeneric;
        Token                         rightGeneric;
        List<TypeArgumentExpression>? arguments = null;

        TypeGenericExpression expression;

        if (ParsingUtilities.TryGetBalancedTypeBrackets(in tokens, index, out var endIndex))
        {
            leftGeneric  = Parser.GetAndMoveToNext(ref index, in tokens);
            rightGeneric = tokens[endIndex];

            if (index < endIndex)
                arguments = new List<TypeArgumentExpression>();

            while (index < endIndex)
            {
                var argument = ParseType(ref index, in tokens);
                var comma    = Parser.GetAndMoveToNextOrDefault(TokenType.Comma, ref index, in tokens);

                arguments!.Add(new TypeArgumentExpression(argument, comma));
            }

            // Ensuring correct indexing
            index      = endIndex + 1;
            expression = new TypeGenericExpression(identifierToken, leftGeneric, rightGeneric, arguments);
        }
        else
        {
            if (index == endIndex - 1 && Parser.TypeAt(index, in tokens) == TokenType.LessThan)
            {
                leftGeneric = Parser.GetAndMoveToNext(ref index, in tokens);
                expression  = new TypeGenericExpression(identifierToken, leftGeneric, Token.None);
            }
            else
            {
                leftGeneric  = Parser.GetAndMoveToNext(ref index, in tokens);
                rightGeneric = Parser.GetAndMoveToNextOrDefault(TokenType.GreaterThan, ref index, in tokens);

                if (index < endIndex)
                    arguments = new List<TypeArgumentExpression>();

                while (index < endIndex)
                {
                    var argument = ParseType(ref index, in tokens);
                    var comma    = Parser.GetAndMoveToNextOrDefault(TokenType.Comma, ref index, in tokens);

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
            leftParen  = Parser.GetAndMoveToNext(ref index, in tokens);
            rightParen = tokens[endIndex];

            if (index < endIndex)
                arguments = new List<ArgumentExpression>();

            while (index < endIndex)
            {
                var argument = Parse(ref index, in tokens);
                var comma    = Parser.GetAndMoveToNextOrDefault(TokenType.Comma, ref index, in tokens);

                arguments!.Add(new ArgumentExpression(argument, comma));
            }

            index                  = endIndex + 1;
            functionCallExpression = new FunctionCallExpression(identifier, leftParen, rightParen, arguments);
        }
        else
        {
            if (index == endIndex - 1 && Parser.TypeAt(index, in tokens) == TokenType.LeftParenthesis)
            {
                leftParen              = Parser.GetAndMoveToNext(ref index, in tokens);
                functionCallExpression = new FunctionCallExpression(identifier, leftParen, Token.None);
            }
            else
            {
                leftParen  = Parser.GetAndMoveToNext(ref index, in tokens);
                rightParen = Parser.GetAndMoveToNextOrDefault(TokenType.RightParenthesis, ref index, in tokens);

                if (index < endIndex)
                    arguments = new List<ArgumentExpression>();

                while (index < endIndex)
                {
                    var argument = Parse(ref index, in tokens);
                    var comma    = Parser.GetAndMoveToNextOrDefault(TokenType.Comma, ref index, in tokens);

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
        Token                          leftBracket;
        Token                          rightBracket;
        List<IndexArgumentExpression>? indices = null;

        IndexerExpression indexerExpression;

        if (ParsingUtilities.TryGetBalancedBrackets(in tokens, index, out var endIndex))
        {
            leftBracket = Parser.GetAndMoveToNext(ref index, in tokens);

            if (index < endIndex)
                indices = new List<IndexArgumentExpression>();

            while (index < endIndex)
            {
                var argument = Parse(ref index, in tokens);
                var comma    = Parser.GetAndMoveToNextOrDefault(TokenType.Comma, ref index, in tokens);

                indices!.Add(new IndexArgumentExpression(argument, comma));
            }

            rightBracket = tokens[endIndex];

            // Ensuring correct indexing
            index             = endIndex + 1;
            indexerExpression = new IndexerExpression(identifier, leftBracket, rightBracket, indices);
        }
        else
        {
            if (index == endIndex - 1 && Parser.TypeAt(index, in tokens) == TokenType.LeftBracket)
            {
                leftBracket       = Parser.GetAndMoveToNext(ref index, in tokens);
                indexerExpression = new IndexerExpression(identifier, leftBracket, Token.None, indices);
            }
            else
            {
                leftBracket = Parser.GetAndMoveToNext(ref index, in tokens);

                if (index < endIndex)
                    indices = new List<IndexArgumentExpression>();

                while (index < endIndex)
                {
                    var argument = Parse(ref index, in tokens);
                    var comma    = Parser.GetAndMoveToNextOrDefault(TokenType.Comma, ref index, in tokens);

                    indices!.Add(new IndexArgumentExpression(argument, comma));
                }

                rightBracket = Parser.GetAndMoveToNextOrDefault(TokenType.RightBracket, ref index, in tokens);

                // Ensuring correct indexing
                index             = endIndex;
                indexerExpression = new IndexerExpression(identifier, leftBracket, rightBracket, indices);
            }
        }

        return new PrimaryExpression(indexerExpression);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static NumberLiteralExpression ParseNumberExpression(ref int index, in Token[] tokens) =>
        new(Parser.GetAndMoveToNext(ref index, in tokens));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static CharacterLiteralExpression ParseCharacterExpression(ref int index, in Token[] tokens) =>
        new(Parser.GetAndMoveToNext(ref index, in tokens));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static StringLiteralExpression ParseStringExpression(ref int index, in Token[] tokens) =>
        new(Parser.GetAndMoveToNext(ref index, in tokens));
}