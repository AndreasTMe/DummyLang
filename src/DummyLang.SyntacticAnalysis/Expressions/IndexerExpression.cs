using DummyLang.LexicalAnalysis;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class IndexerExpression : Expression
{
    public Token       Identifier   { get; }
    public Token       LeftBracket  { get; }
    public Expression? Index        { get; }
    public Token       RightBracket { get; }

    public IndexerExpression(Token identifier, Token leftBracket, Token rightBracket, Expression? index)
    {
        Identifier   = identifier;
        LeftBracket  = leftBracket;
        Index        = index;
        RightBracket = rightBracket;
    }
}