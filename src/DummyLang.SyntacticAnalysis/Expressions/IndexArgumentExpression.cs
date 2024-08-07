﻿using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class IndexArgumentExpression : ArgumentExpression
{
    internal IndexArgumentExpression(IExpression? argument, Token comma)
        : base(argument, comma)
    {
    }
}