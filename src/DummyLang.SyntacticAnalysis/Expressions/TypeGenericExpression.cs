using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;
using System.Collections.Generic;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class TypeGenericExpression : IExpression, ITypeExpression
{
    public Token                 Identifier     { get; }
    public Token                 LessThan       { get; }
    public Token                 GreaterThan    { get; }
    public List<ITypeExpression> TypeParameters { get; }
    public List<Token>           Commas         { get; }

    internal TypeGenericExpression(Token identifier,
                                   Token lessThan,
                                   Token greaterThan,
                                   List<ITypeExpression> typeParameters,
                                   List<Token> commas)
    {
        Identifier     = identifier;
        LessThan       = lessThan;
        GreaterThan    = greaterThan;
        TypeParameters = typeParameters;
        Commas         = commas;
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}