using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class TypeArgumentExpression : ITypeExpression
{
    public ITypeExpression? Argument { get; }
    public Token            Comma    { get; }

    internal TypeArgumentExpression(ITypeExpression? argument, Token comma)
    {
        Argument = argument;
        Comma    = comma;
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}