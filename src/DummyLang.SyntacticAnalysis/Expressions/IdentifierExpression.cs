using DummyLang.LexicalAnalysis;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class IdentifierExpression : Expression
{
    public Token Token { get; }

    public IdentifierExpression(Token token) => Token = token;
}