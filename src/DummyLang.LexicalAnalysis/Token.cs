using System;

namespace DummyLang.LexicalAnalysis;

public class Token : IEquatable<Token>
{
    public static readonly Token None = new();

    public TokenType Type { get; } = TokenType.None;
    public string Value { get; } = string.Empty;
    public TokenPosition Position { get; } = TokenPosition.Zero;

    private Token()
    {
    }

    public Token(TokenType type, string value, TokenPosition position)
    {
        Type = type;
        Value = value;
        Position = position;
    }

    public bool Equals(Token? other)
    {
        if (other is null)
        {
            return false;
        }
        
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Type == other.Type
               && Value == other.Value
               && Position.Index == other.Position.Index
               && Position.Width == other.Position.Width
               && GetHashCode() == other.GetHashCode();
    }

    public override bool Equals(object? obj) => obj is Token other && Equals(other);

    public override int GetHashCode() => HashCode.Combine((int)Type, Value, Position.Index, Position.Width);

    public static bool operator ==(Token left, Token right) => left.Equals(right);

    public static bool operator !=(Token left, Token right) => !(left == right);
}