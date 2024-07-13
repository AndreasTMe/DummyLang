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
        return token.Type switch
        {
            TokenType.Semicolon              => new NoOpStatement(GetAndMoveToNext(ref index, in tokens)),
            TokenType.LeftBrace              => ParseBlock(ref index, in tokens),
            TokenType.Var or TokenType.Const => ParseVariableDeclaration(ref index, in tokens),
            // TODO: TokenType.Func => ParseFunctionDeclaration(ref index, in tokens),
            TokenType.If       => ParseIfElse(ref index, in tokens),
            TokenType.Break    => ParseBreak(ref index, in tokens),
            TokenType.While    => ParseWhile(ref index, in tokens),
            TokenType.Continue => ParseContinue(ref index, in tokens),
            TokenType.Return   => ParseReturn(ref index, in tokens),
            _                  => ParseExpression(ref index, in tokens)
        };
    }

    private static Token GetAndMoveToNext(ref int index, in Token[] tokens) =>
        index >= 0 && index < tokens.Length ? tokens[index++] : Token.None;

    private static TokenType TypeAt(int index, in Token[] tokens) =>
        index >= 0 && index < tokens.Length ? tokens[index].Type : TokenType.None;

    private static Token TokenAt(int index, in Token[] tokens) =>
        index >= 0 && index < tokens.Length ? tokens[index] : Token.None;

    private static CompoundStatement ParseBlock(ref int index, in Token[] tokens)
    {
        var               leftBrace  = GetAndMoveToNext(ref index, in tokens);
        List<IStatement>? statements = null;

        var next = TokenAt(index, in tokens);
        if (next.Type != TokenType.RightBrace)
        {
            statements = new List<IStatement>();

            while (next.Type != TokenType.RightBrace)
            {
                statements.Add(Parse(ref index, in tokens));

                next = TokenAt(index, in tokens);
                if (!next.IsNone() && !next.IsEndOfFile())
                    continue;

                break;
            }
        }

        var rightBrace = next.Type == TokenType.RightBrace
            ? GetAndMoveToNext(ref index, in tokens)
            : Token.None;

        return new CompoundStatement(leftBrace, rightBrace, statements);
    }

    private static VariableDeclarationStatement ParseVariableDeclaration(ref int index, in Token[] tokens)
    {
        var declarationKeyword = GetAndMoveToNext(ref index, in tokens);

        IdentifierExpression? identifier = null;
        if (TypeAt(index, in tokens) != TokenType.Identifier)
            identifier = new IdentifierExpression(GetAndMoveToNext(ref index, in tokens));

        var typeAssignmentOperator = Token.None;
        if (TypeAt(index, in tokens) != TokenType.Colon)
            typeAssignmentOperator = GetAndMoveToNext(ref index, in tokens);

        ITypeExpression? typeValue = null;
        if (TypeAt(index, in tokens) != TokenType.Assign)
            typeValue = ExpressionParser.ParseType(ref index, in tokens);

        var          valueAssignmentOperator = Token.None;
        IExpression? valueAssignment         = null;

        if (TypeAt(index, in tokens) == TokenType.Assign)
        {
            valueAssignmentOperator = GetAndMoveToNext(ref index, in tokens);
            valueAssignment         = ExpressionParser.Parse(ref index, in tokens);
        }

        var terminator = Token.None;
        if (TypeAt(index, in tokens) != TokenType.Semicolon)
            terminator = GetAndMoveToNext(ref index, in tokens);

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

        var leftParenthesis = Token.None;
        if (TypeAt(index, in tokens) != TokenType.LeftParenthesis)
            leftParenthesis = GetAndMoveToNext(ref index, in tokens);

        var condition = ExpressionParser.Parse(ref index, in tokens);

        var rightParenthesis = Token.None;
        if (TypeAt(index, in tokens) != TokenType.RightParenthesis)
            rightParenthesis = GetAndMoveToNext(ref index, in tokens);

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
        var elseBlock   = ParseBlock(ref index, in tokens);

        return new IfElseStatement.ElseBlock(elseKeyword, elseBlock);
    }

    private static BreakStatement ParseBreak(ref int index, in Token[] tokens)
    {
        var breakKeyword = GetAndMoveToNext(ref index, in tokens);

        var label = Token.None;
        if (TypeAt(index, in tokens) == TokenType.Identifier)
            label = GetAndMoveToNext(ref index, in tokens);

        var terminator = Token.None;
        if (TypeAt(index, in tokens) != TokenType.Semicolon)
            terminator = GetAndMoveToNext(ref index, in tokens);

        return new BreakStatement(breakKeyword, label, terminator);
    }

    private static WhileStatement ParseWhile(ref int index, in Token[] tokens)
    {
        var whileKeyword = GetAndMoveToNext(ref index, in tokens);

        var label = Token.None;
        if (TypeAt(index, in tokens) == TokenType.Identifier)
            label = GetAndMoveToNext(ref index, in tokens);

        var leftParenthesis = Token.None;
        if (TypeAt(index, in tokens) == TokenType.LeftParenthesis)
            leftParenthesis = GetAndMoveToNext(ref index, in tokens);

        var condition = ExpressionParser.Parse(ref index, in tokens);

        var rightParenthesis = Token.None;
        if (TypeAt(index, in tokens) == TokenType.RightParenthesis)
            rightParenthesis = GetAndMoveToNext(ref index, in tokens);

        return new WhileStatement(
            whileKeyword,
            label,
            leftParenthesis,
            condition,
            rightParenthesis,
            TypeAt(index, in tokens) == TokenType.LeftBrace ? ParseBlock(ref index, in tokens) : null);
    }

    private static ContinueStatement ParseContinue(ref int index, in Token[] tokens)
    {
        var continueKeyword = GetAndMoveToNext(ref index, in tokens);

        var label = Token.None;
        if (TypeAt(index, in tokens) == TokenType.Identifier)
            label = GetAndMoveToNext(ref index, in tokens);

        var terminator = Token.None;
        if (TypeAt(index, in tokens) != TokenType.Semicolon)
            terminator = GetAndMoveToNext(ref index, in tokens);

        return new ContinueStatement(continueKeyword, label, terminator);
    }

    private static ReturnStatement ParseReturn(ref int index, in Token[] tokens)
    {
        var                       returnKeyword   = GetAndMoveToNext(ref index, in tokens);
        List<ArgumentExpression>? returnArguments = null;

        if (TypeAt(index, in tokens) != TokenType.Semicolon)
        {
            var returnArgument = ExpressionParser.Parse(ref index, in tokens);
            var comma = TypeAt(index, in tokens) == TokenType.Comma
                ? GetAndMoveToNext(ref index, in tokens)
                : Token.None;

            returnArguments = new List<ArgumentExpression> { new(returnArgument, comma) };

            while (comma.Type == TokenType.Comma)
            {
                returnArgument = ExpressionParser.Parse(ref index, in tokens);
                comma = TypeAt(index, in tokens) == TokenType.Comma
                    ? GetAndMoveToNext(ref index, in tokens)
                    : Token.None;

                returnArguments.Add(new ArgumentExpression(returnArgument, comma));
            }
        }

        var terminator = Token.None;
        if (TypeAt(index, in tokens) != TokenType.Semicolon)
            terminator = GetAndMoveToNext(ref index, in tokens);

        return new ReturnStatement(returnKeyword, terminator, returnArguments);
    }

    private static ExpressionStatement ParseExpression(ref int index, in Token[] tokens)
    {
        var expression = ExpressionParser.Parse(ref index, in tokens);

        var terminator = Token.None;
        if (TypeAt(index, in tokens) != TokenType.Semicolon)
            terminator = GetAndMoveToNext(ref index, in tokens);

        return new ExpressionStatement(expression, terminator);
    }
}