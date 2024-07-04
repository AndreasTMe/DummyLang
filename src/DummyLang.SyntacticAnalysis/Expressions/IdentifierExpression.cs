using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class IdentifierExpression : IExpression
{
    public Token Token { get; }

    internal IdentifierExpression(Token token) => Token = token;

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}