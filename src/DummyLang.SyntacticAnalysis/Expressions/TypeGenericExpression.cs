using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;
using System.Collections.Generic;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class TypeGenericExpression : IExpression, ITypeExpression
{
    public Token                                  Identifier    { get; }
    public Token                                  LessThan      { get; }
    public Token                                  GreaterThan   { get; }
    public IReadOnlyList<TypeArgumentExpression>? TypeArguments { get; }

    internal TypeGenericExpression(Token identifier,
                                   Token lessThan,
                                   Token greaterThan,
                                   List<TypeArgumentExpression>? typeArguments = null)
    {
        Identifier    = identifier;
        LessThan      = lessThan;
        GreaterThan   = greaterThan;
        TypeArguments = typeArguments;
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}