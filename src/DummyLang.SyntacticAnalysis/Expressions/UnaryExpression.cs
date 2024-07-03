using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;
using DummyLang.SyntacticAnalysis.Expressions.Abstractions;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class UnaryExpression : IExpression
{
    public Token       Token      { get; }
    public IExpression Expression { get; }

    internal UnaryExpression(Token token, IExpression expression)
    {
        Token      = token;
        Expression = expression;
    }

    public void Accept(ISyntaxNodeVisitor visitor)
    {
        visitor.Visit(this);

        Expression.Accept(visitor);
    }
}