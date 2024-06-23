using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Expressions;
using DummyLang.SyntacticAnalysis.Utilities;
using Xunit;

namespace DummyLang.SyntacticAnalysis.Tests;

public partial class SyntaxParserTests
{
    [Fact]
    public void Feed_SimpleExpression_ShouldReadThreeTokens()
    {
        // Arrange
        const string source = "1 + 2";

        // Act
        var parser = new SyntaxParser();
        parser.Feed(source);

        // Assert
        Assert.Equal(4, parser.TotalTokens);
        Assert.Equal(new Token(TokenType.Integer, "1", TokenPosition.At(1, 1)), parser.Current);
    }

    [Fact]
    public void GenerateSyntax_Expressions_SimpleBinaryExpression()
    {
        // Arrange
        const string source = "1 + 2";

        // Act
        var parser = new SyntaxParser();
        var syntaxTree = parser.Feed(source)
            .GenerateSyntax();

        // Assert
        Assert.NotNull(syntaxTree);
        Assert.Equal(1, syntaxTree.Nodes.Count);
        Assert.IsType<BinaryExpression>(syntaxTree.Nodes[0]);

        var expression = (BinaryExpression)syntaxTree.Nodes[0];
        Assert.IsType<NumberLiteralExpression>(expression.Left);
        Assert.IsType<NumberLiteralExpression>(expression.Right);
        Assert.Equal(TokenType.Plus, expression.Operator.Type);
    }
    
    [Fact]
    public void GenerateSyntax_Expressions_MoreComplexMathTree()
    {
        // Arrange
        const string source = "0b1010 + 2.0e-10f * (-3L - foo++)";

        // Act
        var parser = new SyntaxParser();
        var syntaxTree = parser.Feed(source)
            .GenerateSyntax();

        // Assert
        // addition = 0b1010 + multiplication
        Assert.NotNull(syntaxTree);
        Assert.Equal(1, syntaxTree.Nodes.Count);
        Assert.IsType<BinaryExpression>(syntaxTree.Nodes[0]);
        var expression = (BinaryExpression)syntaxTree.Nodes[0];
        Assert.Equal(TokenType.Plus, expression.Operator.Type);
        
        Assert.IsType<NumberLiteralExpression>(expression.Left);
        var binary = (NumberLiteralExpression)expression.Left;
        Assert.Equal(NumberType.Binary, binary.Type);
        Assert.Equal("0b1010", binary.NumberToken.Value);
        
        // multiplication = 2.0e-10f * parenthesised
        Assert.IsType<BinaryExpression>(expression.Right);
        var multiplication = (BinaryExpression)expression.Right;
        Assert.Equal(TokenType.Star, multiplication.Operator.Type);
        
        Assert.IsType<NumberLiteralExpression>(multiplication.Left);
        var real = (NumberLiteralExpression)multiplication.Left;
        Assert.Equal(NumberType.Float, real.Type);
        Assert.Equal("2.0e-10f", real.NumberToken.Value);
        
        // parenthesised = ( subtraction )
        Assert.IsType<ParenthesisedExpression>(multiplication.Right);
        var parenthesised = (ParenthesisedExpression)multiplication.Right;
        Assert.Equal(TokenType.LeftParen, parenthesised.LeftParen.Type);
        Assert.Equal(TokenType.RightParen, parenthesised.RightParen.Type);
        
        // subtraction = unary - primary
        Assert.IsType<BinaryExpression>(parenthesised.Expression);
        var subtraction = (BinaryExpression)parenthesised.Expression;
        Assert.Equal(TokenType.Minus, subtraction.Operator.Type);
        
        // unary = -3L
        Assert.IsType<UnaryExpression>(subtraction.Left);
        var unary = (UnaryExpression)subtraction.Left;
        Assert.Equal(TokenType.Minus, unary.Token.Type);
        
        Assert.IsType<NumberLiteralExpression>(unary.Expression);
        var longInteger = (NumberLiteralExpression)unary.Expression;
        Assert.Equal(NumberType.Long, longInteger.Type);
        Assert.Equal("3L", longInteger.NumberToken.Value);
        
        // primary = foo++
        Assert.IsType<PrimaryExpression>(subtraction.Right);
        var primary = (PrimaryExpression)subtraction.Right;
        Assert.Equal(TokenType.PlusPlus, primary.Token.Type);
        
        Assert.IsType<IdentifierExpression>(primary.Expression);
        var identifier = (IdentifierExpression)primary.Expression;
        Assert.Equal(TokenType.Identifier, identifier.Token.Type);
        Assert.Equal("foo", identifier.Token.Value);
    }
}