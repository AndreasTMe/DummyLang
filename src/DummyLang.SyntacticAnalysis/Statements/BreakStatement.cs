using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;

namespace DummyLang.SyntacticAnalysis.Statements;

public sealed class BreakStatement : IStatement
{
    internal const string InvalidToken      = "Invalid token after 'break' keyword.";
    internal const string SemicolonExpected = "Semicolon expected at the end of a 'break' statement.";

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