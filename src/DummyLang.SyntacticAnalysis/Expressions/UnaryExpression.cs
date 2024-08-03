using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class UnaryExpression : PositionedNode, IExpression
{
    internal const string ExpressionExpected = "Expression expected after unary operator.";

    public TokenPosition Start => _start ??= TokenPosition.GetMin(Token.Position, Expression?.Start);
    public TokenPosition End   => _end ??= TokenPosition.GetMax(Token.Position, Expression?.End);

    public Token        Token      { get; }
    public IExpression? Expression { get; }

    internal UnaryExpression(Token token, IExpression? expression)
    {
        Token      = token;
        Expression = expression;
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}