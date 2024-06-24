namespace DummyLang.LexicalAnalysis;

public enum TokenType : byte
{
    None = 0,

    // Delimiters
    Comma     = 1,
    Dot       = 2,
    Semicolon = 3,
    Colon     = 4,

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
    LeftParenthesis,
    RightParenthesis,
    LeftBrace,
    RightBrace,
    LeftBracket,
    RightBracket,

    // Identifiers
    Identifier,

    // Literals
    True,
    False,
    Integer,
    Real,
    Character,
    String,

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

    // End of File
    Eof = 255
}