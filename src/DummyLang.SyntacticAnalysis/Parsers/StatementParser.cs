using DummyLang.LexicalAnalysis;
using DummyLang.LexicalAnalysis.Extensions;
using DummyLang.SyntacticAnalysis.Abstractions;
using DummyLang.SyntacticAnalysis.Expressions;
using DummyLang.SyntacticAnalysis.Statements;
using System.Collections.Generic;
using System.Diagnostics;

namespace DummyLang.SyntacticAnalysis.Parsers;

internal static class StatementParser
{
    public static IStatement Parse(ref int index, in Token[] tokens)
    {
        Debug.Assert(index < tokens.Length);

        var token = Parser.TokenAt(index, in tokens);
        return token.Type switch
        {
            TokenType.Semicolon              => new NoOpStatement(Parser.GetAndMoveToNext(ref index, in tokens)),
            TokenType.LeftBrace              => ParseBlock(ref index, in tokens),
            TokenType.Var or TokenType.Const => ParseVariableDeclaration(ref index, in tokens),
            TokenType.Func                   => ParseFunctionDeclaration(ref index, in tokens),
            TokenType.If or TokenType.Else   => ParseIfElse(ref index, in tokens),
            TokenType.Break                  => ParseBreak(ref index, in tokens),
            TokenType.While                  => ParseWhile(ref index, in tokens),
            TokenType.Continue               => ParseContinue(ref index, in tokens),
            TokenType.Return                 => ParseReturn(ref index, in tokens),
            _                                => ParseExpression(ref index, in tokens)
        };
    }

    private static CompoundStatement ParseBlock(ref int index, in Token[] tokens)
    {
        var               leftBrace  = Parser.GetAndMoveToNext(ref index, in tokens);
        List<IStatement>? statements = null;

        var next = Parser.TokenAt(index, in tokens);
        if (next.Type != TokenType.RightBrace)
        {
            statements = new List<IStatement>();

            while (next.Type != TokenType.RightBrace)
            {
                statements.Add(Parse(ref index, in tokens));

                next = Parser.TokenAt(index, in tokens);
                if (!next.IsNone() && !next.IsEndOfFile())
                    continue;

                break;
            }
        }

        var rightBrace = Parser.GetAndMoveToNextOrDefault(TokenType.RightBrace, ref index, in tokens);

        return new CompoundStatement(leftBrace, rightBrace, statements);
    }

    private static VariableDeclarationStatement ParseVariableDeclaration(ref int index, in Token[] tokens)
    {
        var declarationKeyword = Parser.GetAndMoveToNext(ref index, in tokens);

        var identifier             = Parser.GetAndMoveToNextOrDefault(TokenType.Identifier, ref index, in tokens);
        var typeAssignmentOperator = Parser.GetAndMoveToNextOrDefault(TokenType.Colon, ref index, in tokens);

        ITypeExpression? typeValue = null;
        if (Parser.TypeAt(index, in tokens) != TokenType.Assign)
            typeValue = ExpressionParser.ParseType(ref index, in tokens);

        var          valueAssignmentOperator = Token.None;
        IExpression? valueAssignment         = null;

        if (typeAssignmentOperator.Type == TokenType.Colon && Parser.TypeAt(index, in tokens) == TokenType.Assign)
        {
            valueAssignmentOperator = Parser.GetAndMoveToNext(ref index, in tokens);
            valueAssignment         = ExpressionParser.Parse(ref index, in tokens);
        }

        var terminator = Parser.GetAndMoveToNextOrDefault(TokenType.Semicolon, ref index, in tokens);

        return new VariableDeclarationStatement(
            declarationKeyword,
            identifier,
            typeAssignmentOperator,
            typeValue,
            valueAssignmentOperator,
            valueAssignment,
            terminator);
    }

    private static FunctionDeclarationStatement ParseFunctionDeclaration(ref int index, in Token[] tokens)
    {
        var funcKeyword = Parser.GetAndMoveToNext(ref index, in tokens);

        var identifier             = Parser.GetAndMoveToNextOrDefault(TokenType.Identifier, ref index, in tokens);
        var typeAssignmentOperator = Parser.GetAndMoveToNextOrDefault(TokenType.Colon, ref index, in tokens);

        ITypeExpression? typeValue = null;
        if (Parser.TypeAt(index, in tokens) == TokenType.LeftParenthesis)
            typeValue = ExpressionParser.ParseType(ref index, in tokens);

        var valueAssignmentOperator = Parser.GetAndMoveToNextOrDefault(TokenType.Assign, ref index, in tokens);
        var leftParenthesis         = Parser.GetAndMoveToNextOrDefault(TokenType.LeftParenthesis, ref index, in tokens);

        List<ParameterExpression>? parameters = null;
        if (Parser.TypeAt(index, in tokens) != TokenType.RightParenthesis)
        {
            var parameter = Parser.GetAndMoveToNextOrDefault(TokenType.Identifier, ref index, in tokens);
            var comma     = Parser.GetAndMoveToNextOrDefault(TokenType.Comma, ref index, in tokens);

            parameters = new List<ParameterExpression> { new(parameter, comma) };

            while (comma.Type == TokenType.Comma)
            {
                parameter = Parser.GetAndMoveToNextOrDefault(TokenType.Identifier, ref index, in tokens);
                comma     = Parser.GetAndMoveToNextOrDefault(TokenType.Comma, ref index, in tokens);

                parameters.Add(new ParameterExpression(parameter, comma));
            }
        }

        var rightParenthesis = Parser.GetAndMoveToNextOrDefault(TokenType.RightParenthesis, ref index, in tokens);
        var lambdaAssign     = Parser.GetAndMoveToNextOrDefault(TokenType.LambdaAssign, ref index, in tokens);

        CompoundStatement? functionBlock = null;
        if (Parser.TypeAt(index, in tokens) == TokenType.LeftBrace)
            functionBlock = ParseBlock(ref index, in tokens);

        return new FunctionDeclarationStatement(
            funcKeyword,
            identifier,
            typeAssignmentOperator,
            typeValue,
            valueAssignmentOperator,
            leftParenthesis,
            parameters,
            rightParenthesis,
            lambdaAssign,
            functionBlock);
    }

    private static IfElseStatement ParseIfElse(ref int index, in Token[] tokens)
    {
        IfElseStatement.IfBlock? ifPart = null;
        if (Parser.TypeAt(index, in tokens) == TokenType.If)
            ifPart = ParseIfPart(ref index, in tokens);

        var elseIfParts = new List<IfElseStatement.ElseIfBlock>();
        while (Parser.TypeAt(index, in tokens) == TokenType.Else && Parser.TypeAt(index + 1, in tokens) == TokenType.If)
            elseIfParts.Add(ParseElseIfPart(ref index, in tokens));

        return Parser.TypeAt(index, in tokens) == TokenType.Else
            ? new IfElseStatement(ifPart, elseIfParts, ParseElsePart(ref index, in tokens))
            : new IfElseStatement(ifPart, elseIfParts, null);
    }

    private static IfElseStatement.IfBlock ParseIfPart(ref int index, in Token[] tokens)
    {
        var ifKeyword = Parser.GetAndMoveToNext(ref index, in tokens);

        var leftParenthesis = Parser.GetAndMoveToNextOrDefault(TokenType.LeftParenthesis, ref index, in tokens);

        IExpression? condition = null;
        if (Parser.TypeAt(index, in tokens) != TokenType.RightParenthesis)
            condition = ExpressionParser.Parse(ref index, in tokens);

        var rightParenthesis = Parser.GetAndMoveToNextOrDefault(TokenType.RightParenthesis, ref index, in tokens);

        CompoundStatement? ifBlock = null;
        if (Parser.TypeAt(index, in tokens) == TokenType.LeftBrace)
            ifBlock = ParseBlock(ref index, in tokens);

        return new IfElseStatement.IfBlock(ifKeyword, leftParenthesis, condition, rightParenthesis, ifBlock);
    }

    private static IfElseStatement.ElseIfBlock ParseElseIfPart(ref int index, in Token[] tokens)
    {
        var elseKeyword = Parser.GetAndMoveToNext(ref index, in tokens);
        var ifPart      = ParseIfPart(ref index, in tokens);

        return new IfElseStatement.ElseIfBlock(elseKeyword, ifPart);
    }

    private static IfElseStatement.ElseBlock ParseElsePart(ref int index, in Token[] tokens)
    {
        var elseKeyword = Parser.GetAndMoveToNext(ref index, in tokens);

        CompoundStatement? elseBlock = null;
        if (Parser.TypeAt(index, in tokens) == TokenType.LeftBrace)
            elseBlock = ParseBlock(ref index, in tokens);

        return new IfElseStatement.ElseBlock(elseKeyword, elseBlock);
    }

    private static BreakStatement ParseBreak(ref int index, in Token[] tokens)
    {
        var breakKeyword = Parser.GetAndMoveToNext(ref index, in tokens);
        var label        = Parser.GetAndMoveToNextOrDefault(TokenType.Identifier, ref index, in tokens);
        var terminator   = Parser.GetAndMoveToNextOrDefault(TokenType.Semicolon, ref index, in tokens);

        return new BreakStatement(breakKeyword, label, terminator);
    }

    private static WhileStatement ParseWhile(ref int index, in Token[] tokens)
    {
        var whileKeyword = Parser.GetAndMoveToNext(ref index, in tokens);

        var label           = Parser.GetAndMoveToNextOrDefault(TokenType.Identifier, ref index, in tokens);
        var leftParenthesis = Parser.GetAndMoveToNextOrDefault(TokenType.LeftParenthesis, ref index, in tokens);

        IExpression? condition = null;
        if (Parser.TypeAt(index, in tokens) != TokenType.RightParenthesis)
            condition = ExpressionParser.Parse(ref index, in tokens);

        var rightParenthesis = Parser.GetAndMoveToNextOrDefault(TokenType.RightParenthesis, ref index, in tokens);

        return new WhileStatement(
            whileKeyword,
            label,
            leftParenthesis,
            condition,
            rightParenthesis,
            Parser.TypeAt(index, in tokens) == TokenType.LeftBrace ? ParseBlock(ref index, in tokens) : null);
    }

    private static ContinueStatement ParseContinue(ref int index, in Token[] tokens)
    {
        var continueKeyword = Parser.GetAndMoveToNext(ref index, in tokens);

        var label = Token.None;
        if (Parser.TypeAt(index, in tokens) == TokenType.Identifier)
            label = Parser.GetAndMoveToNext(ref index, in tokens);

        var terminator = Parser.GetAndMoveToNextOrDefault(TokenType.Semicolon, ref index, in tokens);

        return new ContinueStatement(continueKeyword, label, terminator);
    }

    private static ReturnStatement ParseReturn(ref int index, in Token[] tokens)
    {
        var                       returnKeyword   = Parser.GetAndMoveToNext(ref index, in tokens);
        List<ArgumentExpression>? returnArguments = null;

        if (Parser.TypeAt(index, in tokens) != TokenType.Semicolon)
        {
            var returnArgument = ExpressionParser.Parse(ref index, in tokens);
            var comma          = Parser.GetAndMoveToNextOrDefault(TokenType.Comma, ref index, in tokens);

            returnArguments = new List<ArgumentExpression> { new(returnArgument, comma) };

            while (comma.Type == TokenType.Comma)
            {
                returnArgument = ExpressionParser.Parse(ref index, in tokens);
                comma          = Parser.GetAndMoveToNextOrDefault(TokenType.Comma, ref index, in tokens);

                returnArguments.Add(new ArgumentExpression(returnArgument, comma));
            }
        }

        var terminator = Parser.GetAndMoveToNextOrDefault(TokenType.Semicolon, ref index, in tokens);

        return new ReturnStatement(returnKeyword, terminator, returnArguments);
    }

    private static ExpressionStatement ParseExpression(ref int index, in Token[] tokens)
    {
        var expression = ExpressionParser.Parse(ref index, in tokens);
        var terminator = Parser.GetAndMoveToNextOrDefault(TokenType.Semicolon, ref index, in tokens);

        return new ExpressionStatement(expression, terminator);
    }
}