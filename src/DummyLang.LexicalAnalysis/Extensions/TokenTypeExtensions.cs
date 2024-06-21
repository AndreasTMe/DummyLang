using System.Diagnostics;

namespace DummyLang.LexicalAnalysis.Extensions;

internal static class TokenTypeExtensions
{
    internal static string GetStringToken(this TokenType type) =>
        type switch
        {
            TokenType.Comma => ",",
            TokenType.Dot => ".",
            TokenType.DoubleDot => "..",
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
            TokenType.PointerAccess => "->",
            TokenType.Star => "*",
            TokenType.Slash => "/",
            TokenType.Percent => "%",
            TokenType.Ampersand => "&",
            TokenType.DoubleAmpersand => "&&",
            TokenType.Pipe => "|",
            TokenType.DoublePipe => "||",
            TokenType.Caret => "^",
            TokenType.QuestionMark => "?",
            TokenType.DoubleQuestionMark => "??",
            TokenType.LessThan => "<",
            TokenType.LessThanOrEqual => "<=",
            TokenType.LeftBitShift => "<<",
            TokenType.GreaterThan => ">",
            TokenType.GreaterThanOrEqual => ">=",
            TokenType.RightBitShift => ">>",
            TokenType.Tilde => "~",
            TokenType.LeftParen => "(",
            TokenType.RightParen => ")",
            TokenType.LeftBrace => "{",
            TokenType.RightBrace => "}",
            TokenType.LeftBracket => "[",
            TokenType.RightBracket => "]",
            _ => throw new UnreachableException("Unsupported token type.")
        };
}