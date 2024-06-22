namespace DummyLang.SyntacticAnalysis.Expressions;

public abstract class Expression : SyntaxTree.Node
{
    public virtual bool IsValid => true;
}