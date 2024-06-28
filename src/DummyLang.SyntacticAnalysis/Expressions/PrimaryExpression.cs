using DummyLang.LexicalAnalysis;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class PrimaryExpression : Expression
{
    public Expression Expression { get; }
    public Token      Token      { get; }

    public PrimaryExpression(Expression expression, Token? token = null)
    {
        Expression = expression;
        Token      = token ?? Token.None;
    }
}