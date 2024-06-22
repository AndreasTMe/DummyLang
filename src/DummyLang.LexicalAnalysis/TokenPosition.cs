namespace DummyLang.LexicalAnalysis;

public readonly struct TokenPosition
{
    public static readonly TokenPosition Zero = new();

    public int Line { get; }
    public int Column { get; }
    public int Index { get; }
    public int Width { get; }

    private TokenPosition(int line, int column, int index, int width)
    {
        Line = line;
        Column = column;
        Index = index;
        Width = width;
    }

    public static TokenPosition At(int line, int column, int index) => new(line, column, index, 1);

    public static TokenPosition At(int line, int column, int index, int width) => new(line, column, index, width);
}