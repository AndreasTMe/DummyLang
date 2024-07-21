using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;
using DummyLang.SyntacticAnalysis.Expressions;
using System.Collections.Generic;

namespace DummyLang.SyntacticAnalysis.Statements;

public sealed class FunctionDeclarationStatement : IStatement
{
    public Token                                   DeclarationKeyword { get; }
    public IExpression?                            Identifier         { get; }
    public Token                                   TypeAssignment     { get; }
    public Token                                   LeftParenthesis    { get; }
    public IReadOnlyList<TypeParameterExpression>? Input              { get; }
    public Token                                   RightParenthesis   { get; }
    public Token                                   LambdaAssign       { get; }
    public IReadOnlyList<TypeArgumentExpression>?  Output             { get; }
    public CompoundStatement?                      Block              { get; }

    internal FunctionDeclarationStatement(Token declarationKeyword,
                                          IExpression? identifier,
                                          Token typeAssignment,
                                          Token leftParenthesis,
                                          IReadOnlyList<TypeParameterExpression>? input,
                                          Token rightParenthesis,
                                          Token lambdaAssign,
                                          IReadOnlyList<TypeArgumentExpression>? output,
                                          CompoundStatement? block)
    {
        DeclarationKeyword = declarationKeyword;
        Identifier         = identifier;
        TypeAssignment     = typeAssignment;
        LeftParenthesis    = leftParenthesis;
        Input              = input is { Count: > 0 } ? input : null;
        RightParenthesis   = rightParenthesis;
        LambdaAssign       = lambdaAssign;
        Output             = output is { Count: > 0 } ? output : null;
        Block              = block;
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}