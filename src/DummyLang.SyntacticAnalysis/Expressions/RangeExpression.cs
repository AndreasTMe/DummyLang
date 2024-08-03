using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class RangeExpression : PositionedNode, IExpression
{
    public TokenPosition Start =>
        _start ??= TokenPosition.GetMin(FirstInclusive?.Start, Operator.Position, LastExclusive?.Start);

    public TokenPosition End =>
        _end ??= TokenPosition.GetMax(FirstInclusive?.End, Operator.Position, LastExclusive?.End);

    public IExpression? FirstInclusive { get; }
    public Token        Operator       { get; }
    public IExpression? LastExclusive  { get; }

    internal RangeExpression(IExpression? firstInclusive, Token @operator, IExpression? lastExclusive)
    {
        FirstInclusive = firstInclusive;
        Operator       = @operator;
        LastExclusive  = lastExclusive;
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}