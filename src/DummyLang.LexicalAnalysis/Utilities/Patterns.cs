using System.Text.RegularExpressions;

namespace DummyLang.LexicalAnalysis.Utilities;

internal abstract partial record Patterns
{
    [GeneratedRegex(Constants.BinaryRegex)]
    internal static partial Regex BinaryNumber();

    [GeneratedRegex(Constants.HexadecimalRegex)]
    internal static partial Regex HexadecimalNumber();

    [GeneratedRegex(Constants.IntegerRegex)]
    internal static partial Regex IntegerNumber();

    [GeneratedRegex(Constants.RealRegex)]
    internal static partial Regex RealNumber();

    private Patterns()
    {
    }
}