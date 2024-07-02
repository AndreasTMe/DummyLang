using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Expressions.Abstractions;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class PrimaryExpression : IExpression
{
    public IExpression Expression { get; }
    public Token      Token      { get; }

    internal PrimaryExpression(IExpression expression, Token? token = null)
    {
        Expression = expression;
        Token      = token ?? Token.None;
    }
}