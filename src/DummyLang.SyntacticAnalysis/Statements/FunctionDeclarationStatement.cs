using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;

namespace DummyLang.SyntacticAnalysis.Statements;

public sealed class FunctionDeclarationStatement : IStatement
{
    internal const string InvalidToken              = "Invalid token after 'func' keyword.";
    internal const string TypeAssignmentExpected    = "Type assignment expected.";
    internal const string TypeExpressionExpected    = "Type expression expected.";
    internal const string CompoundStatementExpected = "Compound statement expected. Check for missing left brace.";

    public Token              FuncKeyword    { get; }
    public Token              Identifier     { get; }
    public Token              TypeAssignment { get; }
    public ITypeExpression?   Type           { get; }
    public CompoundStatement? Block          { get; }

    internal FunctionDeclarationStatement(Token funcKeyword,
                                          Token identifier,
                                          Token typeAssignment,
                                          ITypeExpression? type,
                                          CompoundStatement? block)
    {
        FuncKeyword    = funcKeyword;
        Identifier     = identifier;
        TypeAssignment = typeAssignment;
        Type           = type;
        Block          = block;
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}