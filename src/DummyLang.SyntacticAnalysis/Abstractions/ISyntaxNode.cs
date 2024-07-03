namespace DummyLang.SyntacticAnalysis.Abstractions;

public interface ISyntaxNode
{
    void Accept(ISyntaxNodeVisitor visitor);
}