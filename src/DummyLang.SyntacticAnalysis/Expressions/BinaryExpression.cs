using DummyLang.LexicalAnalysis;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class BinaryExpression : Expression
{
    public SyntaxTree.Node Left     { get; }
    public Token           Operator { get; }
    public SyntaxTree.Node Right    { get; }

    internal BinaryExpression(SyntaxTree.Node left, Token @operator, SyntaxTree.Node right)
    {
        Operator = @operator;
        Left     = left;
        Right    = right;
    }
}