using DummyLang.LexicalAnalysis;
using DummyLang.LexicalAnalysis.Extensions;
using System.Collections.Generic;

namespace DummyLang.SyntacticAnalysis.Utilities;

internal static class ParsingUtilities
{
    public static Token[] ReadAllTokens(in string source)
    {
        var tokenizer = new Tokenizer();
        tokenizer.Use(source);

        var tokens = new List<Token>();
        var token  = tokenizer.ReadNext();

        while (!token.IsEndOfFile())
        {
            tokens.Add(token);
            token = tokenizer.ReadNext();
        }

        tokens.Add(token);

        return tokens.ToArray();
    }

    public static bool TryGetBalancedBrackets(in Token[] tokens, int startIndex, out int endIndex)
    {
        endIndex = startIndex;

        if (endIndex >= tokens.Length)
            return false;

        var stack = new Stack<Token>();
        stack.Push(tokens[endIndex]);
        endIndex++;

        var reachedInvalidToken = false;
        while (endIndex < tokens.Length)
        {
            var currentToken = tokens[endIndex];

            // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
            switch (currentToken.Type)
            {
                case TokenType.LeftParenthesis:
                case TokenType.LeftBrace:
                case TokenType.LeftBracket:
                {
                    stack.Push(currentToken);
                    endIndex++;
                    continue;
                }
                case TokenType.RightParenthesis:
                case TokenType.RightBrace:
                case TokenType.RightBracket:
                {
                    if (stack.TryPeek(out var top)
                        && (top.IsParenthesisMatch(currentToken)
                            || top.IsBracketMatch(currentToken)
                            || top.IsBraceMatch(currentToken)))
                        stack.Pop();
                    else
                        reachedInvalidToken = true;

                    break;
                }
                case TokenType.Semicolon:
                    reachedInvalidToken = true;
                    break;
                default:
                    reachedInvalidToken = currentToken.IsReservedKeyword()
                                          || currentToken.IsEndOfStatement()
                                          || currentToken.IsEndOfFile()
                                          || currentToken.IsInvalid();
                    break;
            }

            if (reachedInvalidToken || stack.Count == 0)
                break;

            endIndex++;
        }

        return !reachedInvalidToken;
    }
}