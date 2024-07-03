using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;
using DummyLang.SyntacticAnalysis.Expressions.Abstractions;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class IndexerExpression : IExpression
{
    public Token        Identifier   { get; }
    public Token        LeftBracket  { get; }
    public IExpression? Index        { get; }
    public Token        RightBracket { get; }

    internal IndexerExpression(Token identifier, Token leftBracket, Token rightBracket, IExpression? index)
    {
        Identifier   = identifier;
        LeftBracket  = leftBracket;
        Index        = index;
        RightBracket = rightBracket;
    }

    public void Accept(ISyntaxNodeVisitor visitor)
    {
        visitor.Visit(this);

        Index?.Accept(visitor);
    }
}