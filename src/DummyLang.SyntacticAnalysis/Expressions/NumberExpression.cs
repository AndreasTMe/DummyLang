using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Utilities;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class NumberExpression : Expression
{
    public Token Token { get; }
    public NumberType Type { get; }

    public NumberExpression(Token token, NumberType type)
    {
        Token = token;
        Type = type;
    }

    public override void PrettyPrint(int indent)
    {
        ConsoleUtilities.WriteLineFormatted($"{nameof(NumberExpression)}({Token.Value} : {Type})", indent);
    }
}