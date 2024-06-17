using System;
using System.Diagnostics.CodeAnalysis;

namespace DummyLang.SyntacticAnalysis.Internal;

internal static class ThrowHelpers
{
    [DoesNotReturn]
    public static void NotBinaryOperator() => throw new ArgumentException("Token is not a binary operator.");

    [DoesNotReturn]
    public static void NotLeftParenthesisOperator()
        => throw new ArgumentException("Token is not a left parenthesis operator.");
    
    [DoesNotReturn]
    public static void NotRightParenthesisOperator()
        => throw new ArgumentException("Token is not a right parenthesis operator.");
}