using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Utilities;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class RangeExpression : Expression
{
    public Expression Start { get; }
    public Token      Operator     { get; }
    public Expression End     { get; }

    public RangeExpression(Expression start, Token @operator, Expression end)
    {
        Start = start;
        Operator     = @operator;
        End     = end;
    }

    public override void PrettyPrint(int indent)
    {
        ConsoleUtilities.WriteLineFormatted(nameof(RangeExpression), indent);

        Start.PrettyPrint(indent + 1);

        if (Operator.Type != TokenType.None)
            ConsoleUtilities.WriteLineFormatted(Operator.Type.ToString(), indent + 1);

        End.PrettyPrint(indent + 1);
    }
}