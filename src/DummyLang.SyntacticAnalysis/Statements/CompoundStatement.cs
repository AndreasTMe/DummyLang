using DummyLang.LexicalAnalysis;

namespace DummyLang.SyntacticAnalysis.Statements;

public sealed class CompoundStatement : Statement
{
    public Token       LeftBrace  { get; }
    public Statement[] Statements { get; }
    public Token       RightBrace { get; }

    internal CompoundStatement(Token leftBrace, Statement[] statements, Token rightBrace)
    {
        LeftBrace  = leftBrace;
        Statements = statements;
        RightBrace = rightBrace;
    }
}