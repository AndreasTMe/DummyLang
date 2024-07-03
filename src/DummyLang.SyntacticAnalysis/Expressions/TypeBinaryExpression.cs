using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;
using DummyLang.SyntacticAnalysis.Expressions.Abstractions;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class TypeBinaryExpression : IExpression, ITypeExpression
{
    public ITypeExpression Left     { get; }
    public Token           Operator { get; }
    public ITypeExpression Right    { get; }

    internal TypeBinaryExpression(ITypeExpression left, Token @operator, ITypeExpression right)
    {
        Left     = left;
        Operator = @operator;
        Right    = right;
    }

    public void Accept(ISyntaxNodeVisitor visitor)
    {
        visitor.Visit(this);

        Left.Accept(visitor);
        Right.Accept(visitor);
    }
}