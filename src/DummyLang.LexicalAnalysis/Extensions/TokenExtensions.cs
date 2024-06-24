using System;

namespace DummyLang.LexicalAnalysis.Extensions;

public static class TokenExtensions
{
    private static readonly TokenType[] BinaryOperators;

    static TokenExtensions()
    {
        BinaryOperators = new TokenType[Enum.GetValues<TokenType>().Length];
        Array.Fill(BinaryOperators, TokenType.None);

        // TODO: Add new binary token types
        BinaryOperators[(int)TokenType.Assign]             = TokenType.Assign;
        BinaryOperators[(int)TokenType.Equal]              = TokenType.Equal;
        BinaryOperators[(int)TokenType.NotEqual]           = TokenType.NotEqual;
        BinaryOperators[(int)TokenType.Plus]               = TokenType.Plus;
        BinaryOperators[(int)TokenType.Minus]              = TokenType.Minus;
        BinaryOperators[(int)TokenType.Star]               = TokenType.Star;
        BinaryOperators[(int)TokenType.Slash]              = TokenType.Slash;
        BinaryOperators[(int)TokenType.Percent]            = TokenType.Percent;
        BinaryOperators[(int)TokenType.Ampersand]          = TokenType.Ampersand;
        BinaryOperators[(int)TokenType.DoubleAmpersand]    = TokenType.DoubleAmpersand;
        BinaryOperators[(int)TokenType.Pipe]               = TokenType.Pipe;
        BinaryOperators[(int)TokenType.DoublePipe]         = TokenType.DoublePipe;
        BinaryOperators[(int)TokenType.LessThan]           = TokenType.LessThan;
        BinaryOperators[(int)TokenType.LessThanOrEqual]    = TokenType.LessThanOrEqual;
        BinaryOperators[(int)TokenType.LeftBitShift]       = TokenType.LeftBitShift;
        BinaryOperators[(int)TokenType.GreaterThan]        = TokenType.GreaterThan;
        BinaryOperators[(int)TokenType.GreaterThanOrEqual] = TokenType.GreaterThanOrEqual;
        BinaryOperators[(int)TokenType.RightBitShift]      = TokenType.RightBitShift;
        BinaryOperators[(int)TokenType.DoubleDot]          = TokenType.DoubleDot;
        BinaryOperators[(int)TokenType.Caret]              = TokenType.Caret;
        BinaryOperators[(int)TokenType.DoubleQuestionMark] = TokenType.DoubleQuestionMark;
    }

    public static bool IsBinaryOperator(this Token token) => BinaryOperators[(int)token.Type] == token.Type;

    public static bool IsEof(this Token token) => token.Type == TokenType.Eof;

    public static bool IsInvalid(this Token token) => token.Type == TokenType.None;
}