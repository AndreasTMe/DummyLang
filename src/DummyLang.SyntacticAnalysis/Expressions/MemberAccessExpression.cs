using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class MemberAccessExpression : PositionedNode, IExpression
{
    internal const string IdentifierExpected = "Identifier expected.";

    public TokenPosition Start => _start ??= TokenPosition.GetMin(Identifier?.Start, Access.Position, Member?.Start);
    public TokenPosition End   => _end ??= TokenPosition.GetMin(Identifier?.End, Access.Position, Member?.End);

    public IExpression? Identifier { get; }
    public Token        Access     { get; }
    public IExpression? Member     { get; }

    internal MemberAccessExpression(IExpression identifier, Token access, IExpression? member)
    {
        Identifier = identifier;
        Access     = access;
        Member     = member;
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}