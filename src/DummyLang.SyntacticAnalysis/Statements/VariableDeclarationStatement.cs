using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Expressions;

namespace DummyLang.SyntacticAnalysis.Statements;

public sealed class VariableDeclarationStatement : Statement
{
    public Token       Declaration     { get; }
    public Expression? Identifier      { get; }
    public Token       TypeAssignment  { get; }
    public Expression? Type            { get; }
    public Token       ValueAssignment { get; }
    public Expression? Value           { get; }
    public Token       Terminator      { get; }

    internal VariableDeclarationStatement(Token declaration,
                                          Expression? identifier,
                                          Token typeAssignment,
                                          Expression? type,
                                          Token valueAssignment,
                                          Expression? value,
                                          Token terminator)
    {
        Declaration     = declaration;
        Identifier      = identifier;
        TypeAssignment  = typeAssignment;
        Type            = type;
        ValueAssignment = valueAssignment;
        Value           = value;
        Terminator      = terminator;
    }
}