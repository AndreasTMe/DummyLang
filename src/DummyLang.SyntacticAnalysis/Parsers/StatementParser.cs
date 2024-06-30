using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Expressions;
using DummyLang.SyntacticAnalysis.Statements;
using DummyLang.SyntacticAnalysis.Utilities;
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
            case TokenType.Semicolon:
                return new NoOpStatement(GetAndMoveToNext(ref index, in tokens));
            default:
                return ParseExpression(ref index, in tokens);
        }
    }

    private static Token GetAndMoveToNext(ref int index, in Token[] tokens) =>
        index >= 0 && index < tokens.Length ? tokens[index++] : Token.None;

    private static TokenType TypeAt(int index, in Token[] tokens) =>
        index >= 0 && index < tokens.Length ? tokens[index].Type : TokenType.None;

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

    private static ExpressionStatement ParseExpression(ref int index, in Token[] tokens)
    {
        var expression = ExpressionParser.Parse(ref index, in tokens);

        if (TypeAt(index, in tokens) != TokenType.Semicolon)
            LanguageSyntax.Expects(
                TokenType.Semicolon,
                tokens[index],
                "Semicolon expected at the end of a variable assignment statement.");

        var terminator = GetAndMoveToNext(ref index, in tokens);

        return new ExpressionStatement(expression, terminator);
    }
}