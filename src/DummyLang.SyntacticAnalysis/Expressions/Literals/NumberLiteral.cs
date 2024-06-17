using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Utilities;

namespace DummyLang.SyntacticAnalysis.Expressions.Literals;

public sealed class NumberLiteral : LiteralExpression
{
    public Token Token { get; }

    public NumberLiteral(Token token)
    {
        Token = token;
    }

    public override void PrettyPrint(int indent)
    {
        ConsoleUtilities.WriteLineFormatted($"{nameof(NumberLiteral)}({Token.Value})", indent);
    }
}