using DummyLang.LexicalAnalysis;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class ParenthesisedExpression : Expression
{
    public Token      LeftParen  { get; }
    public Expression Expression { get; }
    public Token      RightParen { get; }

    internal ParenthesisedExpression(Token leftParen, Expression expression, Token? rightParen = null)
    {
        LeftParen  = leftParen;
        Expression = expression;
        RightParen = rightParen ?? Token.None;
    }
}