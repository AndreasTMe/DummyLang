using DummyLang.LexicalAnalysis.Extensions;
using DummyLang.LexicalAnalysis.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace DummyLang.LexicalAnalysis;

public sealed class Tokenizer
{
    private static readonly Dictionary<TokenType, Regex[]> NumberPatterns = new()
    {
        { TokenType.Real, [Patterns.RealNumber()] },
        { TokenType.Integer, [Patterns.BinaryNumber(), Patterns.HexadecimalNumber(), Patterns.IntegerNumber()] }
    };

    private string _source = string.Empty;
    private int    _index;
    private int    _line;
    private int    _column;

    public void Use(in string? newSource)
    {
        _source = newSource ?? string.Empty;
        _index  = 0;
        _line   = 1;
        _column = 1;
    }

    public Token ReadNext()
    {
        if (_index >= _source.Length)
        {
            return new Token(TokenType.Eof, string.Empty, TokenPosition.At(_line, _column));
        }

        var current = _source[_index];

        return current switch
        {
            ',' => GenerateToken(TokenType.Comma),
            '.' => GenerateTokenBasedOnNext(TokenType.Dot, TokenType.DoubleDot),
            ';' => GenerateToken(TokenType.Semicolon),
            ':' => GenerateToken(TokenType.Colon),
            '=' => GenerateTokenBasedOnNext(TokenType.Assign, TokenType.Equal),
            '!' => GenerateTokenBasedOnNext(TokenType.Bang, TokenType.NotEqual),
            '+' => GenerateTokenBasedOnNext(TokenType.Plus, TokenType.PlusPlus, TokenType.PlusAssign),
            '-' => GenerateTokenBasedOnNext(
                TokenType.Minus,
                TokenType.MinusMinus,
                TokenType.MinusAssign,
                TokenType.PointerAccess),
            '*' => GenerateTokenBasedOnNext(TokenType.Star, TokenType.StarAssign),
            '/' => GenerateTokenBasedOnNext(TokenType.Slash, TokenType.SlashAssign),
            '%' => GenerateTokenBasedOnNext(TokenType.Percent, TokenType.PercentAssign),
            '&' => GenerateTokenBasedOnNext(TokenType.Ampersand, TokenType.DoubleAmpersand, TokenType.AmpersandAssign),
            '|' => GenerateTokenBasedOnNext(TokenType.Pipe, TokenType.DoublePipe, TokenType.PipeAssign),
            '~' => GenerateToken(TokenType.Tilde),
            '^' => GenerateTokenBasedOnNext(TokenType.Caret, TokenType.CaretAssign),
            '<' => GenerateTokenBasedOnNext(
                TokenType.LessThan,
                TokenType.LessThanOrEqual,
                TokenType.LeftBitShift,
                TokenType.LeftBitShiftAssign),
            '>' => GenerateTokenBasedOnNext(
                TokenType.GreaterThan,
                TokenType.GreaterThanOrEqual,
                TokenType.RightBitShift,
                TokenType.RightBitShiftAssign),
            '?' => GenerateTokenBasedOnNext(
                TokenType.QuestionMark,
                TokenType.DoubleQuestionMark,
                TokenType.DoubleQuestionMarkAssign),
            '('  => GenerateToken(TokenType.LeftParenthesis),
            ')'  => GenerateToken(TokenType.RightParenthesis),
            '{'  => GenerateToken(TokenType.LeftBrace),
            '}'  => GenerateToken(TokenType.RightBrace),
            '['  => GenerateToken(TokenType.LeftBracket),
            ']'  => GenerateToken(TokenType.RightBracket),
            '\'' => ReadCharacter(),
            '\"' => ReadString(),
            _ => char.IsLetter(current) || current == '_'
                ? ReadIdentifier()
                : char.IsDigit(current)
                    ? ReadNumber()
                    : SkipWhiteSpace()
        };
    }

    private Token GenerateToken(TokenType type)
    {
        var stringToken       = type.GetStringToken();
        var stringTokenLength = stringToken.Length;
        if (stringTokenLength < 1)
        {
            throw new UnreachableException("This should never happen.");
        }

        var tokenPosition = TokenPosition.At(_line, _column);

        _index  += stringTokenLength;
        _column += stringTokenLength;

        return new Token(type, stringToken, tokenPosition);
    }

    private Token GenerateTokenBasedOnNext(TokenType type, params TokenType[] possibleTypes)
    {
        var sourceFromCurrentPosition = _source.AsSpan(_index);
        var lastTokenMatch            = string.Empty;
        var lastTokenTypeMatch        = TokenType.None;

        foreach (var typeToCheck in possibleTypes)
        {
            var stringToken = typeToCheck.GetStringToken();
            if (!sourceFromCurrentPosition.StartsWith(stringToken))
                continue;

            if (stringToken.Length <= lastTokenMatch.Length)
                continue;

            lastTokenMatch     = stringToken;
            lastTokenTypeMatch = typeToCheck;
        }

        return GenerateToken(lastTokenTypeMatch == TokenType.None ? type : lastTokenTypeMatch);
    }

    private Token ReadString()
    {
        var source = _source.AsSpan(_index);
        var sb     = new StringBuilder();
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

        _index  += stringValue.Length;
        _column += stringValue.Length;

        return new Token(TokenType.String, stringValue, tokenPosition);
    }

    private Token ReadCharacter()
    {
        var source        = _source.AsSpan(_index);
        var sb            = new StringBuilder();
        var current       = 0;
        var maxIterations = 3;

        while (current < maxIterations && current < source.Length && source[current] != '\n')
        {
            if (!(current == maxIterations - 1 && source[current] != '\''))
            {
                sb.Append(source[current]);
            }

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
        var tokenPosition  = TokenPosition.At(_line, _column);

        _index  += characterValue.Length;
        _column += characterValue.Length;

        return new Token(TokenType.Character, characterValue, tokenPosition);
    }

    private Token ReadIdentifier()
    {
        var source = _source.AsSpan(_index);
        var sb     = new StringBuilder();

        foreach (var character in source)
        {
            if (!char.IsLetterOrDigit(character) && character != '_')
            {
                break;
            }

            sb.Append(character);
        }

        var identifierValue = sb.ToString();
        var tokenPosition   = TokenPosition.At(_line, _column);

        _index  += identifierValue.Length;
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

                var numberValue   = match.Value;
                var tokenPosition = TokenPosition.At(_line, _column);

                _index  += numberValue.Length;
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
}