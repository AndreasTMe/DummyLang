using DummyLang.LexicalAnalysis;

namespace DummyLang.SyntacticAnalysis.Abstractions;

public interface ITypeExpression : ISyntaxNode
{
    TokenPosition Start { get; }
    TokenPosition End   { get; }
}