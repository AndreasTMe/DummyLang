using DummyLang.Diagnostics;
using DummyLang.LexicalAnalysis;
using DummyLang.LexicalAnalysis.Extensions;
using DummyLang.SyntacticAnalysis.Abstractions;
using DummyLang.SyntacticAnalysis.Expressions;
using DummyLang.SyntacticAnalysis.Extensions;
using DummyLang.SyntacticAnalysis.Statements;
using DummyLang.SyntacticAnalysis.Utilities;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;

namespace DummyLang.SyntacticAnalysis.Visitors;

// TODO: Handle correct positioning for diagnostics. Several captures have placeholders.
internal sealed class SyntaxNodeValidationVisitor : ISyntaxNodeVisitor
{
    private readonly Dictionary<string, List<DiagnosticInfo>> _diagnostics = new();

    internal FrozenSet<DiagnosticInfo> Diagnostics => _diagnostics.SelectMany(d => d.Value).ToFrozenSet();

    internal int ErrorCount => _diagnostics.Select(d => d.Value.Count).Sum();

    internal bool HasErrors => ErrorCount > 0;

    public void Visit(ArgumentExpression expression)
    {
        if (expression.Argument is null && expression.Comma.IsNone())
            LanguageSyntax.Throw("Empty argument added. How did this happen?");

        if (!expression.Comma.IsNone() && expression.Comma.Type != TokenType.Comma)
            LanguageSyntax.Throw("Invalid comma token added. How did this happen?");
    }

    public void Visit(BinaryExpression expression)
    {
        if (expression.Left is null)
            LanguageSyntax.Throw("Left side of a binary expression is null. How did this happen?");

        if (!expression.Operator.IsBinaryOperator())
            LanguageSyntax.Throw("Binary operator expected. How did this happen?");

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
            message = characterValue.IsSurroundedBySingleQuotes()
                ? CharacterLiteralExpression.InvalidLength
                : CharacterLiteralExpression.InvalidSingleQuotes;
        else if (characterValue.IsUnescapedSingleQuoteOrBackslash())
            message = CharacterLiteralExpression.NonEscapedCharacter;
        else if (characterValue[1] == '\\')
        {
            if (characterValue[2] == 'x')
            {
                if (!characterValue.IsValidHexadecimalCharacter())
                    message = CharacterLiteralExpression.InvalidHex;
            }
            else if (!characterValue.IsValidEscapedCharacter())
                message = CharacterLiteralExpression.InvalidEscapedCharacter;
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
            CaptureDiagnosticsInfo(expression.LeftParenthesis, FunctionCallExpression.RightParenthesisExpected);
        }
        else if (expression.Arguments is { Count: > 0 })
        {
            if (!expression.Arguments[^1].Comma.IsNone())
                CaptureDiagnosticsInfo(expression.RightParenthesis, FunctionCallExpression.LastArgumentHasComma);

            for (var i = 0; i < expression.Arguments.Count; i++)
            {
                var argument = expression.Arguments[i];

                if (argument.Argument is null)
                    LanguageSyntax.Throw("Argument is null. How did this happen?");
                else if (i != expression.Arguments.Count - 1 && argument.Comma.Type != TokenType.Comma)
                    CaptureDiagnosticsInfo(Token.None, FunctionCallExpression.CommaExpected);

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

        if (expression.Index is null)
            CaptureDiagnosticsInfo(expression.LeftBracket, IndexerExpression.IndexerExpected);
        else if (expression.RightBracket.Type != TokenType.RightBracket)
            CaptureDiagnosticsInfo(expression.LeftBracket, IndexerExpression.RightBracketExpected);
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
            LanguageSyntax.Throw("Invalid number type. How did this happen?");
    }

    public void Visit(ParenthesisedExpression expression)
    {
        if (expression.LeftParen.Type != TokenType.LeftParenthesis)
            LanguageSyntax.Throw("Invalid left parenthesis token added. How did this happen?");

        if (expression.Expression is null)
            CaptureDiagnosticsInfo(expression.LeftParen, "Expression expected.");
        else if (expression.RightParen.Type != TokenType.RightParenthesis)
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
            message = StringLiteralExpression.InvalidDoubleQuotes;
        else if (stringValue.EscapesLastDoubleQuote())
            message = StringLiteralExpression.EscapedLastDoubleQuote;
        else if (stringValue.HasInvalidEscapedCharacters())
            message = StringLiteralExpression.InvalidEscapedCharacters;

        if (!string.IsNullOrWhiteSpace(message))
            CaptureDiagnosticsInfo(expression.StringToken, message);
    }

    public void Visit(TypeArgumentExpression expression)
    {
        if (expression.Argument is null && expression.Comma.IsNone())
            LanguageSyntax.Throw("Empty argument added. How did this happen?");

        if (!expression.Comma.IsNone() && expression.Comma.Type != TokenType.Comma)
            LanguageSyntax.Throw("Invalid comma token added. How did this happen?");

        expression.Argument?.Accept(this);
    }

    public void Visit(TypeBinaryExpression expression)
    {
        if (!expression.Operator.IsBitwiseOperator())
            LanguageSyntax.Throw("Bitwise operator expected. How did this happen?");

        if (expression.Left is null)
            LanguageSyntax.Throw("Left side of a type binary expression is null. How did this happen?");

        if (expression.Right is null)
            CaptureDiagnosticsInfo(expression.Operator, TypeBinaryExpression.RightExpressionMissing);

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
            CaptureDiagnosticsInfo(expression.LessThan, TypeGenericExpression.RightGenericBracketMissing);
        }
        else if (expression.TypeArguments is { Count: > 0 })
        {
            if (!expression.TypeArguments[^1].Comma.IsNone())
                CaptureDiagnosticsInfo(expression.GreaterThan, TypeGenericExpression.LastArgumentHasComma);

            for (var i = 0; i < expression.TypeArguments.Count; i++)
            {
                var argument = expression.TypeArguments[i];

                if (argument.Argument is null)
                    LanguageSyntax.Throw("Argument is null. How did this happen?");
                else if (i != expression.TypeArguments.Count - 1 && argument.Comma.Type != TokenType.Comma)
                    CaptureDiagnosticsInfo(Token.None, TypeGenericExpression.CommaExpected);

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
        else if (statement.Terminator.Type != TokenType.Semicolon)
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
        else if (statement.Terminator.Type != TokenType.Semicolon)
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
        Visit(statement.If);

        if (statement.ElseIfs is { Count: > 0 })
        {
            foreach (var elseIf in statement.ElseIfs)
            {
                if (elseIf.ElseKeyword.Type != TokenType.Else)
                    LanguageSyntax.Throw("Invalid 'else' token added. How did this happen?");

                if (elseIf.IfBlock is null)
                    LanguageSyntax.Throw("Invalid 'else if' block. How did this happen?");

                Visit(elseIf.IfBlock);
            }
        }

        if (statement.Else is null)
            return;

        if (statement.Else.ElseKeyword.Type != TokenType.Else)
            LanguageSyntax.Throw("Invalid 'else' token added. How did this happen?");

        if (statement.Else.Block is null)
            LanguageSyntax.Throw("Invalid 'else' block. How did this happen?");

        statement.Else.Block.Accept(this);
    }

    private void Visit(IfElseStatement.IfBlock block)
    {
        if (block.IfKeyword.Type != TokenType.If)
            LanguageSyntax.Throw("Invalid 'if' token added. How did this happen?");

        if (block.LeftParenthesis.Type != TokenType.LeftParenthesis)
            CaptureDiagnosticsInfo(block.IfKeyword, "Left parenthesis token expected after 'if' keyword.");
        else if (block.Condition is null)
            CaptureDiagnosticsInfo(block.LeftParenthesis, "Expression expected.");
        else if (block.RightParenthesis.Type != TokenType.RightParenthesis)
            CaptureDiagnosticsInfo(block.IfKeyword, "Right parenthesis expected after the 'if' condition.");
        else if (block.Block is null)
            CaptureDiagnosticsInfo(block.RightParenthesis, "Statement expected.");
        else
            block.Block.Accept(this);
    }

    public void Visit(NoOpStatement statement)
    {
        if (statement.Terminator.Type != TokenType.Semicolon)
            LanguageSyntax.Throw("Invalid semicolon token added. How did this happen?");
    }

    public void Visit(ReturnStatement statement)
    {
        if (statement.ReturnKeyword.Type != TokenType.Return)
            LanguageSyntax.Throw("Invalid 'return' token added. How did this happen?");

        if (statement.Terminator.Type != TokenType.Semicolon)
            CaptureDiagnosticsInfo(statement.Terminator, "Semicolon expected at the end of a 'return' statement.");

        if (statement.ReturnArguments is not { Count: > 0 })
            return;

        if (!statement.ReturnArguments[^1].Comma.IsNone())
            CaptureDiagnosticsInfo(statement.Terminator, "Last argument should not be followed by comma.");

        for (var i = 0; i < statement.ReturnArguments.Count; i++)
        {
            var argument = statement.ReturnArguments[i];

            if (argument.Argument is null)
                CaptureDiagnosticsInfo(Token.None, "Argument expected.");
            else if (i != statement.ReturnArguments.Count - 1 && argument.Comma.Type != TokenType.Comma)
                CaptureDiagnosticsInfo(Token.None, "Comma expected.");

            argument.Accept(this);
        }
    }

    public void Visit(VariableDeclarationStatement statement)
    {
        if (statement.DeclarationKeyword.Type != TokenType.Var && statement.DeclarationKeyword.Type != TokenType.Const)
            LanguageSyntax.Throw("Invalid declaration token added. How did this happen?");

        if (statement.Identifier is null)
            CaptureDiagnosticsInfo(statement.DeclarationKeyword, VariableDeclarationStatement.IdentifierExpected);

        if (statement.TypeAssignment.Type != TokenType.Colon)
            CaptureDiagnosticsInfo(statement.DeclarationKeyword, VariableDeclarationStatement.ColonExpected);

        if (statement.ValueAssignment.Type != TokenType.None && statement.ValueAssignment.Type != TokenType.Assign)
            CaptureDiagnosticsInfo(statement.ValueAssignment, VariableDeclarationStatement.AssignmentExpected);

        if (statement.ValueAssignment.Type == TokenType.Assign && statement.Value is null)
            CaptureDiagnosticsInfo(statement.ValueAssignment, VariableDeclarationStatement.ExpressionExpected);

        if (statement.Type is null && statement.Value is null)
            CaptureDiagnosticsInfo(statement.TypeAssignment, VariableDeclarationStatement.TypeExpressionExpected);

        if (statement.Terminator.Type != TokenType.Semicolon)
            CaptureDiagnosticsInfo(statement.Terminator, VariableDeclarationStatement.SemicolonExpected);
    }

    public void Visit(WhileStatement statement)
    {
        if (statement.WhileKeyword.Type != TokenType.While)
            LanguageSyntax.Throw("Invalid 'while' token added. How did this happen?");

        if (statement.LeftParenthesis.Type != TokenType.LeftParenthesis)
            CaptureDiagnosticsInfo(
                statement.LeftParenthesis,
                "Left parenthesis token expected after 'while' keyword or its label");

        if (statement.Condition is null)
            CaptureDiagnosticsInfo(statement.LeftParenthesis, "Expression expected.");

        if (statement.RightParenthesis.Type != TokenType.RightParenthesis)
            CaptureDiagnosticsInfo(
                statement.LeftParenthesis,
                "Right parenthesis token expected after the 'while' condition");

        if (statement.Block is null)
            CaptureDiagnosticsInfo(statement.RightParenthesis, "'while' block expected.");
        else
            statement.Block.Accept(this);
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