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
            CaptureDiagnosticsInfo(expression.LeftBracket, "Indexer expected.");
        else
            expression.Index.Accept(this);
    }

    public void Visit(MemberAccessExpression expression)
    {
        if (expression.Access.Type != TokenType.PointerAccess && expression.Access.Type != TokenType.Dot)
            LanguageSyntax.Throw("Invalid access token added. How did this happen?");

        if (expression.Member is null)
            CaptureDiagnosticsInfo(expression.Access, "Identifier expected.");

        expression.Identifier.Accept(this);
        expression.Member?.Accept(this);
    }

    public void Visit(NumberLiteralExpression expression)
    {
        if (expression.Type == NumberType.None)
            CaptureDiagnosticsInfo(expression.NumberToken, "Invalid number type.");
    }

    public void Visit(ParenthesisedExpression expression)
    {
        if (expression.LeftParen.Type != TokenType.LeftParenthesis)
            LanguageSyntax.Throw("Invalid left parenthesis token added. How did this happen?");

        if (expression.Expression is null)
            CaptureDiagnosticsInfo(expression.LeftParen, "Expression expected.");

        if (expression.RightParen.Type != TokenType.RightParenthesis)
            CaptureDiagnosticsInfo(expression.LeftParen, "Right parenthesis expected.");

        expression.Expression?.Accept(this);
    }

    public void Visit(PrimaryExpression expression)
    {
        if (expression.Expression is null)
            LanguageSyntax.Throw("Empty primary expression. How did this happen?");

        if (!expression.Token.IsNone()
            && expression.Token.Type != TokenType.PlusPlus
            && expression.Token.Type != TokenType.MinusMinus)
            LanguageSyntax.Throw("Invalid primary expression token added. How did this happen?");

        expression.Expression.Accept(this);
    }

    public void Visit(RangeExpression expression)
    {
        if (expression.Operator.Type != TokenType.DoubleDot)
            LanguageSyntax.Throw("Invalid range operator token added. How did this happen?");
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

    public void Visit(TypeArgumentExpression expression)
    {
        if (expression.Argument is null && expression.Comma.IsNone())
            LanguageSyntax.Throw("Empty argument added. How did this happen?");

        if (!expression.Comma.IsNone() && expression.Comma.Type != TokenType.Comma)
            LanguageSyntax.Throw("Invalid comma token added. How did this happen?");
    }

    public void Visit(TypeBinaryExpression expression)
    {
        if (!expression.Operator.IsBitwiseOperator())
            LanguageSyntax.Throw("Bitwise operator expected. How did this happen?");

        if (expression.Left is null)
            LanguageSyntax.Throw("Left side of a type binary expression is null. How did this happen?");

        if (expression.Right is null)
            CaptureDiagnosticsInfo(expression.Operator, "Expression expected after bitwise operator.");

        expression.Left.Accept(this);
        expression.Right?.Accept(this);
    }

    public void Visit(TypeGenericExpression expression)
    {
        if (expression.Identifier.Type != TokenType.Identifier)
            LanguageSyntax.Throw("Invalid identifier token added. How did this happen?");

        if (expression.LessThan.Type != TokenType.LessThan)
            LanguageSyntax.Throw("Invalid left generic bracket token added. How did this happen?");

        if (expression.GreaterThan.Type != TokenType.GreaterThan)
        {
            CaptureDiagnosticsInfo(expression.LessThan, "Right generic bracket token expected.");
        }
        else if (expression.TypeArguments is { Count: > 0 })
        {
            if (!expression.TypeArguments[^1].Comma.IsNone())
                CaptureDiagnosticsInfo(expression.GreaterThan, "Last argument should not be followed by comma.");

            for (var i = 0; i < expression.TypeArguments.Count; i++)
            {
                var argument = expression.TypeArguments[i];

                if (argument.Argument is null)
                    CaptureDiagnosticsInfo(Token.None, "Argument expected.");

                if (i != expression.TypeArguments.Count - 1 && argument.Comma.Type != TokenType.Comma)
                    CaptureDiagnosticsInfo(Token.None, "Comma expected.");

                argument.Accept(this);
            }
        }
    }

    public void Visit(TypeIdentifierExpression expression)
    {
        if (expression.Token.Type != TokenType.Identifier)
            LanguageSyntax.Throw("Invalid identifier token added. How did this happen?");
    }

    public void Visit(UnaryExpression expression)
    {
        var type = expression.Token.Type;
        if (type != TokenType.Plus
            && type != TokenType.Minus
            && type != TokenType.PlusPlus
            && type != TokenType.MinusMinus
            && type != TokenType.Bang
            && type != TokenType.Tilde
            && type != TokenType.Star
            && type != TokenType.Ampersand)
            LanguageSyntax.Throw("Invalid unary operator token added. How did this happen?");

        if (expression.Expression is null)
            CaptureDiagnosticsInfo(expression.Token, "Expression expected after unary operator.");

        // TODO: A lot of checks here for the inner expression
    }

    public void Visit(UnexpectedTokenExpression expression) =>
        CaptureDiagnosticsInfo(expression.Token, "Unexpected token.");

    public void Visit(BreakStatement statement)
    {
        if (statement.BreakKeyword.Type != TokenType.Break)
            LanguageSyntax.Throw("Invalid 'break' token added. How did this happen?");

        if (!statement.Label.IsNone() && statement.Label.Type != TokenType.Identifier)
            CaptureDiagnosticsInfo(statement.Label, "Invalid token after 'break' keyword.");

        if (statement.Terminator.Type != TokenType.Semicolon)
            CaptureDiagnosticsInfo(statement.Terminator, "Semicolon expected at the end of a 'break' statement.");
    }

    public void Visit(CompoundStatement statement)
    {
        if (statement.LeftBrace.Type != TokenType.LeftBrace)
            LanguageSyntax.Throw("Invalid left brace token added. How did this happen?");

        if (statement.RightBrace.Type != TokenType.RightBrace)
            CaptureDiagnosticsInfo(statement.RightBrace, "Right brace expected at the end of a block statement.");

        if (statement.Statements is null)
            return;

        foreach (var innerStatement in statement.Statements)
            innerStatement.Accept(this);
    }

    public void Visit(ContinueStatement statement)
    {
        if (statement.ContinueKeyword.Type != TokenType.Continue)
            LanguageSyntax.Throw("Invalid 'continue' token added. How did this happen?");

        if (!statement.Label.IsNone() && statement.Label.Type != TokenType.Identifier)
            CaptureDiagnosticsInfo(statement.Label, "Invalid token after 'continue' keyword.");

        if (statement.Terminator.Type != TokenType.Semicolon)
            CaptureDiagnosticsInfo(statement.Terminator, "Semicolon expected at the end of a 'continue' statement.");
    }

    public void Visit(ExpressionStatement statement)
    {
        if (statement.Expression is null)
            LanguageSyntax.Throw($"This should have been a {nameof(NoOpStatement)}. How did this happen?");

        if (statement.Terminator.Type != TokenType.Semicolon)
            CaptureDiagnosticsInfo(statement.Terminator, "Semicolon expected at the end of an expression statement.");

        statement.Expression?.Accept(this);
    }

    public void Visit(IfElseStatement statement)
    {
    }

    public void Visit(NoOpStatement statement)
    {
        if (statement.Terminator.Type != TokenType.Semicolon)
            LanguageSyntax.Throw("Invalid semicolon token added. How did this happen?");
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