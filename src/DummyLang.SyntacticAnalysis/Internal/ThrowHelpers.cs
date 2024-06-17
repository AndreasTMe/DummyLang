using System;
using System.Diagnostics.CodeAnalysis;

namespace DummyLang.SyntacticAnalysis.Internal;

internal static class ThrowHelpers
{
    [DoesNotReturn]
    public static void NotBinaryOperator() => throw new ArgumentException("Token is not a binary operator.");
}