using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;

namespace DummyLang.SyntacticAnalysis.Expressions;

public class ArgumentExpression : PositionedNode, IExpression
{
    public TokenPosition Start => _start ??= TokenPosition.GetMin(Argument?.Start, Comma.Position);
    public TokenPosition End   => _end ??= TokenPosition.GetMax(Argument?.End, Comma.Position);

    public IExpression? Argument { get; }
    public Token        Comma    { get; }

    internal ArgumentExpression(IExpression? argument, Token comma)
    {
        Argument = argument;
        Comma    = comma;
    }

    public virtual void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}