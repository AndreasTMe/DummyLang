using DummyLang.LexicalAnalysis;

namespace DummyLang.SyntacticAnalysis.Abstractions;

public interface ISyntaxNode : INode
{
    void Accept(ISyntaxNodeVisitor visitor);
}

public interface ISyntaxNodeWithPositions : INode
{
    TokenPositions Positions { get; }

    public TokenPosition FirstTokenPosition() => Positions[0];

    public TokenPosition LastTokenPosition() => Positions[^1, false];
}