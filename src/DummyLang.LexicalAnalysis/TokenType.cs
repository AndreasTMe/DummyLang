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
    Minus,
    Star,
    Slash,
    LessThan,
    LessThanOrEqual,
    GreaterThan,
    GreaterThanOrEqual,

    // Parentheses
    LeftParen,
    RightParen,
    LeftBrace,
    RightBrace,
    LeftBracket,
    RightBracket,

    // Identifiers
    Identifier,
    Number,

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