namespace DummyLang.SyntacticAnalysis.Abstractions;

public interface ISyntaxNodeVisitor
{
    // TODO: For now, just to cover errors. One "Visit" method per syntax node type.
    void Visit<TNode>(TNode syntaxNode)
        where TNode : class, ISyntaxNode;
}