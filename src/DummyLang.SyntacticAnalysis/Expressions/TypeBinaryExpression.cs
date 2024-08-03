using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class TypeBinaryExpression : PositionedNode, IExpression, ITypeExpression
{
    internal const string RightExpressionMissing = "Expression expected after bitwise operator.";

    public TokenPosition Start => _start ??= TokenPosition.GetMin(Left?.Start, Operator.Position, Right?.Start);
    public TokenPosition End   => _end ??= TokenPosition.GetMax(Left?.End, Operator.Position, Right?.Start);

    public ITypeExpression? Left     { get; }
    public Token            Operator { get; }
    public ITypeExpression? Right    { get; }

    internal TypeBinaryExpression(ITypeExpression? left, Token @operator, ITypeExpression? right)
    {
        Left     = left;
        Operator = @operator;
        Right    = right;
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}