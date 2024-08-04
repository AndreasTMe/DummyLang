using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class TypeParameterExpression : ITypeExpression
{
    public Token            Identifier { get; }
    public Token            Colon      { get; }
    public ITypeExpression? Type       { get; }
    public Token            Comma      { get; }

    public TokenPositions Positions { get; }

    internal TypeParameterExpression(Token identifier, Token colon, ITypeExpression? type, Token comma)
    {
        Identifier = identifier;
        Colon      = colon;
        Type       = type;
        Comma      = comma;

        Positions = new TokenPositions(identifier.Position, colon.Position, type?.Positions[0], comma.Position);
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}