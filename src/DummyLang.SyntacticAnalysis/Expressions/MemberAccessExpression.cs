using DummyLang.LexicalAnalysis;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class MemberAccessExpression : Expression
{
    public Expression Identifier { get; }
    public Token      Access     { get; }
    public Expression Member     { get; }

    internal MemberAccessExpression(Expression identifier, Token access, Expression member)
    {
        Identifier = identifier;
        Access     = access;
        Member     = member;
    }
}