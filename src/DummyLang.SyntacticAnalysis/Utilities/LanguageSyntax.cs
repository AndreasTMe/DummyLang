using System;
using System.Diagnostics.CodeAnalysis;

namespace DummyLang.SyntacticAnalysis.Utilities;

internal static class LanguageSyntax
{
    [DoesNotReturn]
    internal static void Throw(string message) => throw new LanguageSyntaxException(message);
}

internal sealed class LanguageSyntaxException : Exception
{
    public LanguageSyntaxException(string message) : base(message)
    {
    }
}