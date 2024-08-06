using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;
using DummyLang.SyntacticAnalysis.Expressions;
using System.Collections.Generic;

namespace DummyLang.SyntacticAnalysis.Statements;

public sealed class FunctionDeclarationStatement : IStatement
{
    internal const string InvalidToken              = "Invalid token after 'func' keyword.";
    internal const string TypeAssignmentExpected    = "Type assignment expected.";
    internal const string TypeExpressionExpected    = "Type expression expected.";
    internal const string CompoundStatementExpected = "Compound statement expected. Check for missing left brace.";

    public Token                               FuncKeyword      { get; }
    public Token                               Identifier       { get; }
    public Token                               TypeAssignment   { get; }
    public ITypeExpression?                    Type             { get; }
    public Token                               ValueAssignment  { get; }
    public Token                               LeftParenthesis  { get; }
    public IReadOnlyList<ParameterExpression>? Parameters       { get; }
    public Token                               RightParenthesis { get; }
    public Token                               LambdaAssign     { get; }
    public CompoundStatement?                  Block            { get; }

    public TokenPositions Positions { get; }

    internal FunctionDeclarationStatement(Token funcKeyword,
                                          Token identifier,
                                          Token typeAssignment,
                                          ITypeExpression? type,
                                          Token valueAssignment,
                                          Token leftParenthesis,
                                          List<ParameterExpression>? parameters,
                                          Token rightParenthesis,
                                          Token lambdaAssign,
                                          CompoundStatement? block)
    {
        FuncKeyword      = funcKeyword;
        Identifier       = identifier;
        TypeAssignment   = typeAssignment;
        Type             = type;
        ValueAssignment  = valueAssignment;
        LeftParenthesis  = leftParenthesis;
        Parameters       = parameters is { Count: > 0 } ? parameters : null;
        RightParenthesis = rightParenthesis;
        LambdaAssign     = lambdaAssign;
        Block            = block;

        Positions = new TokenPositions(
            funcKeyword.Position,
            identifier.Position,
            typeAssignment.Position,
            type?.Positions[0],
            valueAssignment.Position,
            leftParenthesis.Position,
            parameters?[0].Positions[0],
            rightParenthesis.Position,
            lambdaAssign.Position,
            block?.Positions[0]);
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}