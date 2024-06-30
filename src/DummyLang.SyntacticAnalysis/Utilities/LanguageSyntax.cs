using DummyLang.LexicalAnalysis;
using System;
using System.Diagnostics.CodeAnalysis;

namespace DummyLang.SyntacticAnalysis.Utilities;

internal static class LanguageSyntax
{
    [DoesNotReturn]
    internal static void Expects(TokenType expected, Token found, string message) =>
        throw new LanguageSyntaxException(expected, found, message);
}

internal sealed class LanguageSyntaxException : Exception
{
    public TokenType Expected { get; }
    public Token     Found    { get; }

    public LanguageSyntaxException(TokenType expected, Token found, string message) : base(message)
    {
        Expected = expected;
        Found    = found;
    }
}