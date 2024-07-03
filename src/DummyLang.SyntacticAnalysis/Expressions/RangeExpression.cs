using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;
using DummyLang.SyntacticAnalysis.Expressions.Abstractions;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class RangeExpression : IExpression
{
    public IExpression Start    { get; }
    public Token       Operator { get; }
    public IExpression End      { get; }

    internal RangeExpression(IExpression start, Token @operator, IExpression end)
    {
        Start    = start;
        Operator = @operator;
        End      = end;
    }

    public void Accept(ISyntaxNodeVisitor visitor)
    {
        visitor.Visit(this);
        
        Start.Accept(visitor);
        End.Accept(visitor);
    }
}