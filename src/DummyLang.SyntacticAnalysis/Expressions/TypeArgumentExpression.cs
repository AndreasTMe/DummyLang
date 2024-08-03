using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class TypeArgumentExpression : PositionedNode, ITypeExpression
{
    public TokenPosition Start => _start ??= TokenPosition.GetMin(Argument?.Start, Comma.Position);
    public TokenPosition End => _end ??= TokenPosition.GetMax(Argument?.End, Comma.Position);

    public ITypeExpression? Argument { get; }
    public Token            Comma    { get; }

    internal TypeArgumentExpression(ITypeExpression? argument, Token comma)
    {
        Argument = argument;
        Comma    = comma;
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}