using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;
using System.Collections.Generic;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class IndexerExpression : IExpression
{
    internal const string RightBracketExpected = "Right bracket token expected.";
    internal const string IndexerExpected      = "Indexer expected.";

    public Token                                   Identifier   { get; }
    public Token                                   LeftBracket  { get; }
    public IReadOnlyList<IndexArgumentExpression>? Indices      { get; }
    public Token                                   RightBracket { get; }

    public TokenPositions Positions { get; }

    internal IndexerExpression(Token identifier,
                               Token leftBracket,
                               Token rightBracket,
                               List<IndexArgumentExpression>? indices = null)
    {
        Identifier   = identifier;
        LeftBracket  = leftBracket;
        Indices      = indices is { Count: > 0 } ? indices : null;
        RightBracket = rightBracket;

        Positions = new TokenPositions(
            identifier.Position,
            leftBracket.Position,
            indices?[0].Positions[0],
            rightBracket.Position);
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}