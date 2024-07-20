namespace DummyLang.SyntacticAnalysis.Abstractions;

public interface ISyntaxNode : INode
{
    void Accept(ISyntaxNodeVisitor visitor);
}