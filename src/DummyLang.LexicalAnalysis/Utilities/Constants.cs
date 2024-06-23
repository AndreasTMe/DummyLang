using System.Diagnostics.CodeAnalysis;

namespace DummyLang.LexicalAnalysis.Utilities;

internal abstract record Constants
{
    [StringSyntax("Regex")]
    public const string BinaryRegex = "^0(?i)b[0-1]+(ul|u|l)?";

    [StringSyntax("Regex")]
    public const string HexadecimalRegex = "^0(?i)x[0-9a-f]+(ul|u|l)?";

    [StringSyntax("Regex")]
    public const string IntegerRegex = "^[0-9]+(?i)(ul|u|l)?";

    [StringSyntax("Regex")]
    public const string RealRegex = "^[0-9]+\\.[0-9]+(?i)(e[+|-][0-9]+)?(f|d|m)?";
}