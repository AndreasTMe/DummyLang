using DummyLang.LexicalAnalysis;
using DummyLang.LexicalAnalysis.Extensions;
using DummyLang.SyntacticAnalysis.Expressions;
using DummyLang.SyntacticAnalysis.Utilities;
using System;
using System.Collections.Generic;

namespace DummyLang.SyntacticAnalysis;

public class SyntaxParser
{
    private readonly Tokenizer _tokenizer = new();

    private List<Token> _tokens = [];
    private Token _current = Token.None;
    private int _index;

    internal Token Current
    {
        get
        {
            if (_index < _tokens.Count)
            {
                _current = _tokens[_index];
            }

            return _current;
        }
    }

    internal int TotalTokens => _tokens.Count;

    public SyntaxParser Feed(string source)
    {
        _tokenizer.Use(source);

        var tokens = new List<Token>();
        var token = _tokenizer.ReadNext();

        while (!token.IsEof())
        {
            tokens.Add(token);
            token = _tokenizer.ReadNext();
        }

        tokens.Add(token);
        _tokens = tokens;

        return this;
    }

    public SyntaxTree GenerateSyntax()
    {
        SyntaxTree tree = new();

        while (_index < _tokens.Count)
        {
            var lastIndex = _index;

            if (Current.IsEof() || Current.IsInvalid())
            {
                break;
            }

            var node = ParseExpression();
            tree.Add(node);

            if (lastIndex == _index)
            {
                MoveToNext();
            }
        }

        _tokens.Clear();

        return tree;
    }

    private void MoveToNext() => _index++;

    private Token GetAndMoveToNext() => _index < _tokens.Count ? _tokens[_index++] : Token.None;

    private Expression ParseExpression(OperatorPrecedence previousPrecedence = 0)
    {
        var left = ParseExpressionBasedOnCurrentToken();

        while (Current.IsBinaryOperator())
        {
            var currentPrecedence = GetOperatorPrecedence();
            if (currentPrecedence == OperatorPrecedence.None || currentPrecedence <= previousPrecedence)
            {
                break;
            }

            var op = GetAndMoveToNext();
            var right = ParseExpression(currentPrecedence);

            left = new BinaryExpression(left, op, right);
        }

        return left;
    }

    private Expression ParseExpressionBasedOnCurrentToken()
    {
        switch (Current.Type)
        {
            case TokenType.Plus:
            case TokenType.Minus:
            case TokenType.PlusPlus:
            case TokenType.MinusMinus:
            case TokenType.Bang:
            case TokenType.Tilde:
            case TokenType.Star:
            case TokenType.Ampersand:
            {
                var unaryOperator = GetAndMoveToNext();
                if (Current.Type == TokenType.Identifier)
                {
                    return new UnaryExpression(unaryOperator, new IdentifierExpression(GetAndMoveToNext()));
                }

                if ((unaryOperator.Type == TokenType.Plus || unaryOperator.Type == TokenType.Minus) &&
                    (Current.Type == TokenType.Integer || Current.Type == TokenType.Real))
                {
                    return new UnaryExpression(unaryOperator, ParseNumberExpression(Current.Type == TokenType.Integer));
                }

                return new InvalidExpression<UnaryExpression>("Invalid unary expression.");
            }
            case TokenType.LeftParen:
            {
                var leftParen = GetAndMoveToNext();
                var expression = ParseExpression();

                if (Current.Type == TokenType.RightParen)
                {
                    return new ParenthesisedExpression(leftParen, expression, GetAndMoveToNext());
                }

                var parenthesisedExpression = new ParenthesisedExpression(leftParen, expression, Token.None);
                return new InvalidExpression<ParenthesisedExpression>(parenthesisedExpression);
            }
            case TokenType.Identifier:
            {
                var identifierExpression = new IdentifierExpression(GetAndMoveToNext());

                if (Current.Type != TokenType.PlusPlus && Current.Type != TokenType.MinusMinus)
                {
                    return identifierExpression;
                }

                var expression = new PrimaryExpression(identifierExpression, GetAndMoveToNext());

                while (Current.Type == TokenType.PlusPlus || Current.Type == TokenType.MinusMinus)
                {
                    expression = new PrimaryExpression(expression, GetAndMoveToNext());
                }

                return expression;
            }
            case TokenType.Integer:
            case TokenType.Real:
                return ParseNumberExpression(Current.Type == TokenType.Integer);
            case TokenType.Character:
            {
                // TODO: Handle hex values
                // TODO: Handle invalid escaped characters
                var characterLiteralExpression = new CharacterLiteralExpression(GetAndMoveToNext());
                var characterValue = characterLiteralExpression.CharacterToken.Value;

                if (characterValue.Length < 3
                    || !characterValue.StartsWith('\'') || !characterValue.EndsWith('\'')
                    || (characterValue.Length == 3 && (characterValue[1] == '\\' || characterValue[1] == '\'')))
                {
                    return new InvalidExpression<CharacterLiteralExpression>(characterLiteralExpression);
                }

                return characterLiteralExpression;
            }
            case TokenType.String:
            {
                // TODO: Handle invalid escaped characters
                var stringLiteralExpression = new StringLiteralExpression(GetAndMoveToNext());
                var stringValue = stringLiteralExpression.StringToken.Value;

                if (stringValue.Length == 1
                    || !stringValue.StartsWith('\"') || !stringValue.EndsWith('\"')
                    || (stringValue.Length == 3 && stringValue[1] == '\\')
                    || !stringValue.Replace("\\\"", "").EndsWith('\"'))
                {
                    return new InvalidExpression<StringLiteralExpression>(stringLiteralExpression);
                }

                return stringLiteralExpression;
            }
            default:
                return new InvalidExpression<Expression>("Not supported expression type.");
        }
    }

    private NumberLiteralExpression ParseNumberExpression(bool isInteger)
    {
        if (isInteger)
        {
            var integerValue = Current.Value;
            var integerType = NumberType.Integer;

            if (integerValue.StartsWith("0x"))
            {
                integerType = NumberType.Hexadecimal;
            }
            else if (integerValue.StartsWith("0b"))
            {
                integerType = NumberType.Binary;
            }
            else if (integerValue.EndsWith("u", StringComparison.OrdinalIgnoreCase))
            {
                integerType = NumberType.UnsignedInteger;
            }
            else if (integerValue.EndsWith("l", StringComparison.OrdinalIgnoreCase))
            {
                integerType = NumberType.Long;
            }
            else if (integerValue.EndsWith("ul", StringComparison.OrdinalIgnoreCase))
            {
                integerType = NumberType.UnsignedLong;
            }

            return new NumberLiteralExpression(GetAndMoveToNext(), integerType);
        }

        var realValue = Current.Value;
        var realType = NumberType.Double;

        if (realValue.EndsWith("f", StringComparison.OrdinalIgnoreCase))
        {
            realType = NumberType.Float;
        }
        else if (realValue.EndsWith("d", StringComparison.OrdinalIgnoreCase))
        {
            realType = NumberType.Double;
        }
        else if (realValue.EndsWith("m", StringComparison.OrdinalIgnoreCase))
        {
            realType = NumberType.Decimal;
        }
        else if (realValue.Contains('e', StringComparison.OrdinalIgnoreCase))
        {
            realType = NumberType.WithExponent;
        }

        return new NumberLiteralExpression(GetAndMoveToNext(), realType);
    }

    private OperatorPrecedence GetOperatorPrecedence()
    {
        switch (Current.Type)
        {
            case TokenType.DoubleQuestionMark:
                return OperatorPrecedence.NullCoalescing;
            case TokenType.DoublePipe:
                return OperatorPrecedence.ConditionalOr;
            case TokenType.DoubleAmpersand:
                return OperatorPrecedence.ConditionalAnd;
            case TokenType.Pipe:
                return OperatorPrecedence.BitwiseOr;
            case TokenType.Caret:
                return OperatorPrecedence.BitwiseXOr;
            case TokenType.Ampersand:
                return OperatorPrecedence.BitwiseAnd;
            case TokenType.Assign:
                return OperatorPrecedence.Assignment;
            case TokenType.Equal:
            case TokenType.NotEqual:
                return OperatorPrecedence.Equality;
            case TokenType.LessThan:
            case TokenType.LessThanOrEqual:
            case TokenType.GreaterThan:
            case TokenType.GreaterThanOrEqual:
                return OperatorPrecedence.Relational;
            case TokenType.LeftBitShift:
            case TokenType.RightBitShift:
                return OperatorPrecedence.Bitshift;
            case TokenType.Plus:
            case TokenType.Minus:
                return OperatorPrecedence.Additive;
            case TokenType.Star:
            case TokenType.Slash:
            case TokenType.Percent:
                return OperatorPrecedence.Multiplicative;
            case TokenType.DoubleDot:
                return OperatorPrecedence.Range;
            default:
                return OperatorPrecedence.None;
        }
    }
}