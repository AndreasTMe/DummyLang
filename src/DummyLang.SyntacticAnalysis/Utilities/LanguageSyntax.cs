using DummyLang.LexicalAnalysis;
using System;
using System.Diagnostics.CodeAnalysis;

namespace DummyLang.SyntacticAnalysis.Utilities;

internal static class LanguageSyntax
{
    [DoesNotReturn]
    internal static void Throw(string message) => throw new LanguageSyntaxException(message);

    [DoesNotReturn]
    internal static void Found(Token token, string message) => throw new LanguageSyntaxException(token, message);

    [DoesNotReturn]
    internal static void Expects(TokenType expected, Token found, string message) =>
        throw new LanguageSyntaxException(expected, found, message);
}

internal sealed class LanguageSyntaxException : Exception
{
    public TokenType Expected { get; } = TokenType.None;
    public Token     Found    { get; } = Token.None;

    public LanguageSyntaxException(string message) : base(message)
    {
    }

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