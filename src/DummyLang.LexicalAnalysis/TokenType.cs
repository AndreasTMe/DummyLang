namespace DummyLang.LexicalAnalysis;

public enum TokenType : byte
{
    None = 0,

    Comma     = 1,
    Dot       = 2,
    Semicolon = 3,
    Colon     = 4,

    Assign = 10,

    Bang = 15,

    Equal    = 20,
    NotEqual = 21,

    Plus          = 30,
    PlusPlus      = 31,
    PlusAssign    = 32,
    Minus         = 33,
    MinusMinus    = 34,
    MinusAssign   = 35,
    PointerAccess = 36,
    Star          = 37,
    StarAssign    = 38,
    Slash         = 39,
    SlashAssign   = 40,
    Percent       = 41,
    PercentAssign = 42,

    Ampersand       = 45,
    DoubleAmpersand = 46,
    AmpersandAssign = 47,
    Pipe            = 48,
    DoublePipe      = 49,
    PipeAssign      = 50,
    Tilde           = 51,
    Caret           = 52,
    CaretAssign     = 53,

    LessThan           = 55,
    LessThanOrEqual    = 56,
    GreaterThan        = 57,
    GreaterThanOrEqual = 58,

    LeftBitShift        = 60,
    LeftBitShiftAssign  = 61,
    RightBitShift       = 62,
    RightBitShiftAssign = 63,

    DoubleDot = 65,

    QuestionMark             = 70,
    DoubleQuestionMark       = 71,
    DoubleQuestionMarkAssign = 72,

    LeftParenthesis  = 75,
    RightParenthesis = 76,
    LeftBrace        = 77,
    RightBrace       = 78,
    LeftBracket      = 79,
    RightBracket     = 80,

    Identifier = 85,

    True      = 90,
    False     = 91,
    Integer   = 92,
    Real      = 93,
    Character = 94,
    String    = 95,

    Var      = 100,
    Const    = 101,
    Fun      = 102,
    If       = 103,
    Else     = 104,
    Break    = 105,
    While    = 106,
    Continue = 107,
    Return   = 108,

    Eof = 255
}