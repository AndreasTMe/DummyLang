using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Utilities;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class UnaryExpression : Expression
{
    internal const string AppliedToInvalidToken = "Invalid token next to a unary operator.";

    public Token      Token      { get; }
    public Expression Expression { get; }

    public UnaryExpression(Token token, Expression expression)
    {
        Token      = token;
        Expression = expression;
    }

    public override void PrettyPrint(int indent)
    {
        ConsoleUtilities.WriteLineFormatted(nameof(UnaryExpression), indent);

        ConsoleUtilities.WriteLineFormatted(Token.Type.ToString(), indent + 1);
        Expression.PrettyPrint(indent + 1);
    }
}