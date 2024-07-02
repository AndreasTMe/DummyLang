using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Expressions;
using DummyLang.SyntacticAnalysis.Expressions.Abstractions;
using DummyLang.SyntacticAnalysis.Statements.Abstractions;

namespace DummyLang.SyntacticAnalysis.Statements;

public sealed class ExpressionStatement : Statement
{
    public IExpression Expression { get; }
    public Token      Terminator { get; }

    internal ExpressionStatement(IExpression expression, Token terminator)
    {
        Expression = expression;
        Terminator = terminator;
    }
}