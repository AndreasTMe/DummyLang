using System.Collections.Generic;
using System.Text;

namespace DummyLang.LexicalAnalysis;

public sealed class Tokenizer
{
    private static readonly Dictionary<string, TokenType> KeywordTokens = new()
    {
        { Keywords.Var, TokenType.Var },
        { Keywords.Const, TokenType.Const },
        { Keywords.Fun, TokenType.Fun },
        { Keywords.If, TokenType.If },
        { Keywords.Else, TokenType.Else },
        { Keywords.Break, TokenType.Break },
        { Keywords.While, TokenType.While },
        { Keywords.Continue, TokenType.Continue },
        { Keywords.Return, TokenType.Return }
    };

    private string _source = string.Empty;
    private int _index;

    public void Use(in string? newSource)
    {
        _source = newSource ?? string.Empty;
        _index = 0;
    }

    public Token ReadNext()
    {
        if (_index >= _source.Length)
        {
            return new Token(TokenType.Eof, string.Empty, TokenPosition.At(_source.Length));
        }

        var current = _source[_index];
        var currentIndex = _index;
        _index++;

        switch (current)
        {
            case ',':
                return new Token(TokenType.Comma, ",", TokenPosition.At(currentIndex));
            case '.':
                return new Token(TokenType.Dot, ".", TokenPosition.At(currentIndex));
            case ';':
                return new Token(TokenType.Semicolon, ";", TokenPosition.At(currentIndex));
            case ':':
                return new Token(TokenType.Colon, ":", TokenPosition.At(currentIndex));
            case '=':
                if (PeekNext() != '=')
                {
                    return new Token(TokenType.Assign, "=", TokenPosition.At(currentIndex));
                }

                _index++;
                return new Token(TokenType.Equal, "==", TokenPosition.At(currentIndex, _index - 1));
            case '!':
                if (PeekNext() != '=')
                {
                    return new Token(TokenType.Bang, "!", TokenPosition.At(currentIndex));
                }

                _index++;
                return new Token(TokenType.NotEqual, "!=", TokenPosition.At(currentIndex, _index - 1));
            case '+':
                return new Token(TokenType.Plus, "+", TokenPosition.At(currentIndex));
            case '-':
                return new Token(TokenType.Minus, "-", TokenPosition.At(currentIndex));
            case '*':
                return new Token(TokenType.Star, "*", TokenPosition.At(currentIndex));
            case '/':
                return new Token(TokenType.Slash, "/", TokenPosition.At(currentIndex));
            case '<':
                if (PeekNext() != '=')
                {
                    return new Token(TokenType.LessThan, "<", TokenPosition.At(currentIndex));
                }

                _index++;
                return new Token(TokenType.LessThanOrEqual, "<=", TokenPosition.At(currentIndex, _index - 1));
            case '>':
                if (PeekNext() != '=')
                {
                    return new Token(TokenType.GreaterThan, ">", TokenPosition.At(currentIndex));
                }

                _index++;
                return new Token(TokenType.GreaterThanOrEqual, ">=", TokenPosition.At(currentIndex, _index - 1));
            case '(':
                return new Token(TokenType.LeftParen, "(", TokenPosition.At(currentIndex));
            case ')':
                return new Token(TokenType.RightParen, ")", TokenPosition.At(currentIndex));
            case '{':
                return new Token(TokenType.LeftBrace, "{", TokenPosition.At(currentIndex));
            case '}':
                return new Token(TokenType.RightBrace, "}", TokenPosition.At(currentIndex));
            case '[':
                return new Token(TokenType.LeftBracket, "[", TokenPosition.At(currentIndex));
            case ']':
                return new Token(TokenType.RightBracket, "]", TokenPosition.At(currentIndex));
            default:
                return char.IsLetter(current) || current == '_'
                    ? ReadWord(current)
                    : char.IsDigit(current)
                        ? ReadNumber(current)
                        : ReadOther(current);
        }
    }

    private Token ReadWord(char current)
    {
        var startIndex = _index - 1;

        var sb = new StringBuilder();
        sb.Append(current);

        while (_index < _source.Length)
        {
            current = _source[_index];

            if (!char.IsLetterOrDigit(current) && current != '_')
            {
                break;
            }

            sb.Append(current);
            _index++;
        }

        var word = sb.ToString();

        return new Token(
            KeywordTokens.GetValueOrDefault(word, TokenType.Identifier),
            word,
            TokenPosition.At(startIndex, _index - 1));
    }

    private Token ReadNumber(char current)
    {
        var startIndex = _index - 1;

        var sb = new StringBuilder();
        sb.Append(current);

        while (_index < _source.Length)
        {
            current = _source[_index];

            if (!char.IsDigit(current))
            {
                break;
            }

            sb.Append(current);
            _index++;
        }

        return new Token(TokenType.Number, sb.ToString(), TokenPosition.At(startIndex, _index - 1));
    }

    private Token ReadOther(char current)
    {
        var startIndex = _index - 1;
        
        while (char.IsWhiteSpace(current))
        {
            if (_index >= _source.Length)
            {
                return new Token(TokenType.Eof, string.Empty, TokenPosition.At(startIndex, _index));
            }

            current = _source[_index];
            _index++;
        }

        _index--;

        return ReadNext();
    }

    private char PeekNext() => _index < _source.Length ? _source[_index] : '\0';
}