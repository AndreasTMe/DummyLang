using DummyLang.LexicalAnalysis;

namespace DummyLang.SyntacticAnalysis.Abstractions;

public interface IExpression : ISyntaxNode
{
    TokenPositions Positions { get; }
}