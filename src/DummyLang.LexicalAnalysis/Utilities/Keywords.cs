using System.Collections.Generic;

namespace DummyLang.LexicalAnalysis.Utilities;

internal static class Keywords
{
    internal static readonly Dictionary<string, TokenType> Tokens = new()
    {
        { Var, TokenType.Var },
        { Const, TokenType.Const },
        { Fun, TokenType.Fun },
        { If, TokenType.If },
        { Else, TokenType.Else },
        { Break, TokenType.Break },
        { While, TokenType.While },
        { Continue, TokenType.Continue },
        { Return, TokenType.Return },
        { As, TokenType.As },
        { True, TokenType.Boolean },
        { False, TokenType.Boolean },
        { Null, TokenType.Null },
        { Default, TokenType.Default }
    };

    private const string Var = "var";
    private const string Const = "const";
    private const string Fun = "fun";
    private const string If = "if";
    private const string Else = "else";
    private const string Break = "break";
    private const string While = "while";
    private const string Continue = "continue";
    private const string Return = "return";
    private const string As = "as";
    private const string True = "true";
    private const string False = "false";
    private const string Null = "null";
    private const string Default = "default";
}