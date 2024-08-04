using System;

namespace DummyLang.LexicalAnalysis;

public readonly struct TokenPosition : IEquatable<TokenPosition>, IComparable<TokenPosition>
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

    public int CompareTo(TokenPosition other)
    {
        var lineComparison = Line.CompareTo(other.Line);
        return lineComparison != 0 ? lineComparison : Column.CompareTo(other.Column);
    }

    public bool Equals(TokenPosition other) =>
        Line == other.Line
        && Column == other.Column
        && GetHashCode() == other.GetHashCode();

    public override bool Equals(object? obj) => obj is TokenPosition other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(Line, Column);

    public static bool operator ==(TokenPosition left, TokenPosition right) => left.Equals(right);

    public static bool operator !=(TokenPosition left, TokenPosition right) => !(left == right);
}