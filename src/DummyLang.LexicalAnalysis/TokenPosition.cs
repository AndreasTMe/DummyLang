using System;
using System.Linq;

namespace DummyLang.LexicalAnalysis;

public readonly struct TokenPosition : IComparable<TokenPosition>
{
    public static readonly TokenPosition Zero = new();

    public int Line   { get; }
    public int Column { get; }

    private TokenPosition(int line, int column)
    {
        Line   = line;
        Column = column;
    }

    public static TokenPosition At(int line, int column) => new(line, column);

    public static TokenPosition GetMin(params TokenPosition?[] positions) => SelectValid(positions).MinBy(p => p);

    public static TokenPosition GetMax(params TokenPosition?[] positions) => SelectValid(positions).MaxBy(p => p);

    private static TokenPosition[] SelectValid(params TokenPosition?[] positions)
    {
        return positions is { Length: > 0 }
            ? positions.Where(p => p.HasValue).Select(p => p!.Value).ToArray()
            : [];
    }

    public int CompareTo(TokenPosition other)
    {
        var lineComparison = Line.CompareTo(other.Line);
        return lineComparison != 0 ? lineComparison : Column.CompareTo(other.Column);
    }
}