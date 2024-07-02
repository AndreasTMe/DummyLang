using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Expressions.Abstractions;
using System;
using System.Diagnostics;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class InvalidExpression : IExpression, ITypeExpression
{
    public Token[]      Tokens         { get; }
    public ISyntaxNode? Expression     { get; }
    public Type?        ExpressionType { get; }

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

    internal InvalidExpression(ISyntaxNode expression, params Token[] tokens)
    {
        Debug.Assert(expression is IExpression or ITypeExpression);

        Tokens         = tokens;
        Expression     = expression;
        ExpressionType = expression.GetType();
    }
}