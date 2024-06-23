using System.Diagnostics.CodeAnalysis;

namespace DummyLang.SyntacticAnalysis.Utilities;

internal abstract record Constants
{
    [StringSyntax("Regex")]
    public const string HexadecimalRegex = @"(?i)^'\\x[0-9a-f]{1,4}'$";

    public const string EscapedCharacters = "'\'\"\\0abfnrtv";

    private Constants()
    {
    }
}