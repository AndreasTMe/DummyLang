using System.Diagnostics;

namespace DummyLang.LexicalAnalysis.Extensions;

internal static class TokenTypeExtensions
{
    internal static string GetStringToken(this TokenType type) =>
        type switch
        {
            TokenType.Comma => ",",
            TokenType.Dot => ".",
            TokenType.Semicolon => ";",
            TokenType.Colon => ":",
            TokenType.Assign => "=",
            TokenType.Bang => "!",
            TokenType.Equal => "==",
            TokenType.NotEqual => "!=",
            TokenType.Plus => "+",
            TokenType.PlusPlus => "++",
            TokenType.Minus => "-",
            TokenType.MinusMinus => "--",
            TokenType.Star => "*",
            TokenType.Slash => "/",
            TokenType.LessThan => "<",
            TokenType.LessThanOrEqual => "<=",
            TokenType.LeftBitShift => "<<",
            TokenType.GreaterThan => ">",
            TokenType.GreaterThanOrEqual => ">=",
            TokenType.RightBitShift => ">>",
            TokenType.LeftParen => "(",
            TokenType.RightParen => ")",
            TokenType.LeftBrace => "{",
            TokenType.RightBrace => "}",
            TokenType.LeftBracket => "[",
            TokenType.RightBracket => "]",
            _ => throw new UnreachableException("Unsupported token type.")
        };
}