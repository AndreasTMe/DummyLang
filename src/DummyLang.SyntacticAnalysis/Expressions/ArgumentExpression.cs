﻿using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;

namespace DummyLang.SyntacticAnalysis.Expressions;

public class ArgumentExpression : IExpression
{
    public IExpression? Argument { get; }
    public Token        Comma    { get; }

    internal ArgumentExpression(IExpression? argument, Token comma)
    {
        Argument = argument;
        Comma    = comma;
    }

    public virtual void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}