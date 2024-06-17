using DummyLang.LexicalAnalysis;
using DummyLang.LexicalAnalysis.Extensions;
using DummyLang.SyntacticAnalysis.Internal;
using DummyLang.SyntacticAnalysis.Utilities;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class BinaryExpression : Expression
{
    public override SyntaxNodeType Type => SyntaxNodeType.BinaryExpression;

    public SyntaxTree.Node Left { get; }
    public Token Operator { get; }
    public SyntaxTree.Node Right { get; }

    public BinaryExpression(SyntaxTree.Node left, Token @operator, SyntaxTree.Node right)
    {
        if (!@operator.IsBinaryOperator())
        {
            ThrowHelpers.NotBinaryOperator();
        }
        
        Operator = @operator;
        Left = left;
        Right = right;
    }
    
    public override void PrettyPrint(int indent)
    {
        ConsoleUtilities.WriteLineFormatted(nameof(BinaryExpression), indent);
     
        Left.PrettyPrint(indent + 1);
        ConsoleUtilities.WriteLineFormatted(Operator.Type.ToString(), indent + 1);
        Right.PrettyPrint(indent + 1);
    }
}