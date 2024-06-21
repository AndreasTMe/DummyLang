namespace DummyLang.LexicalAnalysis.Extensions;

public static class TokenExtensions
{
    public static bool IsEof(this Token token) => token.Type == TokenType.Eof;

    public static bool IsInvalid(this Token token) => token.Type == TokenType.None;

    public static bool IsBinaryOperator(this Token token) =>
        token.Type == TokenType.Assign
        || token.Type == TokenType.Equal
        || token.Type == TokenType.NotEqual
        || token.Type == TokenType.Plus
        || token.Type == TokenType.Minus
        || token.Type == TokenType.Star
        || token.Type == TokenType.Slash
        || token.Type == TokenType.Percent
        || token.Type == TokenType.LessThan
        || token.Type == TokenType.LessThanOrEqual
        || token.Type == TokenType.LeftBitShift
        || token.Type == TokenType.GreaterThan
        || token.Type == TokenType.GreaterThanOrEqual
        || token.Type == TokenType.RightBitShift
        || token.Type == TokenType.Pipe
        || token.Type == TokenType.DoublePipe
        || token.Type == TokenType.Caret
        || token.Type == TokenType.Ampersand
        || token.Type == TokenType.DoubleAmpersand;
}