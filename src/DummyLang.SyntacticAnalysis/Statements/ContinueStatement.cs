using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;
using DummyLang.SyntacticAnalysis.Statements.Abstractions;

namespace DummyLang.SyntacticAnalysis.Statements;

public sealed class ContinueStatement : IStatement
{
    public Token ContinueKeyword { get; }
    public Token Label           { get; }
    public Token Terminator      { get; }

    internal ContinueStatement(Token continueKeyword, Token label, Token terminator)
    {
        ContinueKeyword = continueKeyword;
        Label           = label;
        Terminator      = terminator;
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}