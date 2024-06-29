using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Utilities;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class NumberLiteralExpression : Expression
{
    public Token      NumberToken { get; }
    public NumberType Type        { get; }

    internal NumberLiteralExpression(Token numberToken, NumberType type)
    {
        NumberToken = numberToken;
        Type        = type;
    }
}