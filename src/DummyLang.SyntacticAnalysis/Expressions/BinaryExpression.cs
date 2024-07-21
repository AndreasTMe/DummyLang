using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class BinaryExpression : IExpression
{
    internal const string RightExpressionMissing = "Expression expected after binary operator.";

    public IExpression? Left     { get; }
    public Token        Operator { get; }
    public IExpression? Right    { get; }

    internal BinaryExpression(IExpression? left, Token @operator, IExpression? right)
    {
        Left     = left;
        Operator = @operator;
        Right    = right;
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}