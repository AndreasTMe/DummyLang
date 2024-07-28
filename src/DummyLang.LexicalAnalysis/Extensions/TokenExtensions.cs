using System;
using System.Runtime.CompilerServices;

namespace DummyLang.LexicalAnalysis.Extensions;

public static class TokenExtensions
{
    private static readonly int[] BinaryOperators;
    private static readonly int[] AllowedTypeTokens;

    static TokenExtensions()
    {
        BinaryOperators =
        [
            (int)TokenType.Assign,
            (int)TokenType.LambdaAssign,
            (int)TokenType.Equal,
            (int)TokenType.NotEqual,
            (int)TokenType.Plus,
            (int)TokenType.PlusAssign,
            (int)TokenType.Minus,
            (int)TokenType.MinusAssign,
            (int)TokenType.Star,
            (int)TokenType.StarAssign,
            (int)TokenType.Slash,
            (int)TokenType.SlashAssign,
            (int)TokenType.Percent,
            (int)TokenType.PercentAssign,
            (int)TokenType.Ampersand,
            (int)TokenType.DoubleAmpersand,
            (int)TokenType.AmpersandAssign,
            (int)TokenType.Pipe,
            (int)TokenType.DoublePipe,
            (int)TokenType.PipeAssign,
            (int)TokenType.Caret,
            (int)TokenType.CaretAssign,
            (int)TokenType.LessThan,
            (int)TokenType.LessThanOrEqual,
            (int)TokenType.GreaterThan,
            (int)TokenType.GreaterThanOrEqual,
            (int)TokenType.LeftBitShift,
            (int)TokenType.LeftBitShiftAssign,
            (int)TokenType.RightBitShift,
            (int)TokenType.RightBitShiftAssign,
            (int)TokenType.DoubleDot,
            (int)TokenType.DoubleQuestionMark,
            (int)TokenType.DoubleQuestionMarkAssign
        ];
        Array.Sort(BinaryOperators);

        AllowedTypeTokens =
        [
            (int)TokenType.Comma,
            (int)TokenType.Ampersand,
            (int)TokenType.Pipe,
            (int)TokenType.Caret,
            (int)TokenType.LessThan,
            (int)TokenType.GreaterThan,
            (int)TokenType.LeftParenthesis,
            (int)TokenType.RightParenthesis,
            (int)TokenType.Identifier,
            (int)TokenType.True,
            (int)TokenType.False,
            (int)TokenType.Integer,
            (int)TokenType.Real,
            (int)TokenType.Character,
            (int)TokenType.String
        ];
        Array.Sort(AllowedTypeTokens);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsBinaryOperator(this Token token) => Contains(BinaryOperators, token.Type);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsBitwiseOperator(this Token token) =>
        token.Type == TokenType.Ampersand || token.Type == TokenType.Pipe || token.Type == TokenType.Caret;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsIdentifierOrLiteral(this Token token) =>
        token.Type >= TokenType.Identifier && token.Type < TokenType.Var;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsReservedKeyword(this Token token) => token.Type >= TokenType.Var;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAllowedInTypes(this Token token) => Contains(AllowedTypeTokens, token.Type);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsParenthesisMatch(this Token token, Token other) =>
        token.Type == TokenType.LeftParenthesis && other.Type == TokenType.RightParenthesis;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsBracketMatch(this Token token, Token other) =>
        token.Type == TokenType.LeftBracket && other.Type == TokenType.RightBracket;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsBraceMatch(this Token token, Token other) =>
        token.Type == TokenType.LeftBrace && other.Type == TokenType.RightBrace;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsGenericMatch(this Token token, Token other) =>
        token.Type == TokenType.LessThan && other.Type == TokenType.GreaterThan;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEndOfStatement(this Token token) => token.Type == TokenType.Semicolon;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEndOfFile(this Token token) => token.Type == TokenType.Eof;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNone(this Token token) => token.Type == TokenType.None;

    private static bool Contains(in int[] array, TokenType tokenType)
    {
        var left   = 0;
        var right  = array.Length - 1;
        var target = (int)tokenType;

        while (left <= right)
        {
            var mid = left + (right - left) / 2;

            if (array[mid] == target)
                return true;

            if (array[mid] < target)
                left = mid + 1;
            else
                right = mid - 1;
        }

        return false;
    }
}