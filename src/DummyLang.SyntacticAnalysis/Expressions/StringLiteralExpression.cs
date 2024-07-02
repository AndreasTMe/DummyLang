using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Expressions.Abstractions;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class StringLiteralExpression : IExpression, ITypeExpression
{
    // internal const string ShouldBeSurroundedByDoubleQuote =
    //     "A string literal must start with a double quote (\") and end with a double quote (\").";
    //
    // internal const string ShouldNotEscapeLastDoubleQuote =
    //     "The last double quote of a string literal should not be escaped.";
    //
    // internal const string InvalidEscapedCharacter =
    //     "The string literal provided contains an invalid escaped character literal.";

    public Token StringToken { get; }

    internal StringLiteralExpression(Token stringToken) => StringToken = stringToken;
    
    // Inside "Evaluate" method:
    // var stringValue = stringToken.Value;
    // var diagnosticsMessage = string.Empty;
    //
    // if (!stringValue.IsValidLength() || !stringValue.IsSurroundedByDoubleQuotes())
    //     diagnosticsMessage = StringLiteralExpression.ShouldBeSurroundedByDoubleQuote;
    // else if (stringValue.EscapesLastDoubleQuote())
    //     diagnosticsMessage = StringLiteralExpression.ShouldNotEscapeLastDoubleQuote;
    // else if (stringValue.HasInvalidEscapedCharacters())
    //     diagnosticsMessage = StringLiteralExpression.InvalidEscapedCharacter;
}