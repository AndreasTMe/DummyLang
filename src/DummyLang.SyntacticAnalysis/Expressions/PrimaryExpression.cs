using DummyLang.LexicalAnalysis;
using DummyLang.LexicalAnalysis.Extensions;
using DummyLang.SyntacticAnalysis.Utilities;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class PrimaryExpression : Expression
{
    public Expression Expression { get; }
    public Token      Token      { get; }

    public PrimaryExpression(Expression expression, Token? token = null)
    {
        Expression = expression;
        Token      = token ?? Token.None;
    }

    public override void PrettyPrint(int indent)
    {
        ConsoleUtilities.WriteLineFormatted(nameof(PrimaryExpression), indent);

        Expression.PrettyPrint(indent + 1);

        if (!Token.IsInvalid())
            ConsoleUtilities.WriteLineFormatted(Token.Type.ToString(), indent + 1);
    }
}