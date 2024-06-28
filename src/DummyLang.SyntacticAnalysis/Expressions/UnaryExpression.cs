using DummyLang.LexicalAnalysis;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class UnaryExpression : Expression
{
    internal const string AppliedToInvalidToken = "Invalid token next to a unary operator.";

    public Token      Token      { get; }
    public Expression Expression { get; }

    public UnaryExpression(Token token, Expression expression)
    {
        Token      = token;
        Expression = expression;
    }
}