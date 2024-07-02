using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Expressions.Abstractions;
using DummyLang.SyntacticAnalysis.Statements.Abstractions;

namespace DummyLang.SyntacticAnalysis.Statements;

public sealed class VariableDeclarationStatement : Statement
{
    public Token            DeclarationKeyword { get; }
    public IExpression?     Identifier         { get; }
    public Token            TypeAssignment     { get; }
    public ITypeExpression? Type               { get; }
    public Token            ValueAssignment    { get; }
    public IExpression?     Value              { get; }
    public Token            Terminator         { get; }

    internal VariableDeclarationStatement(Token declarationKeyword,
                                          IExpression? identifier,
                                          Token typeAssignment,
                                          ITypeExpression? type,
                                          Token valueAssignment,
                                          IExpression? value,
                                          Token terminator)
    {
        DeclarationKeyword = declarationKeyword;
        Identifier         = identifier;
        TypeAssignment     = typeAssignment;
        Type               = type;
        ValueAssignment    = valueAssignment;
        Value              = value;
        Terminator         = terminator;
    }
}