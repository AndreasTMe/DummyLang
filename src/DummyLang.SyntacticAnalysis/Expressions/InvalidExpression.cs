using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Utilities;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class InvalidExpression : Expression
{
    public override bool IsValid => false;

    public Token Token { get; }
    public Expression? Expression { get; }

    internal InvalidExpression(Token token, Expression? expression = null)
    {
        Token = token;
        Expression = expression;
    }

    public override void PrettyPrint(int indent)
    {
        ConsoleUtilities.WriteLineFormatted(nameof(InvalidExpression), indent);

        if (Expression is null && Token.Type == TokenType.None)
        {
            ConsoleUtilities.WriteLineFormatted("Oops! No info provided!", indent + 1);
        }
        
        Expression?.PrettyPrint(indent + 1);

        if (Token.Type != TokenType.None)
        {
            ConsoleUtilities.WriteLineFormatted(Token.Type.ToString(), indent + 1);
        }
    }
}