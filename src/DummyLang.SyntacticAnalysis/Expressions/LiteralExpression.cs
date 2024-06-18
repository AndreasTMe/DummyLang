using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Utilities;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class LiteralExpression : Expression
{
    public Token Token { get; }

    public LiteralExpression(Token token)
    {
        Token = token;
    }

    public override void PrettyPrint(int indent)
    {
        ConsoleUtilities.WriteLineFormatted($"{nameof(LiteralExpression)}({Token.Value})", indent);
    }
}