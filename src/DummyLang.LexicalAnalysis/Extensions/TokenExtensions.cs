using System.Collections.Frozen;
using System.Collections.Generic;

namespace DummyLang.LexicalAnalysis.Extensions;

public static class TokenExtensions
{
    private static readonly FrozenSet<TokenType> BinaryOperators;

    static TokenExtensions() =>
        BinaryOperators = new HashSet<TokenType>
        {
            TokenType.Assign,
            TokenType.LambdaAssign,
            TokenType.Equal,
            TokenType.NotEqual,
            TokenType.Plus,
            TokenType.PlusAssign,
            TokenType.Minus,
            TokenType.MinusAssign,
            TokenType.Star,
            TokenType.StarAssign,
            TokenType.Slash,
            TokenType.SlashAssign,
            TokenType.Percent,
            TokenType.PercentAssign,
            TokenType.Ampersand,
            TokenType.DoubleAmpersand,
            TokenType.AmpersandAssign,
            TokenType.Pipe,
            TokenType.DoublePipe,
            TokenType.PipeAssign,
            TokenType.Caret,
            TokenType.CaretAssign,
            TokenType.LessThan,
            TokenType.LessThanOrEqual,
            TokenType.GreaterThan,
            TokenType.GreaterThanOrEqual,
            TokenType.LeftBitShift,
            TokenType.LeftBitShiftAssign,
            TokenType.RightBitShift,
            TokenType.RightBitShiftAssign,
            TokenType.DoubleDot,
            TokenType.DoubleQuestionMark,
            TokenType.DoubleQuestionMarkAssign
        }.ToFrozenSet();

    public static bool IsBinaryOperator(this Token token) => BinaryOperators.Contains(token.Type);

    public static bool IsBitwiseOperator(this Token token) =>
        token.Type == TokenType.Ampersand || token.Type == TokenType.Pipe || token.Type == TokenType.Caret;

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

    public static bool IsGenericMatch(this Token token, Token other) =>
        token.Type == TokenType.LessThan && other.Type == TokenType.GreaterThan;

    public static bool IsEndOfStatement(this Token token) => token.Type == TokenType.Semicolon;

    public static bool IsEndOfFile(this Token token) => token.Type == TokenType.Eof;

    public static bool IsNone(this Token token) => token.Type == TokenType.None;
}