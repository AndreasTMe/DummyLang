using System;

namespace DummyLang.LexicalAnalysis.Extensions;

public static class TokenExtensions
{
    private static readonly TokenType[] BinaryOperators;
    private static readonly TokenType[] AssignmentOperators;

    static TokenExtensions()
    {
        BinaryOperators     = new TokenType[256];
        AssignmentOperators = new TokenType[256];

        Array.Fill(BinaryOperators, TokenType.None);
        Array.Fill(AssignmentOperators, TokenType.None);

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
        BinaryOperators[(int)TokenType.Caret]              = TokenType.Caret;
        BinaryOperators[(int)TokenType.LessThan]           = TokenType.LessThan;
        BinaryOperators[(int)TokenType.LessThanOrEqual]    = TokenType.LessThanOrEqual;
        BinaryOperators[(int)TokenType.GreaterThan]        = TokenType.GreaterThan;
        BinaryOperators[(int)TokenType.GreaterThanOrEqual] = TokenType.GreaterThanOrEqual;
        BinaryOperators[(int)TokenType.LeftBitShift]       = TokenType.LeftBitShift;
        BinaryOperators[(int)TokenType.RightBitShift]      = TokenType.RightBitShift;
        BinaryOperators[(int)TokenType.DoubleDot]          = TokenType.DoubleDot;
        BinaryOperators[(int)TokenType.DoubleQuestionMark] = TokenType.DoubleQuestionMark;

        AssignmentOperators[(int)TokenType.Assign]                   = TokenType.Assign;
        AssignmentOperators[(int)TokenType.AssignLambda]             = TokenType.AssignLambda;
        AssignmentOperators[(int)TokenType.PlusAssign]               = TokenType.PlusAssign;
        AssignmentOperators[(int)TokenType.MinusAssign]              = TokenType.MinusAssign;
        AssignmentOperators[(int)TokenType.StarAssign]               = TokenType.StarAssign;
        AssignmentOperators[(int)TokenType.SlashAssign]              = TokenType.SlashAssign;
        AssignmentOperators[(int)TokenType.PercentAssign]            = TokenType.PercentAssign;
        AssignmentOperators[(int)TokenType.AmpersandAssign]          = TokenType.AmpersandAssign;
        AssignmentOperators[(int)TokenType.PipeAssign]               = TokenType.PipeAssign;
        AssignmentOperators[(int)TokenType.CaretAssign]              = TokenType.CaretAssign;
        AssignmentOperators[(int)TokenType.LeftBitShiftAssign]       = TokenType.LeftBitShiftAssign;
        AssignmentOperators[(int)TokenType.RightBitShiftAssign]      = TokenType.RightBitShiftAssign;
        AssignmentOperators[(int)TokenType.DoubleQuestionMarkAssign] = TokenType.DoubleQuestionMarkAssign;
    }

    public static bool IsBinaryOperator(this Token token) => BinaryOperators[(int)token.Type] == token.Type;

    public static bool IsAssignmentOperator(this Token token) => AssignmentOperators[(int)token.Type] == token.Type;

    public static bool IsNumber(this Token token) => token.Type == TokenType.Integer || token.Type == TokenType.Real;

    public static bool IsIdentifierOrLiteral(this Token token) =>
        token.Type >= TokenType.Identifier && token.Type < TokenType.Var;

    public static bool IsReservedKeyword(this Token token) => token.Type >= TokenType.Var;

    public static bool IsParenthesisMatch(this Token token, Token other) =>
        token.Type == TokenType.LeftParenthesis && other.Type == TokenType.RightParenthesis;

    public static bool IsBracketMatch(this Token token, Token other) =>
        token.Type == TokenType.LeftBracket && other.Type == TokenType.RightBracket;

    public static bool IsBraceMatch(this Token token, Token other) =>
        token.Type == TokenType.LeftBrace && other.Type == TokenType.RightBrace;

    public static bool IsEndOfStatement(this Token token) => token.Type == TokenType.Semicolon;

    public static bool IsEndOfFile(this Token token) => token.Type == TokenType.Eof;

    public static bool IsInvalid(this Token token) => token.Type == TokenType.None;
}