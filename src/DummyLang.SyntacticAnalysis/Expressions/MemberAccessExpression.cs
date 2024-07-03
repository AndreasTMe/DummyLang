using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;
using DummyLang.SyntacticAnalysis.Expressions.Abstractions;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class MemberAccessExpression : IExpression
{
    public IExpression Identifier { get; }
    public Token       Access     { get; }
    public IExpression Member     { get; }

    internal MemberAccessExpression(IExpression identifier, Token access, IExpression member)
    {
        Identifier = identifier;
        Access     = access;
        Member     = member;
    }

    public void Accept(ISyntaxNodeVisitor visitor)
    {
        visitor.Visit(this);

        Identifier.Accept(visitor);
        Member.Accept(visitor);
    }
}