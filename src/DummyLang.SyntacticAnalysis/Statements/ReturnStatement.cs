using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;
using System.Collections.Generic;

namespace DummyLang.SyntacticAnalysis.Statements;

public sealed class ReturnStatement : IStatement
{
    public Token                       ReturnKeyword { get; }
    public Token                       Terminator    { get; }
    public IReadOnlyList<IExpression>? Expressions   { get; }

    internal ReturnStatement(Token returnKeyword, Token terminator, IReadOnlyList<IExpression>? expressions)
    {
        ReturnKeyword = returnKeyword;
        Terminator    = terminator;
        Expressions   = expressions;
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}