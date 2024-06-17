using DummyLang.SyntacticAnalysis.Utilities;

namespace DummyLang.SyntacticAnalysis.Expressions;

public abstract class LiteralExpression : Expression
{
    public sealed override SyntaxNodeType Type => SyntaxNodeType.LiteralExpression;
}