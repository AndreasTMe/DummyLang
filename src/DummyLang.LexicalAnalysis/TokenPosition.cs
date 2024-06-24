namespace DummyLang.LexicalAnalysis;

public readonly struct TokenPosition
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
}