using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Statements.Abstractions;

namespace DummyLang.SyntacticAnalysis.Statements;

public sealed class NoOpStatement : Statement
{
    public Token Terminator { get; }

    internal NoOpStatement(Token terminator) => Terminator = terminator;
}