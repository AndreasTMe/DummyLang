namespace DummyLang.LexicalAnalysis;

public readonly struct TokenPosition
{
    public static readonly TokenPosition Zero = new();

    public int Start { get; }
    public int End { get; }

    private TokenPosition(int start, int end)
    {
        Start = start;
        End = end;
    }

    public static TokenPosition At(int index) => new(index, index);

    public static TokenPosition At(int start, int end) => new(start, end);
}