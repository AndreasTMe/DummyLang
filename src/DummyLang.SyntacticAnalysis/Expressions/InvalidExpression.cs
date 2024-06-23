using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Utilities;
using System;
using System.Text;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class InvalidExpression : Expression
{
    private static readonly StringBuilder Sb = new();

    public override bool IsValid => false;

    public Expression? Expression { get; }
    public Token Token { get; }
    public string Message { get; }
    public string StackTrace { get; }

    internal InvalidExpression(Expression? expression, Token token, string? message = null)
    {
        Expression = expression;

        ArgumentNullException.ThrowIfNull(token.Position);
        Token = token;

        Sb.Clear();

        if (token.Type != TokenType.None)
        {
            Sb.Append("Invalid token: [").Append(token.Value).Append("].");
        }
        else
        {
            Sb.Append("Invalid token.");
        }

        if (message != null)
        {
            Sb.Append(' ').Append(message);
        }

        Message = Sb.ToString();

        Sb.Clear();
        Sb.Append("line: ").Append(token.Position.Line)
            .Append(", column: ").Append(token.Position.Column)
            .Append('.');
        StackTrace = Sb.ToString();
    }

    internal InvalidExpression(Token token, string? message = null)
        : this(null, token, message)
    {
    }

    public override void PrettyPrint(int indent)
    {
        ConsoleUtilities.WriteLineFormatted(nameof(InvalidExpression), indent);

        Expression?.PrettyPrint(indent + 1);

        if (Token.Type != TokenType.None)
        {
            ConsoleUtilities.WriteLineFormatted(Token.Type.ToString(), indent + 1);
        }

        ConsoleUtilities.WriteLineFormatted(Message, indent + 1);
        ConsoleUtilities.WriteLineFormatted($"    {StackTrace}", indent + 1);
    }
}