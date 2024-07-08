using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;
using DummyLang.SyntacticAnalysis.Expressions;
using System.Collections.Generic;

namespace DummyLang.SyntacticAnalysis.Statements;

public sealed class ReturnStatement : IStatement
{
    public Token                              ReturnKeyword   { get; }
    public Token                              Terminator      { get; }
    public IReadOnlyList<ArgumentExpression>? ReturnArguments { get; }

    internal ReturnStatement(Token returnKeyword,
                             Token terminator,
                             IReadOnlyList<ArgumentExpression>? returnArguments = null)
    {
        ReturnKeyword   = returnKeyword;
        Terminator      = terminator;
        ReturnArguments = returnArguments;
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}