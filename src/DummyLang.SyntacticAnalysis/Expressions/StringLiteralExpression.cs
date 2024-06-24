using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Utilities;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class StringLiteralExpression : Expression
{
    internal const string ShouldBeSurroundedByDoubleQuote =
        "A string literal must start with a double quote (\") and end with a double quote (\").";

    internal const string ShouldNotEscapeLastDoubleQuote =
        "The last double quote of a string literal should not be escaped.";

    internal const string InvalidEscapedCharacter =
        "The string literal provided contains an invalid escaped character literal.";

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