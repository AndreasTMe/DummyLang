using DummyLang.LexicalAnalysis;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class UnaryExpression : Expression
{
    public Token      Token      { get; }
    public Expression Expression { get; }

    internal UnaryExpression(Token token, Expression expression)
    {
        Token      = token;
        Expression = expression;
    }
}