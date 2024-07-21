using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class UnaryExpression : IExpression
{
    internal const string ExpressionExpected = "Expression expected after unary operator.";

    public Token        Token      { get; }
    public IExpression? Expression { get; }

    internal UnaryExpression(Token token, IExpression? expression)
    {
        Token      = token;
        Expression = expression;
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}