using DummyLang.LexicalAnalysis;
using System;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class InvalidExpression : Expression
{
    public Token[]     Tokens         { get; }
    public Expression? Expression     { get; }
    public Type?       ExpressionType { get; }

    internal InvalidExpression(params Token[] tokens)
    {
        Tokens         = tokens;
        ExpressionType = default;
    }

    internal InvalidExpression(Type expressionType, params Token[] tokens)
    {
        Tokens         = tokens;
        Expression     = default;
        ExpressionType = expressionType;
    }

    internal InvalidExpression(Expression expression, params Token[] tokens)
    {
        Tokens         = tokens;
        Expression     = expression;
        ExpressionType = expression.GetType();
    }
}