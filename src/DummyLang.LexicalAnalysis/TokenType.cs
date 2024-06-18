namespace DummyLang.LexicalAnalysis;

public enum TokenType : byte
{
    None = 0,
    Eof,

    // Delimiters
    Comma,
    Dot,
    Semicolon,
    Colon,

    // Operators
    Assign,
    Bang,
    Equal,
    NotEqual,
    Plus,
    PlusPlus,
    Minus,
    MinusMinus,
    Star,
    Slash,
    LessThan,
    LessThanOrEqual,
    LeftBitShift,
    GreaterThan,
    GreaterThanOrEqual,
    RightBitShift,

    // Parentheses
    LeftParen,
    RightParen,
    LeftBrace,
    RightBrace,
    LeftBracket,
    RightBracket,

    // Identifiers
    Identifier,
    Integer,

    // Keywords
    Var,
    Const,
    Fun,
    If,
    Else,
    Break,
    While,
    Continue,
    Return
}