using DummyLang.SyntacticAnalysis.Utilities;
using System;

namespace DummyLang.SyntacticAnalysis.Extensions;

internal static class StringExtensions
{
    // Character parsing
    internal static bool HasValidCharacterLength(this string str) => str is { Length: >= 3 and <= 8 };

    internal static bool IsSurroundedBySingleQuotes(this string str) => str.StartsWith('\'') && str.EndsWith('\'');

    internal static bool IsUnescapedSingleQuoteOrBackslash(this string str) =>
        str.Length == 3 && (str[1] == '\'' || str[1] == '\\');

    internal static bool IsValidEscapedCharacter(this string str) =>
        str.Length >= 4 && Constants.EscapedCharacters.Contains(str[2]);

    internal static bool IsValidHexadecimalCharacter(this string str) =>
        str.Length >= 5 && Patterns.HexadecimalCharacter().Match(str).Success;

    // String parsing
    internal static bool IsValidLength(this string str) => str.Length >= 2;

    internal static bool IsSurroundedByDoubleQuotes(this string str) => str.StartsWith('\"') && str.EndsWith('\"');

    internal static bool EscapesLastDoubleQuote(this string str) => str[^2] == '\\';

    internal static bool HasInvalidEscapedCharacters(this string str)
    {
        var span           = str.AsSpan();
        var expectsEscaped = false;
        var expectsHex     = false;

        foreach (var current in span)
        {
            if (expectsHex)
            {
                if (!char.IsAsciiHexDigit(current))
                {
                    return true;
                }

                expectsEscaped = false;
                expectsHex     = false;
                continue;
            }

            if (expectsEscaped)
            {
                if (current == 'x')
                {
                    expectsEscaped = false;
                    expectsHex     = true;
                    continue;
                }

                if (!Constants.EscapedCharacters.Contains(current))
                {
                    return true;
                }

                expectsEscaped = false;
                continue;
            }

            expectsEscaped = current == '\\';
        }

        return false;
    }
}