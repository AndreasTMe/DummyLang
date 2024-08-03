using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class ParenthesisedExpression : PositionedNode, IExpression, ITypeExpression
{
    internal const string ExpressionExpected       = "Expression expected.";
    internal const string RightParenthesisExpected = "Right parenthesis expected.";

    public TokenPosition Start => _start.GetValueOrDefault();
    public TokenPosition End   => _end.GetValueOrDefault();

    public Token        LeftParen  { get; }
    public ISyntaxNode? Expression { get; }
    public Token        RightParen { get; }

    internal ParenthesisedExpression(Token leftParen, IExpression? expression, Token rightParen)
    {
        LeftParen  = leftParen;
        Expression = expression;
        RightParen = rightParen;

        _start = TokenPosition.GetMin(leftParen.Position, expression?.Start, rightParen.Position);
        _end   = TokenPosition.GetMin(leftParen.Position, expression?.End, rightParen.Position);
    }

    internal ParenthesisedExpression(Token leftParen, ITypeExpression? expression, Token rightParen)
    {
        LeftParen  = leftParen;
        Expression = expression;
        RightParen = rightParen;

        _start = TokenPosition.GetMin(leftParen.Position, expression?.Start, rightParen.Position);
        _end   = TokenPosition.GetMin(leftParen.Position, expression?.End, rightParen.Position);
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}