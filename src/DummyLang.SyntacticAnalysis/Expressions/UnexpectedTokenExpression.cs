using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class UnexpectedTokenExpression : IExpression, ITypeExpression
{
    internal const string UnexpectedToken = "Unexpected token.";

    public Token Token { get; }

    public TokenPositions Positions { get; }

    internal UnexpectedTokenExpression(Token token)
    {
        Token = token;

        Positions = new TokenPositions(token.Position);
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}