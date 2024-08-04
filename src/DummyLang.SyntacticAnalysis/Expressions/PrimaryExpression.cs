using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class PrimaryExpression : IExpression
{
    public IExpression? Expression { get; }
    public Token        Token      { get; }

    public TokenPositions Positions { get; }

    internal PrimaryExpression(IExpression? expression, Token? token = null)
    {
        Expression = expression;
        Token      = token ?? Token.None;

        Positions = new TokenPositions(expression?.Positions[0], token?.Position);
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}