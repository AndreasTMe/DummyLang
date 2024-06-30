using DummyLang.LexicalAnalysis;

namespace DummyLang.SyntacticAnalysis.Statements;

public sealed class NoOpStatement : Statement
{
    public Token Terminator { get; }

    internal NoOpStatement(Token terminator) => Terminator = terminator;
}