using System.Collections.Generic;
using System.Text;

namespace DummyLang.LexicalAnalysis;

public sealed class Lexer
{
    private static readonly Dictionary<string, TokenType> Keywords = new()
    {
        { "var", TokenType.Var },
        { "const", TokenType.Const },
        { "fun", TokenType.Fun },
        { "return", TokenType.Return }
    };

    private readonly string _source;

    private int _position;
    private char _current;

    public Lexer(string source)
    {
        _source = source;
        _position = 0;
        _current = '\0';
    }

    public Token ReadNext()
    {
        if (_position >= _source.Length)
        {
            return new Token(TokenType.Eof, string.Empty);
        }

        _current = _source[_position];
        _position++;

        return _current switch
        {
            ',' => new Token(TokenType.Comma, ","),
            ';' => new Token(TokenType.Semicolon, ";"),
            ':' => new Token(TokenType.Colon, ":"),
            '=' => new Token(TokenType.Assign, "="),
            '+' => new Token(TokenType.Plus, "+"),
            '-' => new Token(TokenType.Minus, "-"),
            '*' => new Token(TokenType.Star, "*"),
            '/' => new Token(TokenType.Slash, "/"),
            '<' => new Token(TokenType.LessThan, "<"),
            '>' => new Token(TokenType.GreaterThan, ">"),
            '(' => new Token(TokenType.LeftParen, "("),
            ')' => new Token(TokenType.RightParen, ")"),
            '{' => new Token(TokenType.LeftBrace, "{"),
            '}' => new Token(TokenType.RightBrace, "}"),
            '[' => new Token(TokenType.LeftBracket, "["),
            ']' => new Token(TokenType.RightBracket, "]"),
            _ => char.IsLetter(_current) || _current == '_'
                ? ReadWord()
                : char.IsDigit(_current)
                    ? ReadNumber()
                    : ReadOther()
        };
    }

    private Token ReadWord()
    {
        var sb = new StringBuilder();
        sb.Append(_current);

        while (_position < _source.Length)
        {
            var current = _source[_position];

            if (!char.IsLetterOrDigit(current) && current != '_')
            {
                break;
            }

            sb.Append(current);
            _position++;
        }

        var word = sb.ToString();

        return new Token(Keywords.GetValueOrDefault(word, TokenType.Identifier), word);
    }

    private Token ReadNumber()
    {
        var hasDot = false;
        var sb = new StringBuilder();
        sb.Append(_current);

        while (_position < _source.Length)
        {
            var current = _source[_position];

            if (!char.IsDigit(current) && current != '.')
            {
                break;
            }

            if (current == '.')
            {
                if (hasDot)
                {
                    return new Token(TokenType.Invalid, string.Empty);
                }

                hasDot = true;
            }

            sb.Append(current);
            _position++;
        }

        return new Token(TokenType.Number, sb.ToString());
    }

    private Token ReadOther()
    {
        while (char.IsWhiteSpace(_current))
        {
            if (_position >= _source.Length)
            {
                return new Token(TokenType.Eof, string.Empty);
            }

            _current = _source[_position];
            _position++;
        }

        _position--;

        return ReadNext();
    }
}