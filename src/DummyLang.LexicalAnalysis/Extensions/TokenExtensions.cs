using System;

namespace DummyLang.LexicalAnalysis.Extensions;

public static class TokenExtensions
{
    private static readonly TokenType[] BinaryOperators;

    static TokenExtensions()
    {
        BinaryOperators = new TokenType[256];
        Array.Fill(BinaryOperators, TokenType.None);

        BinaryOperators[(int)TokenType.Assign]                   = TokenType.Assign;
        BinaryOperators[(int)TokenType.Equal]                    = TokenType.Equal;
        BinaryOperators[(int)TokenType.NotEqual]                 = TokenType.NotEqual;
        BinaryOperators[(int)TokenType.Plus]                     = TokenType.Plus;
        BinaryOperators[(int)TokenType.PlusAssign]               = TokenType.PlusAssign;
        BinaryOperators[(int)TokenType.Minus]                    = TokenType.Minus;
        BinaryOperators[(int)TokenType.MinusAssign]              = TokenType.MinusAssign;
        BinaryOperators[(int)TokenType.Star]                     = TokenType.Star;
        BinaryOperators[(int)TokenType.StarAssign]               = TokenType.StarAssign;
        BinaryOperators[(int)TokenType.Slash]                    = TokenType.Slash;
        BinaryOperators[(int)TokenType.SlashAssign]              = TokenType.SlashAssign;
        BinaryOperators[(int)TokenType.Percent]                  = TokenType.Percent;
        BinaryOperators[(int)TokenType.PercentAssign]            = TokenType.PercentAssign;
        BinaryOperators[(int)TokenType.Ampersand]                = TokenType.Ampersand;
        BinaryOperators[(int)TokenType.DoubleAmpersand]          = TokenType.DoubleAmpersand;
        BinaryOperators[(int)TokenType.AmpersandAssign]          = TokenType.AmpersandAssign;
        BinaryOperators[(int)TokenType.Pipe]                     = TokenType.Pipe;
        BinaryOperators[(int)TokenType.DoublePipe]               = TokenType.DoublePipe;
        BinaryOperators[(int)TokenType.PipeAssign]               = TokenType.PipeAssign;
        BinaryOperators[(int)TokenType.Caret]                    = TokenType.Caret;
        BinaryOperators[(int)TokenType.CaretAssign]              = TokenType.CaretAssign;
        BinaryOperators[(int)TokenType.LessThan]                 = TokenType.LessThan;
        BinaryOperators[(int)TokenType.LessThanOrEqual]          = TokenType.LessThanOrEqual;
        BinaryOperators[(int)TokenType.GreaterThan]              = TokenType.GreaterThan;
        BinaryOperators[(int)TokenType.GreaterThanOrEqual]       = TokenType.GreaterThanOrEqual;
        BinaryOperators[(int)TokenType.LeftBitShift]             = TokenType.LeftBitShift;
        BinaryOperators[(int)TokenType.LeftBitShiftAssign]       = TokenType.LeftBitShiftAssign;
        BinaryOperators[(int)TokenType.RightBitShift]            = TokenType.RightBitShift;
        BinaryOperators[(int)TokenType.RightBitShiftAssign]      = TokenType.RightBitShiftAssign;
        BinaryOperators[(int)TokenType.DoubleDot]                = TokenType.DoubleDot;
        BinaryOperators[(int)TokenType.DoubleQuestionMark]       = TokenType.DoubleQuestionMark;
        BinaryOperators[(int)TokenType.DoubleQuestionMarkAssign] = TokenType.DoubleQuestionMarkAssign;
    }

    public static bool IsBinaryOperator(this Token token) => BinaryOperators[(int)token.Type] == token.Type;

    public static bool IsNumber(this Token token) => token.Type == TokenType.Integer || token.Type == TokenType.Real;

    public static bool IsParenthesisMatch(this Token token, Token other) =>
        token.Type == TokenType.LeftParenthesis && other.Type == TokenType.RightParenthesis;

    public static bool IsBracketMatch(this Token token, Token other) =>
        token.Type == TokenType.LeftBracket && other.Type == TokenType.RightBracket;

    public static bool IsBraceMatch(this Token token, Token other) =>
        token.Type == TokenType.LeftBrace && other.Type == TokenType.RightBrace;

    public static bool IsEndOfFile(this Token token) => token.Type == TokenType.Eof;

    public static bool IsInvalid(this Token token) => token.Type == TokenType.None;
}