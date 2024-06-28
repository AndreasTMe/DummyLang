using DummyLang.LexicalAnalysis;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class CharacterLiteralExpression : Expression
{
    internal const string ShouldBeOfCertainLength =
        "Character literal token with invalid length provided.";

    internal const string ShouldStartEndWithSingleQuote =
        "A character literal must start with a single quote (') and end with a single quote (').";

    internal const string ShouldBeEscaped =
        "The character literal provided must be escaped with a backslash (\\).";

    internal const string InvalidEscapedCharacter =
        "The token provided is not a valid escaped character literal.";

    internal const string InvalidHexadecimalCharacter =
        "The token provided is not a valid hexadecimal character literal.";

    public Token CharacterToken { get; }

    public CharacterLiteralExpression(Token characterToken)
    {
        CharacterToken = characterToken;
    }
}