namespace DummyLang.SyntacticAnalysis.Abstractions;

public interface IExpression : ISyntaxNode, ISyntaxNodeWithPositions;

public interface ITypeExpression : ISyntaxNode, ISyntaxNodeWithPositions;