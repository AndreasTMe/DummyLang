using DummyLang.LexicalAnalysis;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class RangeExpression : Expression
{
    public Expression Start { get; }
    public Token      Operator     { get; }
    public Expression End     { get; }

    internal RangeExpression(Expression start, Token @operator, Expression end)
    {
        Start = start;
        Operator     = @operator;
        End     = end;
    }
}