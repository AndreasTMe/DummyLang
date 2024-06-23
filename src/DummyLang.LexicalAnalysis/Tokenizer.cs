using DummyLang.LexicalAnalysis.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace DummyLang.LexicalAnalysis;

public sealed partial class Tokenizer
{
    private static readonly Dictionary<TokenType, Regex[]> NumberPatterns = new()
    {
        { TokenType.Real, [RealNumberPattern()] },
        { TokenType.Integer, [BinaryNumberPattern(), HexadecimalNumberPattern(), IntegerNumberPattern()] }
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
            return new Token(TokenType.Eof, string.Empty, TokenPosition.At(_line, _column));
        }

        var current = _source[_index];

        switch (current)
        {
            case ',':
                return GenerateToken(TokenType.Comma);
            case '.':
                return GenerateTokenBasedOnNext(TokenType.Dot, TokenType.DoubleDot);
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
                return GenerateTokenBasedOnNext(
                    TokenType.Minus,
                    TokenType.MinusMinus, TokenType.PointerAccess);
            case '*':
                return GenerateToken(TokenType.Star);
            case '/':
                return GenerateToken(TokenType.Slash);
            case '%':
                return GenerateToken(TokenType.Percent);
            case '&':
                return GenerateTokenBasedOnNext(TokenType.Ampersand, TokenType.DoubleAmpersand);
            case '|':
                return GenerateTokenBasedOnNext(TokenType.Pipe, TokenType.DoublePipe);
            case '^':
                return GenerateToken(TokenType.Caret);
            case '?':
                return GenerateTokenBasedOnNext(TokenType.QuestionMark, TokenType.DoubleQuestionMark);
            case '<':
                return GenerateTokenBasedOnNext(
                    TokenType.LessThan,
                    TokenType.LessThanOrEqual, TokenType.LeftBitShift);
            case '>':
                return GenerateTokenBasedOnNext(
                    TokenType.GreaterThan,
                    TokenType.GreaterThanOrEqual, TokenType.RightBitShift);
            case '~':
                return GenerateToken(TokenType.Tilde);
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
            case '\'':
                return ReadCharacter();
            case '\"':
                return ReadString();
            default:
                return char.IsLetter(current) || current == '_'
                    ? ReadIdentifier()
                    : char.IsDigit(current)
                        ? ReadNumber()
                        : SkipWhiteSpace();
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

        var tokenPosition = TokenPosition.At(_line, _column);

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

    private Token ReadString()
    {
        var source = _source.AsSpan(_index);
        var sb = new StringBuilder();
        var escape = true;

        foreach (var character in source)
        {
            sb.Append(character);

            if ((character == '\"' && !escape) || character == '\n')
            {
                break;
            }

            escape = !escape && (character == '\\');
        }

        var stringValue = sb.ToString();
        if (stringValue.EndsWith(Environment.NewLine))
        {
            stringValue = stringValue[..^Environment.NewLine.Length];
        }
        
        var tokenPosition = TokenPosition.At(_line, _column);

        _index += stringValue.Length;
        _column += stringValue.Length;

        return new Token(TokenType.String, stringValue, tokenPosition);
    }

    private Token ReadCharacter()
    {
        var source = _source.AsSpan(_index);
        var sb = new StringBuilder();
        var current = 0;
        var maxIterations = 3;

        while (current < maxIterations && current < source.Length)
        {
            if (source[current] == '\n')
            {
                break;
            }
            
            sb.Append(source[current]);

            if (maxIterations == 3 && source[current] == '\\')
            {
                maxIterations = 4;
            }
            else if (maxIterations == 4 && current == 2 && source[current] == 'x')
            {
                maxIterations = 8;
            }
            
            current++;
        }

        var characterValue = sb.ToString();
        var tokenPosition = TokenPosition.At(_line, _column);

        _index += characterValue.Length;
        _column += characterValue.Length;

        return new Token(TokenType.Character, characterValue, tokenPosition);
    }

    private Token ReadIdentifier()
    {
        var source = _source.AsSpan(_index);
        var sb = new StringBuilder();

        foreach (var character in source)
        {
            if (!char.IsLetterOrDigit(character) && character != '_')
            {
                break;
            }

            sb.Append(character);
        }

        var identifierValue = sb.ToString();
        var tokenPosition = TokenPosition.At(_line, _column);

        _index += identifierValue.Length;
        _column += identifierValue.Length;
        

        return new Token(
            Keywords.Tokens.GetValueOrDefault(identifierValue, TokenType.Identifier),
            identifierValue,
            tokenPosition);
    }

    private Token ReadNumber()
    {
        var source = _source[_index..];
        
        foreach (var (token, patterns) in NumberPatterns)
        {
            foreach (var pattern in patterns)
            {
                var match = pattern.Match(source);
                if (!match.Success)
                {
                    continue;
                }

                var numberValue = match.Value;
                var tokenPosition = TokenPosition.At(_line, _column);

                _index += numberValue.Length;
                _column += numberValue.Length;

                return new Token(token, numberValue, tokenPosition);
            }
        }

        return Token.None;
    }

    private Token SkipWhiteSpace()
    {
        var current = _source[_index];

        while (char.IsWhiteSpace(current))
        {
            StepForward(current == '\n');

            if (_index >= _source.Length)
            {
                return new Token(
                    TokenType.Eof,
                    string.Empty,
                    TokenPosition.At(_line, _column));
            }

            current = _source[_index];
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

    [GeneratedRegex("^0(?i)b[0-1]+(ul|u|l)?")]
    private static partial Regex BinaryNumberPattern();

    [GeneratedRegex("^0(?i)x[0-9a-f]+(ul|u|l)?")]
    private static partial Regex HexadecimalNumberPattern();

    [GeneratedRegex("^[0-9]+(?i)(ul|u|l)?")]
    private static partial Regex IntegerNumberPattern();

    [GeneratedRegex("^[0-9]+\\.[0-9]+(?i)(e[+|-][0-9]+)?(f|d|m)?")]
    private static partial Regex RealNumberPattern();
}