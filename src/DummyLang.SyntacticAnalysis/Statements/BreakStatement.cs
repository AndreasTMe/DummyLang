using DummyLang.LexicalAnalysis;

namespace DummyLang.SyntacticAnalysis.Statements;

public sealed class BreakStatement : Statement
{
    public Token Break      { get; }
    public Token Label      { get; }
    public Token Terminator { get; }

    internal BreakStatement(Token @break, Token label, Token terminator)
    {
        Break      = @break;
        Label      = label;
        Terminator = terminator;
    }
}