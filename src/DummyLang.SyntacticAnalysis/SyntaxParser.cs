using DummyLang.LexicalAnalysis;
using DummyLang.LexicalAnalysis.Extensions;
using DummyLang.SyntacticAnalysis.Diagnostics;
using DummyLang.SyntacticAnalysis.Expressions;
using DummyLang.SyntacticAnalysis.Extensions;
using DummyLang.SyntacticAnalysis.Utilities;
using System;
using System.Collections.Generic;

namespace DummyLang.SyntacticAnalysis;

public class SyntaxParser
{
    private readonly Tokenizer                _tokenizer          = new();
    private readonly SyntaxTree               _syntaxTree         = new();
    private readonly SyntaxDiagnosticsHandler _diagnosticsHandler = new();

    private string  _sourcePath = string.Empty;
    private Token[] _tokens     = [];
    private Token   _current    = Token.None;
    private int     _index;

    private Token Current
    {
        get
        {
            if (_index < _tokens.Length)
                _current = _tokens[_index];

            return _current;
        }
    }

    public SyntaxParser Feed(string sourcePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sourcePath);

        _sourcePath = sourcePath;
        _tokenizer.Use(sourcePath);

        var tokens = new List<Token>();
        var token  = _tokenizer.ReadNext();

        while (!token.IsEndOfFile())
        {
            tokens.Add(token);
            token = _tokenizer.ReadNext();
        }

        tokens.Add(token);
        _tokens = tokens.ToArray();

        _current = Token.None;
        _index   = 0;

        return this;
    }

    public SyntaxTree GenerateSyntax()
    {
        _syntaxTree.Clear();

        while (_index < _tokens.Length)
        {
            var lastIndex = _index;

            if (Current.IsEndOfFile() || Current.IsInvalid())
                break;

            var node = ParseExpression();
            _syntaxTree.Insert(node);

            if (lastIndex == _index)
                MoveToNext();
        }

        _syntaxTree.CaptureDiagnostics(_diagnosticsHandler.Diagnostics);

        _current = Token.None;
        _index   = 0;

        return _syntaxTree;
    }

    private void MoveToNext() => _index++;

    private Token GetAndMoveToNext() => _index < _tokens.Length ? _tokens[_index++] : Token.None;

    private Expression ParseExpression(OperatorPrecedence previousPrecedence = 0)
    {
        var left = ParseExpressionBasedOnCurrentToken();

        while (Current.IsBinaryOperator())
        {
            var currentPrecedence = Current.GetOperatorPrecedence();
            if (currentPrecedence == OperatorPrecedence.None || currentPrecedence <= previousPrecedence)
                break;

            var op    = GetAndMoveToNext();
            var right = ParseExpression(currentPrecedence);

            left = new BinaryExpression(left, op, right);
        }

        return left;
    }

    private Expression ParseExpressionBasedOnCurrentToken()
    {
        // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
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
                return ParseUnaryExpression();
            case TokenType.LeftParenthesis:
                return ParseParenthesisedExpression();
            case TokenType.RightParenthesis:
                CaptureDiagnosticsInfo(Current, ParenthesisedExpression.OpeningParenthesisExpected);
                return new InvalidExpression(GetAndMoveToNext());
            case TokenType.Identifier:
            {
                var identifierToken = GetAndMoveToNext();

                if (Current.Type == TokenType.LeftParenthesis)
                    return ParseFunctionCallExpression(identifierToken);

                // TODO: Check for array access
                // TODO: Check for pointer access
                // TODO: Check for member access
                // TODO: Check for range operator

                var identifierExpression = new IdentifierExpression(identifierToken);

                if (Current.Type != TokenType.PlusPlus && Current.Type != TokenType.MinusMinus)
                    return identifierExpression;

                var expression = new PrimaryExpression(identifierExpression, GetAndMoveToNext());

                while (Current.Type is TokenType.PlusPlus or TokenType.MinusMinus)
                    expression = new PrimaryExpression(expression, GetAndMoveToNext());

                return expression;
            }
            case TokenType.Integer:
            case TokenType.Real:
                return ParseNumberExpression(Current.Type == TokenType.Integer);
            case TokenType.Character:
                return ParseCharacterExpression();
            case TokenType.String:
                return ParseStringExpression();
            default:
                CaptureDiagnosticsInfo(Current, InvalidExpression.UnsupportedOrUnimplemented);
                return new InvalidExpression(GetAndMoveToNext());
        }
    }

    private UnaryExpression ParseUnaryExpression()
    {
        var         unaryOperator = GetAndMoveToNext();
        Expression? expression    = null;

        if (Current.Type == TokenType.Identifier)
            expression = new IdentifierExpression(GetAndMoveToNext());

        if (unaryOperator.IsAdditiveOperator() && Current.IsNumber())
            expression = ParseNumberExpression(Current.Type == TokenType.Integer);

        if (expression is null)
            CaptureDiagnosticsInfo(unaryOperator, UnaryExpression.AppliedToInvalidToken);

        return new UnaryExpression(
            unaryOperator,
            expression ?? new InvalidExpression(GetAndMoveToNext()));
    }

    private Expression ParseParenthesisedExpression()
    {
        var leftParen  = GetAndMoveToNext();
        var expression = ParseExpression();

        if (Current.Type == TokenType.RightParenthesis)
            return new ParenthesisedExpression(leftParen, expression, GetAndMoveToNext());

        CaptureDiagnosticsInfo(Current, ParenthesisedExpression.ClosingParenthesisExpected);

        return new InvalidExpression(
            Token.ExpectedAt(Current.Position),
            new ParenthesisedExpression(leftParen, expression));
    }

    // TODO: Implement function call parsing
    private Expression ParseFunctionCallExpression(in Token identifier)
    {
        // TODO: This is the balanced brackets leetcode, move to utility
        var currentIndex = _index + 1;
        var stack        = new Stack<Token>();
        stack.Push(Current);

        var reachedInvalidToken = false;
        while (currentIndex < _tokens.Length)
        {
            var currentToken = _tokens[currentIndex];

            // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
            switch (currentToken.Type)
            {
                case TokenType.LeftParenthesis:
                case TokenType.LeftBrace:
                case TokenType.LeftBracket:
                {
                    stack.Push(currentToken);
                    currentIndex++;
                    continue;
                }
                case TokenType.RightParenthesis:
                case TokenType.RightBrace:
                case TokenType.RightBracket:
                {
                    if (stack.TryPeek(out var top)
                        && (top.IsParenthesisMatch(currentToken)
                            || top.IsBracketMatch(currentToken)
                            || top.IsBraceMatch(currentToken)))
                        stack.Pop();
                    else
                        reachedInvalidToken = true;

                    break;
                }
            }

            if (reachedInvalidToken)
                break;

            currentIndex++;

            if (stack.Count == 0)
                break;
        }

        return reachedInvalidToken
            ? new InvalidExpression(Token.None)
            : new PrimaryExpression(new FunctionCallExpression(identifier, Token.None, Token.None, [], []));
    }

    private NumberLiteralExpression ParseNumberExpression(bool isInteger)
    {
        var number = Current.Value;

        if (isInteger)
        {
            var integerType = NumberType.Integer;

            if (number.StartsWith("0x"))
                integerType = NumberType.Hexadecimal;
            else if (number.StartsWith("0b"))
                integerType = NumberType.Binary;
            else if (number.EndsWith("ul", StringComparison.OrdinalIgnoreCase))
                integerType = NumberType.UnsignedLong;
            else if (number.EndsWith("u", StringComparison.OrdinalIgnoreCase))
                integerType = NumberType.UnsignedInteger;
            else if (number.EndsWith("l", StringComparison.OrdinalIgnoreCase))
                integerType = NumberType.Long;

            return new NumberLiteralExpression(GetAndMoveToNext(), integerType);
        }

        var realType = NumberType.Double;

        if (number.EndsWith("f", StringComparison.OrdinalIgnoreCase))
            realType = NumberType.Float;
        else if (number.EndsWith("d", StringComparison.OrdinalIgnoreCase))
            realType = NumberType.Double;
        else if (number.EndsWith("m", StringComparison.OrdinalIgnoreCase))
            realType = NumberType.Decimal;
        else if (number.Contains('e', StringComparison.OrdinalIgnoreCase))
            realType = NumberType.WithExponent;

        return new NumberLiteralExpression(GetAndMoveToNext(), realType);
    }

    private Expression ParseCharacterExpression()
    {
        var characterToken             = GetAndMoveToNext();
        var characterValue             = characterToken.Value;
        var characterLiteralExpression = new CharacterLiteralExpression(characterToken);

        var diagnosticMessage = string.Empty;

        if (!characterValue.HasValidCharacterLength())
            diagnosticMessage = CharacterLiteralExpression.ShouldBeOfCertainLength;
        else if (!characterValue.IsSurroundedBySingleQuotes())
            diagnosticMessage = CharacterLiteralExpression.ShouldStartEndWithSingleQuote;
        else if (characterValue.IsUnescapedSingleQuoteOrBackslash())
            diagnosticMessage = CharacterLiteralExpression.ShouldBeEscaped;
        else if (characterValue[1] == '\\')
        {
            if (characterValue[2] == 'x')
            {
                // ReSharper disable once InvertIf
                if (!characterValue.IsValidHexadecimalCharacter())
                    diagnosticMessage = CharacterLiteralExpression.InvalidEscapedCharacter;
            }
            else if (!characterValue.IsValidEscapedCharacter())
                diagnosticMessage = CharacterLiteralExpression.InvalidHexadecimalCharacter;
        }

        switch (string.IsNullOrWhiteSpace(diagnosticMessage))
        {
            case false:
                CaptureDiagnosticsInfo(characterToken, CharacterLiteralExpression.ShouldBeOfCertainLength);
                return new InvalidExpression(characterToken, characterLiteralExpression);
            default:
                return characterLiteralExpression;
        }
    }

    private Expression ParseStringExpression()
    {
        var stringToken             = GetAndMoveToNext();
        var stringValue             = stringToken.Value;
        var stringLiteralExpression = new StringLiteralExpression(stringToken);

        var diagnosticsMessage = string.Empty;

        if (!stringValue.IsValidLength() || !stringValue.IsSurroundedByDoubleQuotes())
            diagnosticsMessage = StringLiteralExpression.ShouldBeSurroundedByDoubleQuote;
        else if (stringValue.EscapesLastDoubleQuote())
            diagnosticsMessage = StringLiteralExpression.ShouldNotEscapeLastDoubleQuote;
        else if (stringValue.HasInvalidEscapedCharacters())
            diagnosticsMessage = StringLiteralExpression.InvalidEscapedCharacter;

        if (string.IsNullOrWhiteSpace(diagnosticsMessage))
            return stringLiteralExpression;

        CaptureDiagnosticsInfo(stringToken, diagnosticsMessage);
        return new InvalidExpression(stringToken, stringLiteralExpression);
    }

    private void CaptureDiagnosticsInfo(Token token, string message)
    {
        if (token.Type is TokenType.None or TokenType.Eof)
            message = "Syntax Error: Invalid token. " + message.TrimStart();
        else
            message = $"Syntax Error: Invalid token ({token.Value}). " + message.TrimStart();

        // TODO: Update when actual file path will be used
        const string sourcePath = "C:/ProjectPath/ProjectFile.dum";

        _diagnosticsHandler.Capture(message.TrimEnd(), sourcePath, token.Position.Line, token.Position.Column);
    }
}