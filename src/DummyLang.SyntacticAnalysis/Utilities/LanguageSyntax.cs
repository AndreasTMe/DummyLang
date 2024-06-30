using DummyLang.LexicalAnalysis;
using System;
using System.Diagnostics.CodeAnalysis;

namespace DummyLang.SyntacticAnalysis.Utilities;

internal static class LanguageSyntax
{
    [DoesNotReturn]
    internal static void Found(Token token, string message) => throw new LanguageSyntaxException(token, message);

    [DoesNotReturn]
    internal static void Expects(TokenType expected, Token found, string message) =>
        throw new LanguageSyntaxException(expected, found, message);
}

internal sealed class LanguageSyntaxException : Exception
{
    public TokenType Expected { get; }
    public Token     Found    { get; }

    public LanguageSyntaxException(Token found, string message) : base(message)
    {
        Expected = TokenType.None;
        Found    = found;
    }

    public LanguageSyntaxException(TokenType expected, Token found, string message) : base(message)
    {
        Expected = expected;
        Found    = found;
    }
}