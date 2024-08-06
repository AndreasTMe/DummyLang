using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;
using System.Collections.Generic;

namespace DummyLang.SyntacticAnalysis.Statements;

public sealed class CompoundStatement : IStatement
{
    internal const string RightBraceExpected = "Right brace expected at the end of a block statement.";

    public Token                      LeftBrace  { get; }
    public IReadOnlyList<IStatement>? Statements { get; }
    public Token                      RightBrace { get; }

    public TokenPositions Positions { get; }

    internal CompoundStatement(Token leftBrace, Token rightBrace, IReadOnlyList<IStatement>? statements = null)
    {
        LeftBrace  = leftBrace;
        RightBrace = rightBrace;
        Statements = statements is { Count: > 0 } ? statements : null;

        Positions = new TokenPositions(
            leftBrace.Position,
            statements?[0].Positions[0],
            rightBrace.Position);
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}