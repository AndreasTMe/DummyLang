using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Utilities;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class ParenthesisedExpression : Expression
{
    public Token LeftParen { get; }
    public Expression Expression { get; }
    public Token RightParen { get; }

    public ParenthesisedExpression(Token leftParen, Expression expression, Token? rightParen = null)
    {
        LeftParen = leftParen;
        Expression = expression;
        RightParen = rightParen ?? Token.None;
    }

    public override void PrettyPrint(int indent)
    {
        ConsoleUtilities.WriteLineFormatted(nameof(ParenthesisedExpression), indent);

        ConsoleUtilities.WriteLineFormatted(LeftParen.Type.ToString(), indent + 1);
        Expression.PrettyPrint(indent + 1);
        ConsoleUtilities.WriteLineFormatted(RightParen.Type.ToString(), indent + 1);
    }
}