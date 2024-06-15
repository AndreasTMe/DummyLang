namespace DummyLang.LexicalAnalysis;

public readonly struct Token
{
    public TokenType Type { get; }
    public string Value { get; }
    public TokenPosition Position { get; }

    public Token(TokenType type, string value, TokenPosition position)
    {
        Type = type;
        Value = value;
        Position = position;
    }
}