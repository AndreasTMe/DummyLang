using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Utilities;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class IdentifierExpression : Expression
{
    public Token Token { get; }

    public IdentifierExpression(Token token)
    {
        Token = token;
    }

    public override void PrettyPrint(int indent)
    {
        ConsoleUtilities.WriteLineFormatted($"{nameof(IdentifierExpression)}({Token.Value})", indent);
    }
}