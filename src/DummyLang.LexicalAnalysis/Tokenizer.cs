using DummyLang.LexicalAnalysis.Extensions;
using System.Collections.Generic;
using System.Diagnostics;
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
    private int _line;
    private int _column;

    public void Use(in string? newSource)
    {
        _source = newSource ?? string.Empty;
        _index = 0;
        _line = 1;
        _column = 1;
    }

    public Token ReadNext()
    {
        if (_index >= _source.Length)
        {
            return new Token(TokenType.Eof, string.Empty, TokenPosition.At(_line, _column, _source.Length, 0));
        }

        var current = _source[_index];

        switch (current)
        {
            case ',':
                return GenerateToken(TokenType.Comma);
            case '.':
                return GenerateToken(TokenType.Dot);
            case ';':
                return GenerateToken(TokenType.Semicolon);
            case ':':
                return GenerateToken(TokenType.Colon);
            case '=':
                return GenerateTokenBasedOnNext(TokenType.Assign, TokenType.Equal);
            case '!':
                return GenerateTokenBasedOnNext(TokenType.Bang, TokenType.NotEqual);
            case '+':
                return GenerateTokenBasedOnNext(TokenType.Plus, TokenType.PlusPlus);
            case '-':
                return GenerateTokenBasedOnNext(TokenType.Minus, TokenType.MinusMinus);
            case '*':
                return GenerateToken(TokenType.Star);
            case '/':
                return GenerateToken(TokenType.Slash);
            case '<':
                return GenerateTokenBasedOnNext(
                    TokenType.LessThan,
                    TokenType.LessThanOrEqual, TokenType.LeftBitShift);
            case '>':
                return GenerateTokenBasedOnNext(
                    TokenType.GreaterThan,
                    TokenType.GreaterThanOrEqual, TokenType.RightBitShift);
            case '(':
                return GenerateToken(TokenType.LeftParen);
            case ')':
                return GenerateToken(TokenType.RightParen);
            case '{':
                return GenerateToken(TokenType.LeftBrace);
            case '}':
                return GenerateToken(TokenType.RightBrace);
            case '[':
                return GenerateToken(TokenType.LeftBracket);
            case ']':
                return GenerateToken(TokenType.RightBracket);
            default:
                return char.IsLetter(current) || current == '_'
                    ? ReadWord(current)
                    : char.IsDigit(current)
                        ? ReadNumber(current)
                        : ReadOther(current);
        }
    }

    private Token GenerateToken(TokenType type)
    {
        var stringToken = type.GetStringToken();
        var stringTokenLength = stringToken.Length;
        if (stringTokenLength < 1)
        {
            throw new UnreachableException("This should never happen.");
        }

        var tokenPosition = stringTokenLength == 1
            ? TokenPosition.At(_line, _column, _index)
            : TokenPosition.At(_line, _column, _index, stringTokenLength);

        _index += stringTokenLength;
        _column += stringTokenLength;

        return new Token(type, stringToken, tokenPosition);
    }

    private Token GenerateTokenBasedOnNext(
        TokenType type,
        params TokenType[] possibleTypes)
    {
        var sourceFromCurrentPosition = _source[_index..];

        foreach (var possibleType in possibleTypes)
        {
            var stringToken = possibleType.GetStringToken();
            if (!sourceFromCurrentPosition.StartsWith(stringToken))
            {
                continue;
            }

            return GenerateToken(possibleType);
        }

        return GenerateToken(type);
    }

    private Token ReadWord(char current)
    {
        var startIndex = _index;
        var startColumn = _column;
        StepForward();

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
            StepForward();
        }

        var word = sb.ToString();

        return new Token(
            KeywordTokens.GetValueOrDefault(word, TokenType.Identifier),
            word,
            TokenPosition.At(_line, startColumn, startIndex, word.Length));
    }

    private Token ReadNumber(char current)
    {
        var startIndex = _index;
        var startColumn = _column;
        StepForward();

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
            StepForward();
        }

        var number = sb.ToString();

        return new Token(
            TokenType.Integer,
            number,
            TokenPosition.At(_line, startColumn, startIndex, number.Length));
    }

    private Token ReadOther(char current)
    {
        var startIndex = _index;
        var count = 1;

        while (char.IsWhiteSpace(current))
        {
            StepForward(current == '\n');

            if (_index >= _source.Length)
            {
                return new Token(
                    TokenType.Eof,
                    string.Empty,
                    TokenPosition.At(_line, _column, startIndex, count));
            }

            current = _source[_index];
            count++;
        }

        return ReadNext();
    }

    private void StepForward(bool isNewLine = false)
    {
        _index++;

        if (isNewLine)
        {
            _line++;
            _column = 1;
        }
        else
        {
            _column++;
        }
    }
}