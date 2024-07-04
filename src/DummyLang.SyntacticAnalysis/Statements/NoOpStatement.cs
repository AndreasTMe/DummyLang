using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;

namespace DummyLang.SyntacticAnalysis.Statements;

public sealed class NoOpStatement : IStatement
{
    public Token Terminator { get; }

    internal NoOpStatement(Token terminator) => Terminator = terminator;

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}