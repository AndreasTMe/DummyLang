using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Expressions.Abstractions;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class TypeIdentifierExpression : IExpression, ITypeExpression
{
    public Token Token { get; }

    internal TypeIdentifierExpression(Token token) => Token = token;
}