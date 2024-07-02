using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Expressions.Abstractions;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class CharacterLiteralExpression : IExpression, ITypeExpression
{
    // internal const string ShouldBeOfCertainLength =
    //     "Character literal token with invalid length provided.";
    //
    // internal const string ShouldStartEndWithSingleQuote =
    //     "A character literal must start with a single quote (') and end with a single quote (').";
    //
    // internal const string ShouldBeEscaped =
    //     "The character literal provided must be escaped with a backslash (\\).";
    //
    // internal const string InvalidEscapedCharacter =
    //     "The token provided is not a valid escaped character literal.";
    //
    // internal const string InvalidHexadecimalCharacter =
    //     "The token provided is not a valid hexadecimal character literal.";

    public Token CharacterToken { get; }

    internal CharacterLiteralExpression(Token characterToken) => CharacterToken = characterToken;
    
    // Inside "Evaluate" method:
    // var characterValue = characterToken.Value;
    // var diagnosticMessage = string.Empty;
    //
    // if (!characterValue.HasValidCharacterLength())
    //     diagnosticMessage = CharacterLiteralExpression.ShouldBeOfCertainLength;
    // else if (!characterValue.IsSurroundedBySingleQuotes())
    //     diagnosticMessage = CharacterLiteralExpression.ShouldStartEndWithSingleQuote;
    // else if (characterValue.IsUnescapedSingleQuoteOrBackslash())
    //     diagnosticMessage = CharacterLiteralExpression.ShouldBeEscaped;
    // else if (characterValue[1] == '\\')
    // {
    //     if (characterValue[2] == 'x')
    //     {
    //         // ReSharper disable once InvertIf
    //         if (!characterValue.IsValidHexadecimalCharacter())
    //             diagnosticMessage = CharacterLiteralExpression.InvalidEscapedCharacter;
    //     }
    //     else if (!characterValue.IsValidEscapedCharacter())
    //         diagnosticMessage = CharacterLiteralExpression.InvalidHexadecimalCharacter;
    // }
}