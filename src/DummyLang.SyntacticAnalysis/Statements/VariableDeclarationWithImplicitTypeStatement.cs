using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;

namespace DummyLang.SyntacticAnalysis.Statements;

public sealed class VariableDeclarationWithImplicitTypeStatement : IStatement
{
    internal const string IdentifierExpected     = "Identifier expected.";
    internal const string ColonExpected          = "Colon expected.";
    internal const string AssignmentExpected     = "Assignment expected.";
    internal const string ExpressionExpected     = "Expression expected.";
    internal const string SemicolonExpected      = "Semicolon expected at the end of a variable declaration statement.";

    public Token            DeclarationKeyword { get; }
    public Token            Identifier         { get; }
    public Token            TypeAssignment     { get; }
    public Token            ValueAssignment    { get; }
    public IExpression?     Value              { get; }
    public Token            Terminator         { get; }

    public TokenPositions Positions { get; }

    internal VariableDeclarationWithImplicitTypeStatement(Token declarationKeyword,
                                                          Token identifier,
                                                          Token typeAssignment,
                                                          Token valueAssignment,
                                                          IExpression? value,
                                                          Token terminator)
    {
        DeclarationKeyword = declarationKeyword;
        Identifier         = identifier;
        TypeAssignment     = typeAssignment;
        ValueAssignment    = valueAssignment;
        Value              = value;
        Terminator         = terminator;

        Positions = new TokenPositions(
            declarationKeyword.Position,
            identifier.Position,
            typeAssignment.Position,
            valueAssignment.Position,
            value?.Positions[0],
            terminator.Position);
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}