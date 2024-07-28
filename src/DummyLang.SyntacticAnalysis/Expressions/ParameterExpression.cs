using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;

namespace DummyLang.SyntacticAnalysis.Expressions;

public class ParameterExpression : IExpression
{
    public Token Parameter { get; }
    public Token Comma     { get; }

    internal ParameterExpression(Token parameter, Token comma)
    {
        Parameter = parameter;
        Comma     = comma;
    }

    public virtual void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}