using System.Diagnostics;

namespace DummyLang.LexicalAnalysis.Extensions;

internal static class TokenTypeExtensions
{
    internal static string GetStringToken(this TokenType type) =>
        type switch
        {
            TokenType.Comma                    => ",",
            TokenType.Dot                      => ".",
            TokenType.Semicolon                => ";",
            TokenType.Colon                    => ":",
            TokenType.Assign                   => "=",
            TokenType.AssignLambda             => "=>",
            TokenType.Bang                     => "!",
            TokenType.Equal                    => "==",
            TokenType.NotEqual                 => "!=",
            TokenType.Plus                     => "+",
            TokenType.PlusPlus                 => "++",
            TokenType.PlusAssign               => "+=",
            TokenType.Minus                    => "-",
            TokenType.MinusMinus               => "--",
            TokenType.MinusAssign              => "-=",
            TokenType.PointerAccess            => "->",
            TokenType.Star                     => "*",
            TokenType.StarAssign               => "*=",
            TokenType.Slash                    => "/",
            TokenType.SlashAssign              => "/=",
            TokenType.Percent                  => "%",
            TokenType.PercentAssign            => "%=",
            TokenType.Ampersand                => "&",
            TokenType.DoubleAmpersand          => "&&",
            TokenType.AmpersandAssign          => "&=",
            TokenType.Pipe                     => "|",
            TokenType.DoublePipe               => "||",
            TokenType.PipeAssign               => "|=",
            TokenType.Tilde                    => "~",
            TokenType.Caret                    => "^",
            TokenType.CaretAssign              => "^=",
            TokenType.LessThan                 => "<",
            TokenType.LessThanOrEqual          => "<=",
            TokenType.GreaterThan              => ">",
            TokenType.GreaterThanOrEqual       => ">=",
            TokenType.LeftBitShift             => "<<",
            TokenType.LeftBitShiftAssign       => "<<=",
            TokenType.RightBitShift            => ">>",
            TokenType.RightBitShiftAssign      => ">>=",
            TokenType.DoubleDot                => "..",
            TokenType.QuestionMark             => "?",
            TokenType.DoubleQuestionMark       => "??",
            TokenType.DoubleQuestionMarkAssign => "??=",
            TokenType.LeftParenthesis          => "(",
            TokenType.RightParenthesis         => ")",
            TokenType.LeftBrace                => "{",
            TokenType.RightBrace               => "}",
            TokenType.LeftBracket              => "[",
            TokenType.RightBracket             => "]",
            _                                  => throw new UnreachableException("Unsupported token type.")
        };
}