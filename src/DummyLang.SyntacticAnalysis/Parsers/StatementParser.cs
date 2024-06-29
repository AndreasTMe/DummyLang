using DummyLang.LexicalAnalysis;
using DummyLang.LexicalAnalysis.Extensions;
using DummyLang.SyntacticAnalysis.Expressions;
using DummyLang.SyntacticAnalysis.Statements;
using System.ComponentModel;
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
            case TokenType.Var:
            case TokenType.Const:
                return ParseVariableDeclaration(ref index, in tokens);
            default:
                throw new InvalidEnumArgumentException(nameof(tokenType));
        }
    }

    private static Token GetAndMoveToNext(ref int index, in Token[] tokens) =>
        index >= 0 && index < tokens.Length ? tokens[index++] : Token.None;

    private static Token LookAt(int index, in Token[] tokens) =>
        index >= 0 && index < tokens.Length ? tokens[index] : Token.None;

    private static VariableDeclarationStatement ParseVariableDeclaration(ref int index, in Token[] tokens)
    {
        //  TODO: Handle type with more than one token, e.g. generic, discriminated union, etc.

        var         declarationKeyword = GetAndMoveToNext(ref index, in tokens);
        Expression  identifier;
        var         typeAssignmentOperator  = Token.None;
        Expression? typeValue               = default;
        var         valueAssignmentOperator = Token.None;
        Expression? valueAssignment         = default;
        var         terminator              = Token.None;

        Token lookAhead;
        var   lastIndex = index;
        var   colonIdx  = -1;
        var   assignIdx = -1;
        while ((lookAhead = LookAt(lastIndex, in tokens)).Type != TokenType.Eof)
        {
            if (lookAhead.Type == TokenType.Semicolon || lookAhead.IsReservedKeyword())
                break;

            if (colonIdx < 0 && lookAhead.Type == TokenType.Colon)
                colonIdx = lastIndex;
            else if (assignIdx < 0 && lookAhead.Type == TokenType.Assign)
                assignIdx = lastIndex;

            lastIndex++;
        }

        // ReSharper disable once ConvertIfStatementToSwitchStatement
        if (colonIdx >= 0 && assignIdx >= 0 && colonIdx > assignIdx)
        {
            if (lookAhead.Type == TokenType.Semicolon)
            {
                terminator = lookAhead;
                index      = lastIndex + 1;
            }
            else
            {
                index = lastIndex;
            }

            return new VariableDeclarationStatement(
                declarationKeyword,
                new InvalidExpression(
                    Token.ExpectedAt(tokens[assignIdx].Position, TokenType.Colon),
                    Token.ExpectedAt(tokens[colonIdx].Position, TokenType.Assign)),
                typeAssignmentOperator,
                typeValue,
                valueAssignmentOperator,
                valueAssignment,
                terminator);
        }

        if (colonIdx >= 0 && assignIdx >= 0)
        {
            identifier = index == colonIdx - 1
                ? new IdentifierExpression(tokens[colonIdx - 1])
                : new InvalidExpression(typeof(IdentifierExpression), tokens[index..colonIdx]);

            typeAssignmentOperator = colonIdx < tokens.Length ? tokens[colonIdx] : Token.None;

            switch (assignIdx - colonIdx)
            {
                case > 1:
                    typeValue = new TypeIdentifierExpression(tokens[(colonIdx + 1)..assignIdx]);
                    break;
                case 1:
                    typeValue = default;
                    break;
                default:
                {
                    if (lastIndex - colonIdx > 1)
                        typeValue = new TypeIdentifierExpression(tokens[(colonIdx + 1)..lastIndex]);
                    break;
                }
            }

            valueAssignmentOperator = assignIdx < tokens.Length ? tokens[assignIdx] : Token.None;

            var valueIndex = assignIdx + 1;
            if (valueIndex < tokens.Length && tokens[valueIndex].Type != TokenType.Semicolon)
                valueAssignment = ExpressionParser.Parse(ref valueIndex, tokens[..lastIndex]);

            if (lastIndex == valueIndex)
                terminator = lookAhead.Type == TokenType.Semicolon ? lookAhead : Token.None;
            else
                lastIndex = valueIndex;
        }
        else if (colonIdx < 0 && assignIdx >= 0)
        {
            identifier = index == assignIdx - 1
                ? new IdentifierExpression(tokens[assignIdx - 1])
                : new InvalidExpression(typeof(IdentifierExpression), tokens[index..assignIdx]);

            typeAssignmentOperator = Token.None;
            typeValue = new InvalidExpression(Token.ExpectedAt(tokens[assignIdx - 1].Position, TokenType.Colon));

            valueAssignmentOperator = assignIdx < tokens.Length ? tokens[assignIdx] : Token.None;

            var valueIndex = assignIdx + 1;
            if (valueIndex < tokens.Length && tokens[valueIndex].Type != TokenType.Semicolon)
                valueAssignment = ExpressionParser.Parse(ref valueIndex, tokens[..lastIndex]);

            if (lastIndex == valueIndex)
                terminator = lookAhead.Type == TokenType.Semicolon ? lookAhead : Token.None;
            else
                lastIndex = valueIndex;
        }
        else if (colonIdx >= 0 && assignIdx < 0)
        {
            identifier = index == colonIdx - 1
                ? new IdentifierExpression(tokens[colonIdx - 1])
                : new InvalidExpression(typeof(IdentifierExpression), tokens[index..colonIdx]);

            typeAssignmentOperator = colonIdx < tokens.Length ? tokens[colonIdx] : Token.None;

            var typeValueIndex = colonIdx + 1;
            typeValue = colonIdx + 1 == lastIndex - 1
                ? new TypeIdentifierExpression(tokens[lastIndex - 1])
                : new InvalidExpression(ExpressionParser.Parse(ref typeValueIndex, tokens[..lastIndex]));

            terminator = lookAhead.Type == TokenType.Semicolon ? lookAhead : Token.None;
        }
        else
        {
            identifier = new InvalidExpression(tokens[index..lastIndex]);
            terminator = lookAhead.Type == TokenType.Semicolon ? lookAhead : Token.None;
        }

        index = terminator.Type == TokenType.Semicolon ? lastIndex + 1 : lastIndex;

        return new VariableDeclarationStatement(
            declarationKeyword,
            identifier,
            typeAssignmentOperator,
            typeValue,
            valueAssignmentOperator,
            valueAssignment,
            terminator);
    }
}