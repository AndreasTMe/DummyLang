using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class ParameterExpression : PositionedNode, IExpression
{
    public TokenPosition Start => _start ??= TokenPosition.GetMin(Parameter.Position, Comma.Position);
    public TokenPosition End   => _end ??= TokenPosition.GetMax(Parameter.Position, Comma.Position);

    public Token Parameter { get; }
    public Token Comma     { get; }

    internal ParameterExpression(Token parameter, Token comma)
    {
        Parameter = parameter;
        Comma     = comma;
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}