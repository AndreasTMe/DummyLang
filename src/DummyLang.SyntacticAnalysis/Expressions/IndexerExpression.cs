using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class IndexerExpression : IExpression
{
    public Token Identifier   { get; }
    public Token LeftBracket  { get; }
    public Token RightBracket { get; }

    // TODO: Multiple indices
    public IExpression? Index { get; }

    internal IndexerExpression(Token identifier, Token leftBracket, Token rightBracket, IExpression? index)
    {
        Identifier   = identifier;
        LeftBracket  = leftBracket;
        Index        = index;
        RightBracket = rightBracket;
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}