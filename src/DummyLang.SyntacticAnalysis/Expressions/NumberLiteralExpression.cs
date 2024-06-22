using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Utilities;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class NumberLiteralExpression : Expression
{
    public Token NumberToken { get; }
    public NumberType Type { get; }

    public NumberLiteralExpression(Token numberToken, NumberType type)
    {
        NumberToken = numberToken;
        Type = type;
    }

    public override void PrettyPrint(int indent)
    {
        ConsoleUtilities.WriteLineFormatted($"{nameof(NumberLiteralExpression)}({NumberToken.Value} : {Type})", indent);
    }
}