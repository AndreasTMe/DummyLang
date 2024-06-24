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
    private readonly Tokenizer _tokenizer = new();
    private readonly SyntaxTree _syntaxTree = new();
    private readonly SyntaxDiagnosticsHandler _diagnosticsHandler = new();

    private string _sourcePath = string.Empty;
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

    public SyntaxParser Feed(string sourcePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sourcePath);

        _sourcePath = sourcePath;
        _tokenizer.Use(sourcePath);

        var tokens = new List<Token>();
        var token = _tokenizer.ReadNext();

        while (!token.IsEof())
        {
            tokens.Add(token);
            token = _tokenizer.ReadNext();
        }

        tokens.Add(token);
        _tokens = tokens;

        _current = Token.None;
        _index = 0;

        return this;
    }

    public SyntaxTree GenerateSyntax()
    {
        _syntaxTree.Clear();

        while (_index < _tokens.Count)
        {
            var lastIndex = _index;

            if (Current.IsEof() || Current.IsInvalid())
            {
                break;
            }

            var node = ParseExpression();
            _syntaxTree.Insert(node);

            if (lastIndex == _index)
            {
                MoveToNext();
            }
        }

        _syntaxTree.CaptureDiagnostics(_diagnosticsHandler.Diagnostics);

        _tokens.Clear();
        _diagnosticsHandler.Clear();

        return _syntaxTree;
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
            {
                var unaryOperator = GetAndMoveToNext();
                Expression? expression = null;

                if (Current.Type == TokenType.Identifier)
                {
                    expression = new IdentifierExpression(GetAndMoveToNext());
                }

                if ((unaryOperator.Type == TokenType.Plus || unaryOperator.Type == TokenType.Minus) &&
                    (Current.Type == TokenType.Integer || Current.Type == TokenType.Real))
                {
                    expression = ParseNumberExpression(Current.Type == TokenType.Integer);
                }

                if (expression is null)
                {
                    CaptureDiagnosticsInfo(unaryOperator, UnaryExpression.AppliedToInvalidToken);
                }

                return new UnaryExpression(
                    unaryOperator,
                    expression ?? new InvalidExpression(GetAndMoveToNext()));
            }
            case TokenType.LeftParen:
            {
                var leftParen = GetAndMoveToNext();
                var expression = ParseExpression();

                if (Current.Type == TokenType.RightParen)
                {
                    return new ParenthesisedExpression(leftParen, expression, GetAndMoveToNext());
                }

                CaptureDiagnosticsInfo(Current, ParenthesisedExpression.ClosingParenthesisExpected);

                return new InvalidExpression(
                    Token.ExpectedAt(Current.Position),
                    new ParenthesisedExpression(leftParen, expression));
            }
            case TokenType.RightParen:
            {
                CaptureDiagnosticsInfo(Current, ParenthesisedExpression.OpeningParenthesisExpected);
                return new InvalidExpression(GetAndMoveToNext());
            }
            case TokenType.Identifier:
            {
                var identifierExpression = new IdentifierExpression(GetAndMoveToNext());

                if (Current.Type != TokenType.PlusPlus && Current.Type != TokenType.MinusMinus)
                {
                    return identifierExpression;
                }

                var expression = new PrimaryExpression(identifierExpression, GetAndMoveToNext());

                while (Current.Type is TokenType.PlusPlus or TokenType.MinusMinus)
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
                var characterToken = GetAndMoveToNext();
                var characterValue = characterToken.Value;
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
            case TokenType.String:
            {
                var stringToken = GetAndMoveToNext();
                var stringValue = stringToken.Value;
                var stringLiteralExpression = new StringLiteralExpression(stringToken);

                var diagnosticsMessage = string.Empty;

                if (!stringValue.IsValidLength() || !stringValue.IsSurroundedByDoubleQuotes())
                    diagnosticsMessage = StringLiteralExpression.ShouldBeSurroundedByDoubleQuote;
                else if (stringValue.EscapesLastDoubleQuote())
                    diagnosticsMessage = StringLiteralExpression.ShouldNotEscapeLastDoubleQuote;
                else if (stringValue.HasInvalidEscapedCharacters())
                    diagnosticsMessage = StringLiteralExpression.InvalidEscapedCharacter;

                if (string.IsNullOrWhiteSpace(diagnosticsMessage))
                {
                    return stringLiteralExpression;
                }

                CaptureDiagnosticsInfo(stringToken, diagnosticsMessage);
                return new InvalidExpression(stringToken, stringLiteralExpression);
            }
            default:
            {
                CaptureDiagnosticsInfo(Current, InvalidExpression.UnsupportedOrUnimplemented);
                return new InvalidExpression(GetAndMoveToNext());
            }
        }
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

    private void CaptureDiagnosticsInfo(Token token, string message)
    {
        if (token.Type is TokenType.None or TokenType.Eof)
        {
            message = "Syntax Error: Invalid token. " + message.TrimStart();
        }
        else
        {
            message = $"Syntax Error: Invalid token ({token.Value}). " + message.TrimStart();
        }

        // TODO: Update when actual file path will be used
        const string sourcePath = "C:/ProjectPath/ProjectFile.dum";

        _diagnosticsHandler.Capture(message.TrimEnd(), sourcePath, token.Position.Line, token.Position.Column);
    }
}