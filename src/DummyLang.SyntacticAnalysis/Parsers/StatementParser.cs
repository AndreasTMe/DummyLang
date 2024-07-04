using DummyLang.LexicalAnalysis;
using DummyLang.LexicalAnalysis.Extensions;
using DummyLang.SyntacticAnalysis.Abstractions;
using DummyLang.SyntacticAnalysis.Expressions;
using DummyLang.SyntacticAnalysis.Statements;
using DummyLang.SyntacticAnalysis.Utilities;
using System.Collections.Generic;
using System.Diagnostics;

namespace DummyLang.SyntacticAnalysis.Parsers;

internal static class StatementParser
{
    public static IStatement Parse(ref int index, in Token[] tokens)
    {
        Debug.Assert(index < tokens.Length);

        var token = TokenAt(index, in tokens);
        switch (token.Type)
        {
            case TokenType.Semicolon:
                return new NoOpStatement(GetAndMoveToNext(ref index, in tokens));
            case TokenType.LeftBrace:
                return ParseBlock(ref index, in tokens);
            case TokenType.Var:
            case TokenType.Const:
                return ParseVariableDeclaration(ref index, in tokens);
            // TODO: case TokenType.Func:
            case TokenType.If:
                return ParseIfElse(ref index, in tokens);
            case TokenType.Else:
                LanguageSyntax.Found(token, "The 'else' keyword can only come after an 'if'/'else if' block.");
                break;
            case TokenType.Break:
                return ParseBreak(ref index, in tokens);
            case TokenType.While:
                return ParseWhile(ref index, in tokens);
            case TokenType.Continue:
                return ParseContinue(ref index, in tokens);
            case TokenType.Return:
                return ParseReturn(ref index, in tokens);
        }

        return ParseExpression(ref index, in tokens);
    }

    private static Token GetAndMoveToNext(ref int index, in Token[] tokens) =>
        index >= 0 && index < tokens.Length ? tokens[index++] : Token.None;

    private static TokenType TypeAt(int index, in Token[] tokens) =>
        index >= 0 && index < tokens.Length ? tokens[index].Type : TokenType.None;

    private static Token TokenAt(int index, in Token[] tokens) =>
        index >= 0 && index < tokens.Length ? tokens[index] : Token.None;

    private static CompoundStatement ParseBlock(ref int index, in Token[] tokens)
    {
        var leftBrace = GetAndMoveToNext(ref index, in tokens);

        var statements  = new List<IStatement>();
        var next        = TokenAt(index, in tokens);
        var shouldThrow = next.IsNone() || next.IsEndOfFile();
        while (next.Type != TokenType.RightBrace && !shouldThrow)
        {
            statements.Add(Parse(ref index, in tokens));

            next = TokenAt(index, in tokens);
            if (!next.IsNone() && !next.IsEndOfFile())
                continue;

            shouldThrow = true;
            break;
        }

        if (shouldThrow)
            LanguageSyntax.Found(next, "Could not find closing brace for block.");

        var rightBrace = GetAndMoveToNext(ref index, in tokens);

        return new CompoundStatement(leftBrace, statements.ToArray(), rightBrace);
    }

    private static VariableDeclarationStatement ParseVariableDeclaration(ref int index, in Token[] tokens)
    {
        var declarationKeyword = GetAndMoveToNext(ref index, in tokens);

        if (TypeAt(index, in tokens) != TokenType.Identifier)
            LanguageSyntax.Expects(
                TokenType.Identifier,
                tokens[index],
                $"Identifier expected after '{declarationKeyword.Value}' keyword.");

        var identifier = new IdentifierExpression(GetAndMoveToNext(ref index, in tokens));

        if (TypeAt(index, in tokens) != TokenType.Colon)
            LanguageSyntax.Expects(
                TokenType.Colon,
                tokens[index],
                "Colon expected after identifier in a variable declaration statement.");

        var              typeAssignmentOperator = GetAndMoveToNext(ref index, in tokens);
        ITypeExpression? typeValue              = default;

        if (TypeAt(index, in tokens) != TokenType.Assign)
            typeValue = ExpressionParser.ParseType(ref index, in tokens);

        var          valueAssignmentOperator = Token.None;
        IExpression? valueAssignment         = default;

        if (TypeAt(index, in tokens) == TokenType.Assign)
        {
            valueAssignmentOperator = GetAndMoveToNext(ref index, in tokens);
            valueAssignment         = ExpressionParser.Parse(ref index, in tokens);
        }

        if (TypeAt(index, in tokens) != TokenType.Semicolon)
            LanguageSyntax.Expects(
                TokenType.Semicolon,
                tokens[index],
                "Semicolon expected at the end of a variable declaration statement.");

        var terminator = GetAndMoveToNext(ref index, in tokens);

        return new VariableDeclarationStatement(
            declarationKeyword,
            identifier,
            typeAssignmentOperator,
            typeValue,
            valueAssignmentOperator,
            valueAssignment,
            terminator);
    }

    private static IfElseStatement ParseIfElse(ref int index, in Token[] tokens)
    {
        var ifPart      = ParseIfPart(ref index, in tokens);
        var elseIfParts = new List<IfElseStatement.ElseIfBlock>();

        while (TypeAt(index, in tokens) == TokenType.Else && TypeAt(index + 1, in tokens) == TokenType.If)
            elseIfParts.Add(ParseElseIfPart(ref index, in tokens));

        return TypeAt(index, in tokens) == TokenType.Else
            ? new IfElseStatement(ifPart, elseIfParts, ParseElsePart(ref index, in tokens))
            : new IfElseStatement(ifPart, elseIfParts, null);
    }

    private static IfElseStatement.IfBlock ParseIfPart(ref int index, in Token[] tokens)
    {
        var ifKeyword = GetAndMoveToNext(ref index, in tokens);

        if (TypeAt(index, in tokens) != TokenType.LeftParenthesis)
            LanguageSyntax.Expects(
                TokenType.LeftParenthesis,
                tokens[index],
                "Left parenthesis token expected after 'if' keyword.");

        var leftParenthesis = GetAndMoveToNext(ref index, in tokens);

        if (TypeAt(index, in tokens) == TokenType.RightParenthesis)
            LanguageSyntax.Found(tokens[index], "Condition expected for 'if' statement.");

        var condition = ExpressionParser.Parse(ref index, in tokens);

        if (TypeAt(index, in tokens) != TokenType.RightParenthesis)
            LanguageSyntax.Expects(
                TokenType.RightParenthesis,
                tokens[index],
                "Right parenthesis expected after the end of the condition.");

        var rightParenthesis = GetAndMoveToNext(ref index, in tokens);

        if (TypeAt(index, in tokens) != TokenType.LeftBrace)
            LanguageSyntax.Expects(TokenType.LeftBrace, tokens[index], "Left brace expected to open an 'if' block.");

        var ifBlock = ParseBlock(ref index, in tokens);

        return new IfElseStatement.IfBlock(ifKeyword, leftParenthesis, condition, rightParenthesis, ifBlock);
    }

    private static IfElseStatement.ElseIfBlock ParseElseIfPart(ref int index, in Token[] tokens)
    {
        var elseKeyword = GetAndMoveToNext(ref index, in tokens);
        var ifPart      = ParseIfPart(ref index, in tokens);

        return new IfElseStatement.ElseIfBlock(elseKeyword, ifPart);
    }

    private static IfElseStatement.ElseBlock ParseElsePart(ref int index, in Token[] tokens)
    {
        var elseKeyword = GetAndMoveToNext(ref index, in tokens);

        if (TypeAt(index, in tokens) != TokenType.LeftBrace)
            LanguageSyntax.Expects(TokenType.LeftBrace, tokens[index], "Left brace expected to open an 'else' block.");

        var elseBlock = ParseBlock(ref index, in tokens);

        return new IfElseStatement.ElseBlock(elseKeyword, elseBlock);
    }

    private static BreakStatement ParseBreak(ref int index, in Token[] tokens)
    {
        var breakKeyword = GetAndMoveToNext(ref index, in tokens);

        var label = Token.None;
        if (TypeAt(index, in tokens) == TokenType.Identifier)
            label = GetAndMoveToNext(ref index, in tokens);

        if (TypeAt(index, in tokens) != TokenType.Semicolon)
            LanguageSyntax.Expects(
                TokenType.Semicolon,
                tokens[index],
                "Semicolon expected at the end of a 'break' statement.");

        var terminator = GetAndMoveToNext(ref index, in tokens);

        return new BreakStatement(breakKeyword, label, terminator);
    }

    private static WhileStatement ParseWhile(ref int index, in Token[] tokens)
    {
        var whileKeyword = GetAndMoveToNext(ref index, in tokens);

        var label = Token.None;
        if (TypeAt(index, in tokens) == TokenType.Identifier)
            label = GetAndMoveToNext(ref index, in tokens);

        if (TypeAt(index, in tokens) != TokenType.LeftParenthesis)
            LanguageSyntax.Expects(
                TokenType.LeftParenthesis,
                tokens[index],
                "Left parenthesis token expected after 'while' keyword or its label.");

        var leftParenthesis = GetAndMoveToNext(ref index, in tokens);

        if (TypeAt(index, in tokens) == TokenType.RightParenthesis)
            LanguageSyntax.Found(tokens[index], "Condition expected for 'while' statement.");

        var condition = ExpressionParser.Parse(ref index, in tokens);

        if (TypeAt(index, in tokens) != TokenType.RightParenthesis)
            LanguageSyntax.Expects(
                TokenType.RightParenthesis,
                tokens[index],
                "Right parenthesis expected after the end of the condition.");

        var rightParenthesis = GetAndMoveToNext(ref index, in tokens);

        if (TypeAt(index, in tokens) != TokenType.LeftBrace)
            LanguageSyntax.Expects(TokenType.LeftBrace, tokens[index], "Left brace expected to open a 'while' block.");

        var whileBlock = ParseBlock(ref index, in tokens);

        return new WhileStatement(whileKeyword, label, leftParenthesis, condition, rightParenthesis, whileBlock);
    }

    private static ContinueStatement ParseContinue(ref int index, in Token[] tokens)
    {
        var continueKeyword = GetAndMoveToNext(ref index, in tokens);

        var label = Token.None;
        if (TypeAt(index, in tokens) == TokenType.Identifier)
            label = GetAndMoveToNext(ref index, in tokens);

        if (TypeAt(index, in tokens) != TokenType.Semicolon)
            LanguageSyntax.Expects(
                TokenType.Semicolon,
                tokens[index],
                "Semicolon expected at the end of a 'continue' statement.");

        var terminator = GetAndMoveToNext(ref index, in tokens);

        return new ContinueStatement(continueKeyword, label, terminator);
    }

    private static ReturnStatement ParseReturn(ref int index, in Token[] tokens)
    {
        var returnKeyword = GetAndMoveToNext(ref index, in tokens);

        var expressions = new List<IExpression>();
        if (TypeAt(index, in tokens) != TokenType.Semicolon)
        {
            expressions.Add(ExpressionParser.Parse(ref index, in tokens));

            if (TokenAt(index, in tokens).IsIdentifierOrLiteral())
                LanguageSyntax.Expects(
                    TokenType.Comma,
                    tokens[index],
                    "Comma expected between multiple 'return' arguments.");

            while (TypeAt(index, in tokens) == TokenType.Comma)
            {
                index++;
                expressions.Add(ExpressionParser.Parse(ref index, in tokens));

                if (TokenAt(index, in tokens).IsIdentifierOrLiteral())
                    LanguageSyntax.Expects(
                        TokenType.Comma,
                        tokens[index],
                        "Comma expected between multiple 'return' arguments.");
            }
        }

        if (TypeAt(index, in tokens) != TokenType.Semicolon)
            LanguageSyntax.Expects(
                TokenType.Semicolon,
                tokens[index],
                "Semicolon expected at the end of a 'return' statement.");

        var terminator = GetAndMoveToNext(ref index, in tokens);

        return new ReturnStatement(returnKeyword, expressions.ToArray(), terminator);
    }

    private static ExpressionStatement ParseExpression(ref int index, in Token[] tokens)
    {
        var expression = ExpressionParser.Parse(ref index, in tokens);

        if (TypeAt(index, in tokens) != TokenType.Semicolon)
            LanguageSyntax.Expects(
                TokenType.Semicolon,
                tokens[index],
                "Semicolon expected at the end of an expression statement.");

        var terminator = GetAndMoveToNext(ref index, in tokens);

        return new ExpressionStatement(expression, terminator);
    }
}