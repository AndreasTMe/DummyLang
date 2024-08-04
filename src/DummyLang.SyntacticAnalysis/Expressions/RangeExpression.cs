using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class RangeExpression : IExpression
{
    public IExpression? FirstInclusive { get; }
    public Token        Operator       { get; }
    public IExpression? LastExclusive  { get; }

    public TokenPositions Positions { get; }

    internal RangeExpression(IExpression? firstInclusive, Token @operator, IExpression? lastExclusive)
    {
        FirstInclusive = firstInclusive;
        Operator       = @operator;
        LastExclusive  = lastExclusive;

        Positions = new TokenPositions(firstInclusive?.Positions[0], @operator.Position, lastExclusive?.Positions[0]);
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}