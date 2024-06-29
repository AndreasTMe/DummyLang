using DummyLang.LexicalAnalysis;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class TypeIdentifierExpression : Expression
{
    public Token[] Tokens { get; }

    public TypeIdentifierExpression(params Token[] tokens) => Tokens = tokens;
}