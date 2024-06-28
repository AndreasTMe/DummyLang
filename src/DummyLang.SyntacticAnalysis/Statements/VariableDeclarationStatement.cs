using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Expressions;

namespace DummyLang.SyntacticAnalysis.Statements;

/*
 * With type info:
 *
 * var param: int;
 * var param: int = 1 + 2;
 * const param: int;
 * const param: int = 1 + 2;
 *
 * Without type info:
 *
 * var param := 1 + 2;
 * const param := 1 + 2;
 */
public sealed class VariableDeclarationStatement : Statement
{
    public Token       Declaration     { get; }
    public Expression? Identifier      { get; }
    public Token       TypeAssignment  { get; }
    public Expression? Type            { get; }
    public Token       ValueAssignment { get; }
    public Expression? Value           { get; }
    public Token       Terminator      { get; }

    public VariableDeclarationStatement(Token declaration,
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