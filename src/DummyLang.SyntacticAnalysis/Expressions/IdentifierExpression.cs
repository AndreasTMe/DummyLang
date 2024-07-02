using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Expressions.Abstractions;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class IdentifierExpression : IExpression
{
    public Token Token { get; }

    internal IdentifierExpression(Token token) => Token = token;
}