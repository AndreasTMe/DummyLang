using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class IdentifierExpression : IExpression
{
    public Token Token { get; }

    public TokenPositions Positions { get; }

    internal IdentifierExpression(Token token)
    {
        Token = token;

        Positions = new TokenPositions(token.Position);
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}