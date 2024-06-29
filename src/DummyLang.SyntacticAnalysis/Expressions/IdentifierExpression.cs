using DummyLang.LexicalAnalysis;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class IdentifierExpression : Expression
{
    public Token Token { get; }

    internal IdentifierExpression(Token token) => Token = token;
}