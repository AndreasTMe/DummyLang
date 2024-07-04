using DummyLang.Diagnostics;
using DummyLang.LexicalAnalysis;
using DummyLang.LexicalAnalysis.Extensions;
using DummyLang.SyntacticAnalysis.Abstractions;
using DummyLang.SyntacticAnalysis.Expressions;
using DummyLang.SyntacticAnalysis.Extensions;
using DummyLang.SyntacticAnalysis.Statements;
using DummyLang.SyntacticAnalysis.Utilities;
using System.Collections.Generic;

namespace DummyLang.SyntacticAnalysis.Visitors;

// TODO: Handle correct positioning for diagnostics. Several captures have placeholders.
internal sealed class SyntaxNodeValidationVisitor : ISyntaxNodeVisitor
{
    private readonly Dictionary<string, List<DiagnosticInfo>> _diagnostics = new();

    public void Visit(ArgumentExpression expression)
    {
        if (expression.Argument is null && expression.Comma.IsNone())
            LanguageSyntax.Throw("Empty argument added. How did this happen?");

        if (!expression.Comma.IsNone() && expression.Comma.Type != TokenType.Comma)
            LanguageSyntax.Throw("Invalid comma token added. How did this happen?");
    }

    public void Visit(BinaryExpression expression)
    {
        if (!expression.Operator.IsBinaryOperator())
            LanguageSyntax.Throw("Binary operator expected. How did this happen?");

        if (expression.Left is null)
            LanguageSyntax.Throw("Left side of a binary expression is null. How did this happen?");

        if (expression.Right is null)
            CaptureDiagnosticsInfo(expression.Operator, "Expression expected after binary operator.");

        expression.Left.Accept(this);
        expression.Right?.Accept(this);
    }

    public void Visit(CharacterLiteralExpression expression)
    {
        var characterValue = expression.CharacterToken.Value;
        var message        = string.Empty;

        if (!characterValue.HasValidCharacterLength())
            message = "Character literal token with invalid length provided.";
        else if (!characterValue.IsSurroundedBySingleQuotes())
            message = "A character literal must start with a single quote (') and end with a single quote (').";
        else if (characterValue.IsUnescapedSingleQuoteOrBackslash())
            message = "The character literal provided must be escaped with a backslash (\\).";
        else if (characterValue[1] == '\\')
        {
            if (characterValue[2] == 'x')
            {
                if (!characterValue.IsValidHexadecimalCharacter())
                    message = "The token provided is not a valid escaped character literal.";
            }
            else if (!characterValue.IsValidEscapedCharacter())
                message = "The token provided is not a valid hexadecimal character literal.";
        }

        if (!string.IsNullOrWhiteSpace(message))
            CaptureDiagnosticsInfo(expression.CharacterToken, message);
    }

    public void Visit(FunctionCallExpression expression)
    {
        if (expression.Identifier.Type != TokenType.Identifier)
            LanguageSyntax.Throw("Invalid identifier token added. How did this happen?");

        if (expression.LeftParenthesis.Type != TokenType.LeftParenthesis)
            LanguageSyntax.Throw("Invalid left parenthesis token added. How did this happen?");

        if (expression.RightParenthesis.Type != TokenType.RightParenthesis)
        {
            CaptureDiagnosticsInfo(expression.LeftParenthesis, "Right parenthesis token expected.");
        }
        else if (expression.Arguments is { Count: > 0 })
        {
            if (!expression.Arguments[^1].Comma.IsNone())
                CaptureDiagnosticsInfo(expression.RightParenthesis, "Last argument should not be followed by comma.");

            for (var i = 0; i < expression.Arguments.Count; i++)
            {
                var argument = expression.Arguments[i];

                if (argument.Argument is null)
                    CaptureDiagnosticsInfo(Token.None, "Argument expected.");

                if (i != expression.Arguments.Count - 1 && argument.Comma.Type != TokenType.Comma)
                    CaptureDiagnosticsInfo(Token.None, "Comma expected.");

                argument.Accept(this);
            }
        }
    }

    public void Visit(IdentifierExpression expression)
    {
        if (expression.Token.Type != TokenType.Identifier)
            LanguageSyntax.Throw("Invalid identifier token added. How did this happen?");
    }

    public void Visit(IndexerExpression expression)
    {
        if (expression.Identifier.Type != TokenType.Identifier)
            LanguageSyntax.Throw("Invalid identifier token added. How did this happen?");

        if (expression.LeftBracket.Type != TokenType.LeftBracket)
            LanguageSyntax.Throw("Invalid left bracket token added. How did this happen?");

        if (expression.RightBracket.Type != TokenType.RightBracket)
            CaptureDiagnosticsInfo(expression.LeftBracket, "Right bracket token expected.");
        else if (expression.Index is null)
            CaptureDiagnosticsInfo(expression.LeftBracket, "Index expression expected.");
        else
            expression.Index.Accept(this);
    }

    public void Visit(InvalidExpression expression)
    {
        // TODO: Remove
    }

    public void Visit(MemberAccessExpression expression)
    {
    }

    public void Visit(NumberLiteralExpression expression)
    {
        if (expression.Type == NumberType.None)
            CaptureDiagnosticsInfo(expression.NumberToken, "Invalid number type.");
    }

    public void Visit(ParenthesisedExpression expression)
    {
    }

    public void Visit(PrimaryExpression expression)
    {
    }

    public void Visit(RangeExpression expression)
    {
    }

    public void Visit(StringLiteralExpression expression)
    {
        var stringValue = expression.StringToken.Value;
        var message     = string.Empty;

        if (!stringValue.IsValidLength() || !stringValue.IsSurroundedByDoubleQuotes())
            message = "A string literal must start with a double quote (\") and end with a double quote (\").";
        else if (stringValue.EscapesLastDoubleQuote())
            message = "The last double quote of a string literal should not be escaped.";
        else if (stringValue.HasInvalidEscapedCharacters())
            message = "The string literal provided contains an invalid escaped character literal.";

        if (!string.IsNullOrWhiteSpace(message))
            CaptureDiagnosticsInfo(expression.StringToken, message);
    }

    public void Visit(TypeBinaryExpression expression)
    {
    }

    public void Visit(TypeGenericExpression expression)
    {
    }

    public void Visit(TypeIdentifierExpression expression)
    {
    }

    public void Visit(UnaryExpression expression)
    {
    }

    public void Visit(BreakStatement statement)
    {
    }

    public void Visit(CompoundStatement statement)
    {
    }

    public void Visit(ContinueStatement statement)
    {
    }

    public void Visit(ExpressionStatement statement)
    {
    }

    public void Visit(IfElseStatement statement)
    {
    }

    public void Visit(NoOpStatement statement)
    {
    }

    public void Visit(ReturnStatement statement)
    {
    }

    public void Visit(VariableDeclarationStatement statement)
    {
    }

    public void Visit(WhileStatement statement)
    {
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

        if (!_diagnostics.ContainsKey(sourcePath))
            _diagnostics[sourcePath] = [];

        _diagnostics[sourcePath]
            .Add(new DiagnosticInfo(message.TrimEnd(), sourcePath, token.Position.Line, token.Position.Column));
    }
}