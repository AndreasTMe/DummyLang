﻿using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;
using DummyLang.SyntacticAnalysis.Statements.Abstractions;

namespace DummyLang.SyntacticAnalysis.Statements;

public sealed class CompoundStatement : IStatement
{
    public Token        LeftBrace  { get; }
    public IStatement[] Statements { get; }
    public Token        RightBrace { get; }

    internal CompoundStatement(Token leftBrace, IStatement[] statements, Token rightBrace)
    {
        LeftBrace  = leftBrace;
        Statements = statements;
        RightBrace = rightBrace;
    }

    public void Accept(ISyntaxNodeVisitor visitor)
    {
        visitor.Visit(this);

        foreach (var statement in Statements)
            statement.Accept(visitor);
    }
}