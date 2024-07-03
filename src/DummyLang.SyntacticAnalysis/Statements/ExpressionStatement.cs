using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;
using DummyLang.SyntacticAnalysis.Expressions.Abstractions;
using DummyLang.SyntacticAnalysis.Statements.Abstractions;

namespace DummyLang.SyntacticAnalysis.Statements;

public sealed class ExpressionStatement : IStatement
{
    public IExpression Expression { get; }
    public Token       Terminator { get; }

    internal ExpressionStatement(IExpression expression, Token terminator)
    {
        Expression = expression;
        Terminator = terminator;
    }

    public void Accept(ISyntaxNodeVisitor visitor)
    {
        visitor.Visit(this);

        Expression.Accept(visitor);
    }
}