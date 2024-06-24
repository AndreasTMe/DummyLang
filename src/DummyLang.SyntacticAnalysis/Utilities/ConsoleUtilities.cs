using System;

namespace DummyLang.SyntacticAnalysis.Utilities;

internal static class ConsoleUtilities
{
    public static void WriteLineFormatted(string value, int indent)
    {
        switch (indent)
        {
            case 0:
                Console.WriteLine(value);
                return;
            default:
                Console.WriteLine("{0}{1}", GetIndent(indent), value);
                return;
        }
    }

    private static string GetIndent(int indent)
    {
        if (indent <= 0)
        {
            return string.Empty;
        }

        return new string(' ', (indent - 1) * 5) + "  +--";
    }
}