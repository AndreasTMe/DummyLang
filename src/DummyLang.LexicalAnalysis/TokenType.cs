namespace DummyLang.LexicalAnalysis;

public enum TokenType
{
    Invalid = 0,
    Eof,
    
    // Delimiters
    Comma,
    Semicolon,
    Colon,
    
    // Operators
    Assign,
    Plus,
    Minus,
    Star,
    Slash,
    LessThan,
    GreaterThan,
    
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
    Return
}