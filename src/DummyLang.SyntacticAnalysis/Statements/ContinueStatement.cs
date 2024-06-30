using DummyLang.LexicalAnalysis;

namespace DummyLang.SyntacticAnalysis.Statements;

public sealed class ContinueStatement : Statement
{
    public Token Continue   { get; }
    public Token Label      { get; }
    public Token Terminator { get; }

    internal ContinueStatement(Token @continue, Token label, Token terminator)
    {
        Continue   = @continue;
        Label      = label;
        Terminator = terminator;
    }
}