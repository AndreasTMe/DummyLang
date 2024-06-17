using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Internal;
using DummyLang.SyntacticAnalysis.Utilities;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class ParenthesisedExpression : Expression
{
    public override SyntaxNodeType Type => SyntaxNodeType.ParenthesisedExpression;

    public Token LeftParen { get; }
    public Expression Expression { get; }
    public Token RightParen { get; }

    public ParenthesisedExpression(Token leftParen, Expression expression, Token rightParen)
    {
        if (leftParen.Type != TokenType.None && leftParen.Type != TokenType.LeftParen)
        {
            ThrowHelpers.NotLeftParenthesisOperator();
        }

        if (rightParen.Type != TokenType.None && rightParen.Type != TokenType.RightParen)
        {
            ThrowHelpers.NotRightParenthesisOperator();
        }

        LeftParen = leftParen;
        Expression = expression;
        RightParen = rightParen;
    }

    public override void PrettyPrint(int indent)
    {
        ConsoleUtilities.WriteLineFormatted(nameof(ParenthesisedExpression), indent);

        ConsoleUtilities.WriteLineFormatted(LeftParen.Type.ToString(), indent + 1);
        Expression.PrettyPrint(indent + 1);
        ConsoleUtilities.WriteLineFormatted(RightParen.Type.ToString(), indent + 1);
    }
}