using DummyLang.LexicalAnalysis;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class InvalidExpression : Expression
{
    internal const string UnsupportedOrUnimplemented = "Unsupported or unimplemented token.";

    public override bool IsValid => false;

    public Token       Token      { get; }
    public Expression? Expression { get; }

    internal InvalidExpression(Token token, Expression? expression = null)
    {
        Token      = token;
        Expression = expression;
    }
}