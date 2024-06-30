using DummyLang.LexicalAnalysis;
using DummyLang.LexicalAnalysis.Extensions;
using DummyLang.SyntacticAnalysis.Expressions;
using DummyLang.SyntacticAnalysis.Statements;
using DummyLang.SyntacticAnalysis.Utilities;
using System.Collections.Generic;
using System.Diagnostics;

namespace DummyLang.SyntacticAnalysis.Parsers;

internal static class StatementParser
{
    public static Statement Parse(ref int index, in Token[] tokens)
    {
        Debug.Assert(index < tokens.Length);

        var tokenType = tokens[index].Type;
        switch (tokenType)
        {
            case TokenType.Semicolon:
                return new NoOpStatement(GetAndMoveToNext(ref index, in tokens));
            case TokenType.LeftBrace:
                return ParseBlock(ref index, in tokens);
            case TokenType.Var:
            case TokenType.Const:
                return ParseVariableDeclaration(ref index, in tokens);
            // TODO: case TokenType.Func:
            // TODO: case TokenType.If:
            // TODO: case TokenType.Else:
            case TokenType.Break:
                return ParseBreak(ref index, in tokens);
            // TODO: case TokenType.While:
            case TokenType.Continue:
                return ParseContinue(ref index, in tokens);
            case TokenType.Return:
                return ParseReturn(ref index, in tokens);
            default:
                return ParseExpression(ref index, in tokens);
        }
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

        var statements  = new List<Statement>();
        var next        = TokenAt(index, in tokens);
        var shouldThrow = next.IsInvalid() || next.IsEndOfFile();
        while (next.Type != TokenType.RightBrace && !shouldThrow)
        {
            statements.Add(Parse(ref index, in tokens));

            next = TokenAt(index, in tokens);
            if (!next.IsInvalid() && !next.IsEndOfFile())
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

        var         typeAssignmentOperator = GetAndMoveToNext(ref index, in tokens);
        Expression? typeValue              = default;

        //  TODO: Handle type with more than one token, e.g. generic, discriminated union, etc.
        if (TypeAt(index, in tokens) != TokenType.Assign)
            typeValue = new TypeIdentifierExpression(GetAndMoveToNext(ref index, in tokens));

        var         valueAssignmentOperator = Token.None;
        Expression? valueAssignment         = default;

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

        var expressions = new List<Expression>();
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