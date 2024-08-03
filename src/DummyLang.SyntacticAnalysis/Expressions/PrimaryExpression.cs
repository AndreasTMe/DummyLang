using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class PrimaryExpression : PositionedNode, IExpression
{
    public TokenPosition Start => _start ??= TokenPosition.GetMin(Expression?.Start, Token.Position);
    public TokenPosition End   => _end ??= TokenPosition.GetMax(Expression?.End, Token.Position);

    public IExpression? Expression { get; }
    public Token        Token      { get; }

    internal PrimaryExpression(IExpression? expression, Token? token = null)
    {
        Expression = expression;
        Token      = token ?? Token.None;
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}