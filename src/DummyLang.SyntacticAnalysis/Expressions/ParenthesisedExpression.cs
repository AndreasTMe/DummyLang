using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class ParenthesisedExpression : IExpression, ITypeExpression
{
    public Token       LeftParen  { get; }
    public ISyntaxNode Expression { get; }
    public Token       RightParen { get; }

    internal ParenthesisedExpression(Token leftParen, IExpression expression, Token? rightParen = null)
    {
        LeftParen  = leftParen;
        Expression = expression;
        RightParen = rightParen ?? Token.None;
    }

    internal ParenthesisedExpression(Token leftParen, ITypeExpression expression, Token? rightParen = null)
    {
        LeftParen  = leftParen;
        Expression = expression;
        RightParen = rightParen ?? Token.None;
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}