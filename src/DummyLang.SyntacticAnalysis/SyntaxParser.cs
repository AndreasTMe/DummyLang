using DummyLang.LexicalAnalysis;
using DummyLang.LexicalAnalysis.Extensions;
using DummyLang.SyntacticAnalysis.Expressions;
using DummyLang.SyntacticAnalysis.Utilities;
using System.Collections.Generic;

namespace DummyLang.SyntacticAnalysis;

public class SyntaxParser
{
    private readonly Tokenizer _tokenizer = new();

    private List<Token> _tokens = [];
    private Token _current = Token.None;
    private int _index;

    private Token Current
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
            if (Current.IsEof() || Current.IsInvalid())
            {
                break;
            }

            var node = ParseExpression();
            tree.Add(node);

            _index++;
        }

        _tokens.Clear();

        return tree;
    }

    private Token GetAndMoveToNext() => _index < _tokens.Count ? _tokens[_index++] : Token.None;

    private Token LookAtNext() => LookAt(1);

    private Token LookAt(int position)
    {
        var index = _index + position;

        if (index < 0)
        {
            return Token.None;
        }

        return index >= _tokens.Count
            ? _tokens[^1]
            : _tokens[index];
    }
    
    private Expression ParseExpression(OperatorPrecedence previousPrecedence = 0)
    {
        var left = ParseExpressionBasedOnCurrentToken();

        while (true)
        {
            var currentPrecedence = GetOperatorPrecedence();
            if (currentPrecedence == OperatorPrecedence.None)
            {
                return new InvalidExpression<Expression>($"Invalid token: {Current.Value} at {Current.Position}");
            }

            if (currentPrecedence <= previousPrecedence)
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
            case TokenType.PlusPlus:
            case TokenType.MinusMinus:
            {
                return new InvalidExpression<Expression>("Not supported expression type.");
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
            case TokenType.Integer:
                return new LiteralExpression(GetAndMoveToNext());
            default:
                return new InvalidExpression<Expression>("Not supported expression type.");
        }
    }

    private OperatorPrecedence GetOperatorPrecedence()
    {
        switch (Current.Type)
        {
            case TokenType.Plus:
            case TokenType.Minus:
                return OperatorPrecedence.Additive;
            case TokenType.Star:
            case TokenType.Slash:
                return OperatorPrecedence.Multiplicative;
            default:
                return OperatorPrecedence.None;
        }
    }
}