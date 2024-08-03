using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class TypeParameterExpression : PositionedNode, ITypeExpression
{
    public TokenPosition Start =>
        _start ??= TokenPosition.GetMin(
            Identifier.Position,
            Colon.Position,
            Type?.Start,
            Comma.Position);

    public TokenPosition End =>
        _end ??= TokenPosition.GetMin(
            Identifier.Position,
            Colon.Position,
            Type?.End,
            Comma.Position);

    public Token            Identifier { get; }
    public Token            Colon      { get; }
    public ITypeExpression? Type       { get; }
    public Token            Comma      { get; }

    internal TypeParameterExpression(Token identifier, Token colon, ITypeExpression? type, Token comma)
    {
        Identifier = identifier;
        Colon      = colon;
        Type       = type;
        Comma      = comma;
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}