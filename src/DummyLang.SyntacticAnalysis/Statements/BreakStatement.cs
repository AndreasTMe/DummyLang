using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;
using DummyLang.SyntacticAnalysis.Statements.Abstractions;

namespace DummyLang.SyntacticAnalysis.Statements;

public sealed class BreakStatement : IStatement
{
    public Token BreakKeyword { get; }
    public Token Label        { get; }
    public Token Terminator   { get; }

    internal BreakStatement(Token breakKeyword, Token label, Token terminator)
    {
        BreakKeyword = breakKeyword;
        Label        = label;
        Terminator   = terminator;
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}