using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;
using DummyLang.SyntacticAnalysis.Expressions;
using System.Collections.Generic;

namespace DummyLang.SyntacticAnalysis.Statements;

public sealed class FunctionDeclarationWithImplicitTypeStatement : IStatement
{
    internal const string InvalidToken              = "Invalid token after 'func' keyword.";
    internal const string TypeAssignmentExpected    = "Type assignment expected.";
    internal const string CompoundStatementExpected = "Compound statement expected. Check for missing left brace.";
    internal const string LastArgumentHasComma      = "Last argument should not be followed by comma.";
    internal const string CommaExpected             = "Comma expected.";
    internal const string TypeExpressionExpected    = "Type expression expected.";
    internal const string IdentifierExpected        = "Identifier expected";
    internal const string ColonExpected             = "Colon expected";

    public Token                                   FuncKeyword      { get; }
    public Token                                   Identifier       { get; }
    public Token                                   TypeAssignment   { get; }
    public Token                                   ValueAssignment  { get; }
    public Token                                   LeftParenthesis  { get; }
    public IReadOnlyList<TypeParameterExpression>? TypedParameters  { get; }
    public Token                                   RightParenthesis { get; }
    public Token                                   LambdaAssign     { get; }
    public CompoundStatement?                      Block            { get; }

    public TokenPositions Positions { get; }

    internal FunctionDeclarationWithImplicitTypeStatement(Token funcKeyword,
                                                          Token identifier,
                                                          Token typeAssignment,
                                                          Token valueAssignment,
                                                          Token leftParenthesis,
                                                          List<TypeParameterExpression>? typedParameters,
                                                          Token rightParenthesis,
                                                          Token lambdaAssign,
                                                          CompoundStatement? block)
    {
        FuncKeyword      = funcKeyword;
        Identifier       = identifier;
        TypeAssignment   = typeAssignment;
        ValueAssignment  = valueAssignment;
        LeftParenthesis  = leftParenthesis;
        TypedParameters  = typedParameters is { Count: > 0 } ? typedParameters : null;
        RightParenthesis = rightParenthesis;
        LambdaAssign     = lambdaAssign;
        Block            = block;

        Positions = new TokenPositions(
            funcKeyword.Position,
            identifier.Position,
            typeAssignment.Position,
            valueAssignment.Position,
            leftParenthesis.Position,
            typedParameters?[0].Positions[0],
            rightParenthesis.Position,
            lambdaAssign.Position,
            block?.Positions[0]);
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}