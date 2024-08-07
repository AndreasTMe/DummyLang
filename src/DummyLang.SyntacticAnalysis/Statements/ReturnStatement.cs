﻿using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;
using DummyLang.SyntacticAnalysis.Expressions;
using System.Collections.Generic;

namespace DummyLang.SyntacticAnalysis.Statements;

public sealed class ReturnStatement : IStatement
{
    internal const string SemicolonExpected    = "Semicolon expected at the end of a 'return' statement.";
    internal const string LastArgumentHasComma = "Last argument should not be followed by comma.";
    internal const string CommaExpected        = "Comma expected.";

    public Token                              ReturnKeyword   { get; }
    public IReadOnlyList<ArgumentExpression>? ReturnArguments { get; }
    public Token                              Terminator      { get; }

    public TokenPositions Positions { get; }

    internal ReturnStatement(Token returnKeyword,
                             Token terminator,
                             IReadOnlyList<ArgumentExpression>? returnArguments = null)
    {
        ReturnKeyword   = returnKeyword;
        Terminator      = terminator;
        ReturnArguments = returnArguments is { Count: > 0 } ? returnArguments : null;

        Positions = new TokenPositions(
            returnKeyword.Position,
            returnArguments?[0].Positions[0],
            terminator.Position);
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}