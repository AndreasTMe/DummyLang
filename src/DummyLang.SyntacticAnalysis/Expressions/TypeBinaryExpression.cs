using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class TypeBinaryExpression : IExpression, ITypeExpression
{
    internal const string RightExpressionMissing = "Expression expected after bitwise operator.";

    public ITypeExpression? Left     { get; }
    public Token            Operator { get; }
    public ITypeExpression? Right    { get; }

    public TokenPositions Positions { get; }

    internal TypeBinaryExpression(ITypeExpression? left, Token @operator, ITypeExpression? right)
    {
        Left     = left;
        Operator = @operator;
        Right    = right;

        Positions = new TokenPositions(left?.Positions[0], @operator.Position, right?.Positions[0]);
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}