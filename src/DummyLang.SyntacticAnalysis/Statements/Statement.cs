namespace DummyLang.SyntacticAnalysis.Statements;

public abstract class Statement : SyntaxTree.Node
{
    public virtual void Evaluate(/* TODO: Use the visitor pattern */)
    {
    }
}