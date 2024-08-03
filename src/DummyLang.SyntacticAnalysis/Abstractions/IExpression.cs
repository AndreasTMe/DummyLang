using DummyLang.LexicalAnalysis;

namespace DummyLang.SyntacticAnalysis.Abstractions;

public interface IExpression : ISyntaxNode
{
    TokenPosition Start { get; }
    TokenPosition End   { get; }
}