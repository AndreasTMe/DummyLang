using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class ParameterExpression : IExpression
{
    public Token Parameter { get; }
    public Token Comma     { get; }

    public TokenPositions Positions { get; }

    internal ParameterExpression(Token parameter, Token comma)
    {
        Parameter = parameter;
        Comma     = comma;

        Positions = new TokenPositions(parameter.Position, comma.Position);
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}