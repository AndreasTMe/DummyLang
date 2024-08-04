using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;

namespace DummyLang.SyntacticAnalysis.Expressions;

public class ArgumentExpression : IExpression
{
    public IExpression? Argument { get; }
    public Token        Comma    { get; }

    public TokenPositions Positions { get; }

    internal ArgumentExpression(IExpression? argument, Token comma)
    {
        Argument = argument;
        Comma    = comma;

        Positions = new TokenPositions(argument?.Positions[0], comma.Position);
    }

    public virtual void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}