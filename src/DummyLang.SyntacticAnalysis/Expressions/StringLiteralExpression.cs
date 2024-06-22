using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Utilities;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class StringLiteralExpression : Expression
{
    public Token StringToken { get; }

    public StringLiteralExpression(Token stringToken)
    {
        StringToken = stringToken;
    }

    public override void PrettyPrint(int indent)
    {
        ConsoleUtilities.WriteLineFormatted($"{nameof(StringLiteralExpression)}({StringToken.Value})", indent);
    }
}