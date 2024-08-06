using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;

namespace DummyLang.SyntacticAnalysis.Statements;

public sealed class ExpressionStatement : IStatement
{
    internal const string SemicolonExpected = "Semicolon expected at the end of an expression statement.";

    public IExpression? Expression { get; }
    public Token        Terminator { get; }

    public TokenPositions Positions { get; }

    internal ExpressionStatement(IExpression? expression, Token terminator)
    {
        Expression = expression;
        Terminator = terminator;

        Positions = new TokenPositions(expression?.Positions[0], terminator.Position);
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}