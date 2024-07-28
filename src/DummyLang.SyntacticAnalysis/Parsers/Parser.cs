using DummyLang.LexicalAnalysis;
using System.Runtime.CompilerServices;

namespace DummyLang.SyntacticAnalysis.Parsers;

public static class Parser
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Token GetAndMoveToNext(ref int index, in Token[] tokens) =>
        index >= 0 && index < tokens.Length ? tokens[index++] : Token.None;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Token GetAndMoveToNextOrDefault(TokenType expected, ref int index, in Token[] tokens) =>
        TypeAt(index, in tokens) == expected ? tokens[index++] : Token.None;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TokenType TypeAt(int index, in Token[] tokens) =>
        index >= 0 && index < tokens.Length ? tokens[index].Type : TokenType.None;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Token TokenAt(int index, in Token[] tokens) =>
        index >= 0 && index < tokens.Length ? tokens[index] : Token.None;
}