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
    Percent,
    Ampersand,
    LessThan,
    LessThanOrEqual,
    LeftBitShift,
    GreaterThan,
    GreaterThanOrEqual,
    RightBitShift,
    Tilde,

    // Parentheses
    LeftParen,
    RightParen,
    LeftBrace,
    RightBrace,
    LeftBracket,
    RightBracket,

    // Identifiers
    Identifier,
    
    // Literals
    Boolean,
    Integer,
    Real,
    Character,
    String,
    Null,

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