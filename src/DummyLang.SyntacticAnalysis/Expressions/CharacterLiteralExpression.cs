using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class CharacterLiteralExpression : IExpression, ITypeExpression
{
    internal const string InvalidLength = "Character literal token with invalid length provided.";
    internal const string InvalidSingleQuotes =
        "A character literal must start with a single quote (') and end with a single quote (').";
    internal const string NonEscapedCharacter = "The character literal provided must be escaped with a backslash (\\).";
    internal const string InvalidEscapedCharacter = "The token provided is not a valid escaped character literal.";
    internal const string InvalidHex = "The token provided is not a valid hexadecimal character literal.";

    public Token CharacterToken { get; }

    public TokenPositions Positions { get; }

    internal CharacterLiteralExpression(Token characterToken)
    {
        CharacterToken = characterToken;

        Positions = new TokenPositions(characterToken.Position);
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}