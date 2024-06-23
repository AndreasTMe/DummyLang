using System.Text.RegularExpressions;

namespace DummyLang.SyntacticAnalysis.Utilities;

internal abstract partial record Patterns
{
    [GeneratedRegex(Constants.HexadecimalRegex)]
    public static partial Regex HexadecimalCharacter();
}