using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Expressions;

namespace DummyLang.SyntacticAnalysis.Statements;

public sealed class ExpressionStatement : Statement
{
    public Expression Expression { get; }
    public Token      Terminator { get; }

    internal ExpressionStatement(Expression expression, Token terminator)
    {
        Expression = expression;
        Terminator = terminator;
    }
}