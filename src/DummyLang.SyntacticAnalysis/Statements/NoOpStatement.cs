using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;

namespace DummyLang.SyntacticAnalysis.Statements;

public sealed class NoOpStatement : IStatement
{
    public Token Terminator { get; }

    public TokenPositions Positions { get; }

    internal NoOpStatement(Token terminator)
    {
        Terminator = terminator;

        Positions = new TokenPositions(terminator.Position);
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}