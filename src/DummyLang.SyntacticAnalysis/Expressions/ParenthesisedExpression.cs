using DummyLang.LexicalAnalysis;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class ParenthesisedExpression : Expression
{
    internal const string OpeningParenthesisExpected = "An opening parenthesis token was expected.";
    internal const string ClosingParenthesisExpected = "A closing parenthesis token was expected.";

    public Token      LeftParen  { get; }
    public Expression Expression { get; }
    public Token      RightParen { get; }

    public ParenthesisedExpression(Token leftParen, Expression expression, Token? rightParen = null)
    {
        LeftParen  = leftParen;
        Expression = expression;
        RightParen = rightParen ?? Token.None;
    }
}