using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;
using System.Collections.Generic;

namespace DummyLang.SyntacticAnalysis.Statements;

public sealed class CompoundStatement : IStatement
{
    public Token                      LeftBrace  { get; }
    public Token                      RightBrace { get; }
    public IReadOnlyList<IStatement>? Statements { get; }

    internal CompoundStatement(Token leftBrace, Token rightBrace, IReadOnlyList<IStatement>? statements = null)
    {
        LeftBrace  = leftBrace;
        RightBrace = rightBrace;
        Statements = statements is { Count: > 0 } ? statements : null;
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}