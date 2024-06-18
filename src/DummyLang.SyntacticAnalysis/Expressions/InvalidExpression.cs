using DummyLang.SyntacticAnalysis.Utilities;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class InvalidExpression<T> : Expression
    where T : Expression
{
    public T? Expression { get; }
    public string? Message { get; }

    public InvalidExpression(T expression)
    {
        Expression = expression;
        Message = null;
    }

    public InvalidExpression(string message)
    {
        Expression = default;
        Message = message;
    }

    public override void PrettyPrint(int indent)
    {
        ConsoleUtilities.WriteLineFormatted(nameof(InvalidExpression<T>), indent);

        if (Expression != null && Message == null)
        {
            Expression.PrettyPrint(indent + 1);
        }
        else if (Expression == null && Message != null)
        {
            ConsoleUtilities.WriteLineFormatted(Message, indent + 1);
        }
    }
}