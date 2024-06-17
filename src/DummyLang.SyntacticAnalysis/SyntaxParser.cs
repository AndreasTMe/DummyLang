using DummyLang.LexicalAnalysis;
using DummyLang.LexicalAnalysis.Extensions;
using DummyLang.SyntacticAnalysis.Expressions;
using DummyLang.SyntacticAnalysis.Expressions.Literals;
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

    private Expression ParseExpression()
    {
        return ParseAdditiveExpression();
    }

    private Expression ParseAdditiveExpression()
    {
        var left = ParseMultiplicativeExpression();

        while (Current.Type == TokenType.Plus || Current.Type == TokenType.Minus)
        {
            var op = GetAndMoveToNext();
            var right = ParseMultiplicativeExpression();

            left = new BinaryExpression(left, op, right);
        }

        return left;
    }

    private Expression ParseMultiplicativeExpression()
    {
        var left = ParsePrimaryExpression();

        while (Current.Type == TokenType.Star || Current.Type == TokenType.Slash)
        {
            var op = GetAndMoveToNext();
            var right = ParsePrimaryExpression();

            left = new BinaryExpression(left, op, right);
        }

        return left;
    }

    private Expression ParsePrimaryExpression()
    {
        if (Current.Type == TokenType.Number)
        {
            return new NumberLiteral(GetAndMoveToNext());
        }

        return default!;
    }
}