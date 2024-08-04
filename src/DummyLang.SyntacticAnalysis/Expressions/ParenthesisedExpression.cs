using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class ParenthesisedExpression : IExpression, ITypeExpression
{
    internal const string ExpressionExpected       = "Expression expected.";
    internal const string RightParenthesisExpected = "Right parenthesis expected.";

    public Token        LeftParen  { get; }
    public ISyntaxNode? Expression { get; }
    public Token        RightParen { get; }

    public TokenPositions Positions { get; }

    internal ParenthesisedExpression(Token leftParen, IExpression? expression, Token rightParen)
    {
        LeftParen  = leftParen;
        Expression = expression;
        RightParen = rightParen;

        Positions = new TokenPositions(leftParen.Position, expression?.Positions[0], rightParen.Position);
    }

    internal ParenthesisedExpression(Token leftParen, ITypeExpression? expression, Token rightParen)
    {
        LeftParen  = leftParen;
        Expression = expression;
        RightParen = rightParen;

        Positions = new TokenPositions(leftParen.Position, expression?.Positions[0], rightParen.Position);
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}