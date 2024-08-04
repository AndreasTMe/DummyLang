using DummyLang.LexicalAnalysis;

namespace DummyLang.SyntacticAnalysis.Abstractions;

public interface ITypeExpression : ISyntaxNode
{
    TokenPositions Positions { get; }
}