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
    
    // Quotes
    SingleQuote,
    DoubleQuote,

    // Operators
    Assign,
    Bang,
    Equal,
    NotEqual,
    Plus,
    PlusPlus,
    Minus,
    MinusMinus,
    PointerAccess,
    Star,
    Slash,
    Percent,
    Ampersand,
    DoubleAmpersand,
    Pipe,
    DoublePipe,
    LessThan,
    LessThanOrEqual,
    LeftBitShift,
    GreaterThan,
    GreaterThanOrEqual,
    RightBitShift,
    Tilde,
    DoubleDot,
    Caret,
    QuestionMark,
    DoubleQuestionMark,

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
    Return,
    As,
    Default
}