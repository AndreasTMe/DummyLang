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
        if (expression.Argument is null)
            LanguageSyntax.Throw("Argument is null. How did this happen?");

        if (!expression.Comma.IsNone() && expression.Comma.Type != TokenType.Comma)
            LanguageSyntax.Throw("Invalid comma token added. How did this happen?");

        expression.Argument?.Accept(this);
    }

    public void Visit(BinaryExpression expression)
    {
        if (expression.Left is null)
            LanguageSyntax.Throw("Left side of a binary expression is null. How did this happen?");

        if (!expression.Operator.IsBinaryOperator())
            LanguageSyntax.Throw("Binary operator expected. How did this happen?");

        if (expression.Right is null)
            CaptureDiagnosticsInfo(
                expression.Operator,
                expression.Positions[1],
                BinaryExpression.RightExpressionMissing);

        expression.Left?.Accept(this);
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

        if (string.IsNullOrWhiteSpace(message))
            return;

        CaptureDiagnosticsInfo(expression.CharacterToken, expression.Positions[0], message);
    }

    public void Visit(FunctionCallExpression expression)
    {
        if (expression.Identifier.Type != TokenType.Identifier)
            LanguageSyntax.Throw("Invalid identifier token added. How did this happen?");

        if (expression.LeftParenthesis.Type != TokenType.LeftParenthesis)
            LanguageSyntax.Throw("Invalid left parenthesis token added. How did this happen?");

        if (expression.RightParenthesis.Type != TokenType.RightParenthesis)
        {
            CaptureDiagnosticsInfo(
                expression.RightParenthesis,
                expression.Positions[1],
                FunctionCallExpression.RightParenthesisExpected);
        }
        else if (expression.Arguments is { Count: > 0 })
        {
            foreach (var argument in expression.Arguments)
                argument.Accept(this);

            if (expression.Arguments[^1].Comma.IsNone())
                return;

            var arguments = expression.Arguments[^1];
            CaptureDiagnosticsInfo(
                arguments.Comma,
                arguments.Argument?.Positions[^1, false] ?? arguments.Positions[1],
                FunctionCallExpression.LastArgumentHasComma);
        }
    }

    public void Visit(IdentifierExpression expression)
    {
        if (expression.Token.Type != TokenType.Identifier)
            LanguageSyntax.Throw("Invalid identifier token added. How did this happen?");
    }

    public void Visit(IndexArgumentExpression expression)
    {
        if (expression.Argument is null && expression.Comma.IsNone())
            LanguageSyntax.Throw("Empty argument added. How did this happen?");

        if (!expression.Comma.IsNone() && expression.Comma.Type != TokenType.Comma)
            LanguageSyntax.Throw("Invalid comma token added. How did this happen?");

        expression.Argument?.Accept(this);
    }

    public void Visit(IndexerExpression expression)
    {
        if (expression.Identifier.Type != TokenType.Identifier)
            LanguageSyntax.Throw("Invalid identifier token added. How did this happen?");

        if (expression.LeftBracket.Type != TokenType.LeftBracket)
            LanguageSyntax.Throw("Invalid left bracket token added. How did this happen?");

        if (expression.Indices is null || expression.Indices.Count == 0)
            CaptureDiagnosticsInfo(
                expression.LeftBracket,
                expression.Positions[1],
                IndexerExpression.IndexerExpected);
        else if (expression.RightBracket.Type != TokenType.RightBracket)
            CaptureDiagnosticsInfo(
                expression.LeftBracket,
                expression.Positions[1],
                IndexerExpression.RightBracketExpected);
        else
            foreach (var index in expression.Indices)
                index.Accept(this);
    }

    public void Visit(MemberAccessExpression expression)
    {
        if (expression.Access.Type != TokenType.PointerAccess && expression.Access.Type != TokenType.Dot)
            LanguageSyntax.Throw("Invalid access token added. How did this happen?");

        if (expression.Member is null)
            CaptureDiagnosticsInfo(
                expression.Access,
                expression.Positions[1],
                MemberAccessExpression.IdentifierExpected);

        expression.Identifier?.Accept(this);
        expression.Member?.Accept(this);
    }

    public void Visit(NumberLiteralExpression expression)
    {
        if (expression.Type == NumberType.None)
            LanguageSyntax.Throw("Invalid number type. How did this happen?");
    }

    public void Visit(ParameterExpression expression)
    {
        if (expression.Parameter.Type != TokenType.Identifier)
            LanguageSyntax.Throw("Invalid parameter token added. How did this happen?");

        if (!expression.Comma.IsNone() && expression.Comma.Type != TokenType.Comma)
            LanguageSyntax.Throw("Invalid comma token added. How did this happen?");
    }

    public void Visit(ParenthesisedExpression expression)
    {
        if (expression.LeftParen.Type != TokenType.LeftParenthesis)
            LanguageSyntax.Throw("Invalid left parenthesis token added. How did this happen?");

        if (expression.Expression is null)
        {
            CaptureDiagnosticsInfo(
                expression.LeftParen,
                expression.Positions[0],
                ParenthesisedExpression.ExpressionExpected);
        }
        else if (expression.RightParen.Type != TokenType.RightParenthesis)
        {
            var position = TokenPosition.Zero;
            switch (expression.Expression)
            {
                case IExpression expr:
                    position = expr.Positions[^1];
                    break;
                case ITypeExpression typeExpr:
                    position = typeExpr.Positions[^1];
                    break;
                default:
                    LanguageSyntax.Throw("");
                    break;
            }
            CaptureDiagnosticsInfo(
                expression.RightParen,
                position,
                ParenthesisedExpression.RightParenthesisExpected);
        }

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
            CaptureDiagnosticsInfo(
                expression.StringToken,
                expression.Positions[0],
                message);
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
            CaptureDiagnosticsInfo(
                expression.Operator,
                expression.Positions[1],
                TypeBinaryExpression.RightExpressionMissing);

        expression.Left?.Accept(this);
        expression.Right?.Accept(this);
    }

    public void Visit(TypeFunctionExpression expression)
    {
        if (expression.LeftParen.Type != TokenType.LeftParenthesis)
            LanguageSyntax.Throw("Invalid left parenthesis token added. How did this happen?");

        if (expression.InputTypes is { Count: > 0 })
        {
            foreach (var inputType in expression.InputTypes)
                inputType.Accept(this);

            if (expression.InputTypes[^1].Comma.IsNone())
                return;

            var inputTypes = expression.InputTypes[^1];
            CaptureDiagnosticsInfo(
                inputTypes.Comma,
                inputTypes.Argument?.Positions[^1, false] ?? inputTypes.Positions[1],
                TypeFunctionExpression.LastParameterHasComma);
        }
        else if (expression.RightParen.Type != TokenType.RightParenthesis)
        {
            CaptureDiagnosticsInfo(
                expression.RightParen,
                expression.InputTypes?[^1].Argument?.Positions[^1, false] ?? expression.Positions[0],
                TypeFunctionExpression.RightParenthesisExpected);
        }
        else if (expression.LambdaAssign.Type != TokenType.LambdaAssign)
        {
            CaptureDiagnosticsInfo(
                expression.LambdaAssign,
                expression.Positions[2],
                TypeFunctionExpression.LambdaAssignExpected);
        }
        else if (expression.OutputTypes is { Count: > 0 })
        {
            foreach (var outputType in expression.OutputTypes)
                outputType.Accept(this);

            if (expression.OutputTypes[^1].Comma.IsNone())
                return;

            var outputTypes = expression.OutputTypes[^1];
            CaptureDiagnosticsInfo(
                outputTypes.Comma,
                outputTypes.Argument?.Positions[^1, false] ?? outputTypes.Positions[1],
                TypeFunctionExpression.LastArgumentHasComma);
        }
    }

    public void Visit(TypeGenericExpression expression)
    {
        if (expression.Identifier.Type != TokenType.Identifier)
            LanguageSyntax.Throw("Invalid identifier token added. How did this happen?");

        if (expression.LessThan.Type != TokenType.LessThan)
            LanguageSyntax.Throw("Invalid left generic bracket token added. How did this happen?");

        if (expression.GreaterThan.Type != TokenType.GreaterThan)
        {
            CaptureDiagnosticsInfo(
                expression.GreaterThan,
                expression.Positions[^3],
                TypeGenericExpression.RightGenericBracketMissing);
        }
        else if (expression.TypeArguments is { Count: > 0 })
        {
            foreach (var typeArgument in expression.TypeArguments)
                typeArgument.Accept(this);

            if (expression.TypeArguments[^1].Comma.IsNone())
                return;

            var typeArguments = expression.TypeArguments[^1];
            CaptureDiagnosticsInfo(
                typeArguments.Comma,
                typeArguments.Argument?.Positions[^1, false] ?? typeArguments.Positions[1],
                TypeGenericExpression.LastArgumentHasComma);
        }
    }

    public void Visit(TypeIdentifierExpression expression)
    {
        if (expression.Token.Type != TokenType.Identifier)
            LanguageSyntax.Throw("Invalid identifier token added. How did this happen?");
    }

    public void Visit(TypeParameterExpression expression)
    {
        if (expression.Identifier.Type != TokenType.Identifier)
            LanguageSyntax.Throw("Invalid parameter identifier token. How did this happen?");

        if (!expression.Comma.IsNone() && expression.Colon.Type != TokenType.Colon)
            LanguageSyntax.Throw("Invalid colon token added. How did this happen?");

        if (expression.Type is null)
            LanguageSyntax.Throw("Empty type added. How did this happen?");

        if (!expression.Comma.IsNone() && expression.Comma.Type != TokenType.Comma)
            LanguageSyntax.Throw("Invalid comma token added. How did this happen?");

        expression.Type?.Accept(this);
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
            CaptureDiagnosticsInfo(
                expression.Token,
                expression.Positions[0],
                UnaryExpression.ExpressionExpected);

        // TODO: A lot of checks here for the inner expression
    }

    public void Visit(UnexpectedTokenExpression expression) =>
        CaptureDiagnosticsInfo(expression.Token, expression.Positions[0], UnexpectedTokenExpression.UnexpectedToken);

    public void Visit(BreakStatement statement)
    {
        if (statement.BreakKeyword.Type != TokenType.Break)
            LanguageSyntax.Throw("Invalid 'break' token added. How did this happen?");

        if (!statement.Label.IsNone() && statement.Label.Type != TokenType.Identifier)
            CaptureDiagnosticsInfo(
                statement.Label,
                statement.Positions[1],
                BreakStatement.InvalidToken);
        else if (statement.Terminator.Type != TokenType.Semicolon)
            CaptureDiagnosticsInfo(
                statement.Terminator,
                statement.Positions[2],
                BreakStatement.SemicolonExpected);
    }

    public void Visit(CompoundStatement statement)
    {
        if (statement.LeftBrace.Type != TokenType.LeftBrace)
            LanguageSyntax.Throw("Invalid left brace token added. How did this happen?");

        if (statement.RightBrace.Type != TokenType.RightBrace)
            CaptureDiagnosticsInfo(
                statement.RightBrace,
                !statement.RightBrace.IsNone()
                    ? statement.Positions[2]
                    : statement.Statements?[^1].Positions[^1] ?? statement.Positions[0],
                CompoundStatement.RightBraceExpected);

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
            CaptureDiagnosticsInfo(
                statement.Label,
                statement.Positions[1],
                ContinueStatement.InvalidToken);
        else if (statement.Terminator.Type != TokenType.Semicolon)
            CaptureDiagnosticsInfo(
                statement.Terminator,
                statement.Positions[2],
                ContinueStatement.SemicolonExpected);
    }

    public void Visit(ExpressionStatement statement)
    {
        if (statement.Expression is null)
            LanguageSyntax.Throw($"This should have been a {nameof(NoOpStatement)}. How did this happen?");

        if (statement.Terminator.Type != TokenType.Semicolon)
            CaptureDiagnosticsInfo(
                statement.Terminator,
                statement.Positions[1],
                ExpressionStatement.SemicolonExpected);

        statement.Expression?.Accept(this);
    }

    public void Visit(FunctionDeclarationStatement statement)
    {
        if (statement.FuncKeyword.Type != TokenType.Func)
            LanguageSyntax.Throw("Invalid 'func' token added. How did this happen?");

        if (statement.Identifier.Type != TokenType.Identifier)
        {
            CaptureDiagnosticsInfo(
                statement.Identifier,
                statement.Positions[1],
                FunctionDeclarationStatement.InvalidToken);
        }
        else if (statement.TypeAssignment.Type != TokenType.Colon)
        {
            CaptureDiagnosticsInfo(
                statement.TypeAssignment,
                statement.Positions[2],
                FunctionDeclarationStatement.TypeAssignmentExpected);
        }
        else if (statement.Type is null)
        {
            CaptureDiagnosticsInfo(
                statement.TypeAssignment,
                statement.Positions[3],
                FunctionDeclarationStatement.TypeExpressionExpected);
        }
        else if (statement.Block is null)
        {
            CaptureDiagnosticsInfo(
                statement.LambdaAssign,
                statement.Positions[8],
                FunctionDeclarationStatement.CompoundStatementExpected);
        }
        else
        {
            statement.Type?.Accept(this);
            statement.Block?.Accept(this);
        }
    }

    public void Visit(FunctionDeclarationWithImplicitTypeStatement statement)
    {
        if (statement.FuncKeyword.Type != TokenType.Func)
            LanguageSyntax.Throw("Invalid 'func' token added. How did this happen?");

        if (statement.Identifier.Type != TokenType.Identifier)
        {
            CaptureDiagnosticsInfo(
                statement.Identifier,
                statement.Positions[1],
                FunctionDeclarationWithImplicitTypeStatement.InvalidToken);
        }
        else if (statement.TypeAssignment.Type != TokenType.Colon)
        {
            CaptureDiagnosticsInfo(
                statement.TypeAssignment,
                statement.Positions[2],
                FunctionDeclarationWithImplicitTypeStatement.TypeAssignmentExpected);
        }

        if (statement.TypedParameters is { Count: > 0 })
        {
            if (!statement.TypedParameters[^1].Comma.IsNone())
                CaptureDiagnosticsInfo(
                    statement.TypedParameters[^1].Comma,
                    statement.TypedParameters[^1].Positions[^1],
                    FunctionDeclarationWithImplicitTypeStatement.LastArgumentHasComma);

            for (var i = 0; i < statement.TypedParameters.Count; i++)
            {
                var typedParameter = statement.TypedParameters[i];

                if (typedParameter.Identifier.Type != TokenType.Identifier)
                    CaptureDiagnosticsInfo(
                        typedParameter.Identifier,
                        typedParameter.Positions[0],
                        FunctionDeclarationWithImplicitTypeStatement.IdentifierExpected);
                else if (typedParameter.Colon.Type != TokenType.Colon)
                    CaptureDiagnosticsInfo(
                        typedParameter.Colon,
                        typedParameter.Positions[1],
                        FunctionDeclarationWithImplicitTypeStatement.ColonExpected);
                else if (typedParameter.Type is null)
                    CaptureDiagnosticsInfo(
                        typedParameter.Colon,
                        typedParameter.Positions[2],
                        FunctionDeclarationWithImplicitTypeStatement.TypeExpressionExpected);
                else if (i != statement.TypedParameters.Count - 1 && typedParameter.Comma.Type != TokenType.Comma)
                    CaptureDiagnosticsInfo(
                        typedParameter.Comma,
                        typedParameter.Positions[3],
                        FunctionDeclarationWithImplicitTypeStatement.CommaExpected);

                typedParameter.Accept(this);
            }
        }

        if (statement.Block is null)
        {
            CaptureDiagnosticsInfo(
                statement.FuncKeyword,
                statement.Positions[8],
                FunctionDeclarationWithImplicitTypeStatement.CompoundStatementExpected);
        }
        else
        {
            statement.Block.Accept(this);
        }
    }

    public void Visit(IfElseStatement statement)
    {
        if (statement.If is not null)
            Visit(statement.If);
        else if (statement.ElseIfs is not null)
            CaptureDiagnosticsInfo(
                statement.ElseIfs[0].ElseKeyword,
                statement.Positions[5],
                IfElseStatement.ElseBeforeIf);
        else if (statement.Else is not null)
            CaptureDiagnosticsInfo(
                statement.Else.ElseKeyword,
                statement.Positions[^2],
                IfElseStatement.ElseBeforeIf);

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
            CaptureDiagnosticsInfo(
                statement.Else.ElseKeyword,
                statement.Positions[^2],
                IfElseStatement.CompoundStatementExpected);
        else
            statement.Else.Block.Accept(this);
    }

    private void Visit(IfElseStatement.IfBlock block)
    {
        if (block.IfKeyword.Type != TokenType.If)
            LanguageSyntax.Throw("Invalid 'if' token added. How did this happen?");

        if (block.LeftParenthesis.Type != TokenType.LeftParenthesis)
            CaptureDiagnosticsInfo(
                block.IfKeyword,
                block.Positions[1],
                IfElseStatement.LeftParenthesisExpected);
        else if (block.Condition is null)
            CaptureDiagnosticsInfo(
                block.LeftParenthesis,
                block.Positions[2],
                IfElseStatement.ExpressionExpected);
        else if (block.RightParenthesis.Type != TokenType.RightParenthesis)
            CaptureDiagnosticsInfo(
                block.RightParenthesis,
                block.Positions[2],
                IfElseStatement.RightParenthesisExpected);
        else if (block.Block is null)
            CaptureDiagnosticsInfo(
                block.RightParenthesis,
                block.Positions[3],
                IfElseStatement.CompoundStatementExpected);
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
        {
            CaptureDiagnosticsInfo(
                statement.Terminator,
                statement.Positions[2],
                ReturnStatement.SemicolonExpected);
        }
        else if (statement.ReturnArguments is { Count: > 0 })
        {
            foreach (var returnArgument in statement.ReturnArguments)
                returnArgument.Accept(this);

            if (!statement.ReturnArguments[^1].Comma.IsNone())
                CaptureDiagnosticsInfo(
                    statement.ReturnArguments[^1].Comma,
                    statement.ReturnArguments[^1].Positions[1],
                    ReturnStatement.LastArgumentHasComma);
        }
    }

    public void Visit(VariableDeclarationStatement statement)
    {
        if (statement.DeclarationKeyword.Type != TokenType.Var && statement.DeclarationKeyword.Type != TokenType.Const)
            LanguageSyntax.Throw("Invalid declaration token added. How did this happen?");

        if (statement.Identifier.Type != TokenType.Identifier)
            CaptureDiagnosticsInfo(statement.DeclarationKeyword, VariableDeclarationStatement.IdentifierExpected);
        else if (statement.TypeAssignment.Type != TokenType.Colon)
            CaptureDiagnosticsInfo(statement.DeclarationKeyword, VariableDeclarationStatement.ColonExpected);
        else if (statement.ValueAssignment.Type != TokenType.None && statement.ValueAssignment.Type != TokenType.Assign)
            CaptureDiagnosticsInfo(statement.ValueAssignment, VariableDeclarationStatement.AssignmentExpected);
        else if (statement.ValueAssignment.Type == TokenType.Assign && statement.Value is null)
            CaptureDiagnosticsInfo(statement.ValueAssignment, VariableDeclarationStatement.ExpressionExpected);
        else if (statement.Type is null && statement.Value is null)
            CaptureDiagnosticsInfo(statement.TypeAssignment, VariableDeclarationStatement.TypeExpressionExpected);
        else if (statement.Terminator.Type != TokenType.Semicolon)
            CaptureDiagnosticsInfo(statement.Terminator, VariableDeclarationStatement.SemicolonExpected);
    }

    public void Visit(VariableDeclarationWithImplicitTypeStatement statement)
    {
        if (statement.DeclarationKeyword.Type != TokenType.Var && statement.DeclarationKeyword.Type != TokenType.Const)
            LanguageSyntax.Throw("Invalid declaration token added. How did this happen?");

        if (statement.Identifier.Type != TokenType.Identifier)
            CaptureDiagnosticsInfo(
                statement.DeclarationKeyword,
                VariableDeclarationWithImplicitTypeStatement.IdentifierExpected);
        else if (statement.TypeAssignment.Type != TokenType.Colon)
            CaptureDiagnosticsInfo(
                statement.DeclarationKeyword,
                VariableDeclarationWithImplicitTypeStatement.ColonExpected);
        else if (statement.ValueAssignment.Type != TokenType.None && statement.ValueAssignment.Type != TokenType.Assign)
            CaptureDiagnosticsInfo(
                statement.ValueAssignment,
                VariableDeclarationWithImplicitTypeStatement.AssignmentExpected);
        else if (statement.ValueAssignment.Type == TokenType.Assign && statement.Value is null)
            CaptureDiagnosticsInfo(
                statement.ValueAssignment,
                VariableDeclarationWithImplicitTypeStatement.ExpressionExpected);
        else if (statement.Terminator.Type != TokenType.Semicolon)
            CaptureDiagnosticsInfo(
                statement.Terminator,
                VariableDeclarationWithImplicitTypeStatement.SemicolonExpected);
    }

    public void Visit(WhileStatement statement)
    {
        if (statement.WhileKeyword.Type != TokenType.While)
            LanguageSyntax.Throw("Invalid 'while' token added. How did this happen?");

        if (statement.LeftParenthesis.Type != TokenType.LeftParenthesis)
            CaptureDiagnosticsInfo(statement.LeftParenthesis, WhileStatement.LeftParenthesisExpected);
        else if (statement.Condition is null)
            CaptureDiagnosticsInfo(statement.LeftParenthesis, WhileStatement.ExpressionExpected);
        else if (statement.RightParenthesis.Type != TokenType.RightParenthesis)
            CaptureDiagnosticsInfo(statement.LeftParenthesis, WhileStatement.RightParenthesisExpected);
        else if (statement.Block is null)
            CaptureDiagnosticsInfo(statement.RightParenthesis, WhileStatement.WhileBlockExpected);
        else
            statement.Block.Accept(this);
    }

    [System.Obsolete]
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

        const string sourcePath = "C:/ProjectPath/ProjectFile.dum";

        if (!_diagnostics.ContainsKey(sourcePath))
            _diagnostics[sourcePath] = [];

        _diagnostics[sourcePath]
            .Add(new DiagnosticInfo(message.TrimEnd(), sourcePath, token.Position.Line, token.Position.Column));
    }

    private void CaptureDiagnosticsInfo(Token token, TokenPosition errorPosition, string message)
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
            .Add(new DiagnosticInfo(message.TrimEnd(), sourcePath, errorPosition.Line, errorPosition.Column));
    }
}