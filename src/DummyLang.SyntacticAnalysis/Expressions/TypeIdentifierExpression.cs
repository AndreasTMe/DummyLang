using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class TypeIdentifierExpression : IExpression, ITypeExpression
{
    public Token Token { get; }

    public TokenPositions Positions { get; }

    internal TypeIdentifierExpression(Token token)
    {
        Token = token;

        Positions = new TokenPositions(token.Position);
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}