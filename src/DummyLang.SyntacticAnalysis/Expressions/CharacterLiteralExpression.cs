using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Utilities;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class CharacterLiteralExpression : Expression
{
    public Token CharacterToken { get; }

    public CharacterLiteralExpression(Token characterToken)
    {
        CharacterToken = characterToken;
    }

    public override void PrettyPrint(int indent)
    {
        ConsoleUtilities.WriteLineFormatted($"{nameof(CharacterLiteralExpression)}({CharacterToken.Value})", indent);
    }
}