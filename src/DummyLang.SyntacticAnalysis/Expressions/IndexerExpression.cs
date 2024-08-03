using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;
using System.Collections.Generic;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class IndexerExpression : PositionedNode, IExpression
{
    internal const string RightBracketExpected = "Right bracket token expected.";
    internal const string IndexerExpected      = "Indexer expected.";

    public TokenPosition Start =>
        _start ??= TokenPosition.GetMin(
            Identifier.Position,
            LeftBracket.Position,
            RightBracket.Position,
            Indices?[0].Start);

    public TokenPosition End =>
        _end ??= TokenPosition.GetMax(
            Identifier.Position,
            LeftBracket.Position,
            RightBracket.Position,
            Indices?[^1].End);

    public Token                                   Identifier   { get; }
    public Token                                   LeftBracket  { get; }
    public Token                                   RightBracket { get; }
    public IReadOnlyList<IndexArgumentExpression>? Indices      { get; }

    internal IndexerExpression(Token identifier,
                               Token leftBracket,
                               Token rightBracket,
                               List<IndexArgumentExpression>? indices)
    {
        Identifier   = identifier;
        LeftBracket  = leftBracket;
        RightBracket = rightBracket;
        Indices      = indices is { Count: > 0 } ? indices : null;
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}