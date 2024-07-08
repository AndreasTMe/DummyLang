﻿using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class UnexpectedTokenExpression : IExpression, ITypeExpression
{
    public Token Token { get; }

    internal UnexpectedTokenExpression(Token token) => Token = token;

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}