using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Utilities;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class PrimaryExpression : Expression
{
    public Expression Expression { get; }
    public Token      Token      { get; }

    public PrimaryExpression(Expression expression, Token token)
    {
        Expression = expression;
        Token      = token;
    }

    public override void PrettyPrint(int indent)
    {
        ConsoleUtilities.WriteLineFormatted(nameof(PrimaryExpression), indent);

        Expression.PrettyPrint(indent + 1);
        ConsoleUtilities.WriteLineFormatted(Token.Type.ToString(), indent + 1);
    }
}