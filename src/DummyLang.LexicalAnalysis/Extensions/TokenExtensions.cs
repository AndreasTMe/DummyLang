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
        || token.Type == TokenType.LessThan
        || token.Type == TokenType.LessThanOrEqual
        || token.Type == TokenType.LeftBitShift
        || token.Type == TokenType.GreaterThan
        || token.Type == TokenType.GreaterThanOrEqual
        || token.Type == TokenType.RightBitShift;

    public static bool IsIdentifier(this Token token) =>
        token.Type == TokenType.Identifier
        || token.Type == TokenType.Integer;

    public static bool IsKeyword(this Token token) =>
        token.Type == TokenType.Var
        || token.Type == TokenType.Const
        || token.Type == TokenType.Fun
        || token.Type == TokenType.If
        || token.Type == TokenType.Else
        || token.Type == TokenType.Break
        || token.Type == TokenType.While
        || token.Type == TokenType.Continue
        || token.Type == TokenType.Return;
}