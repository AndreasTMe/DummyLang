namespace DummyLang.LexicalAnalysis.Extensions;

public static class TokenExtensions
{
    public static bool IsEof(this Token token) => token.Type == TokenType.Eof;

    public static bool IsInvalid(this Token token) => token.Type == TokenType.None;

    public static bool IsBinaryOperator(this Token token) =>
        token.Type == TokenType.Plus
        || token.Type == TokenType.Minus
        || token.Type == TokenType.Star
        || token.Type == TokenType.Slash;
}