using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Expressions;
using DummyLang.SyntacticAnalysis.Utilities;
using Xunit;

namespace DummyLang.SyntacticAnalysis.Tests;

public class NumberSyntaxParserTests
{
    [Fact]
    public void GenerateSyntax_Numbers_Binary()
    {
        // Arrange
        // Act
        var parser = new SyntaxParser();
        var syntaxTree = parser.Feed("0b101")
            .GenerateSyntax();

        // Assert
        Assert.NotNull(syntaxTree);
        Assert.Equal(1, syntaxTree.Nodes.Count);
        Assert.IsType<NumberLiteralExpression>(syntaxTree.Nodes[0]);

        var expression = (NumberLiteralExpression)syntaxTree.Nodes[0];
        Assert.Equal(NumberType.Binary, expression.Type);
    }

    [Fact]
    public void GenerateSyntax_Numbers_Hexadecimal()
    {
        // Arrange
        // Act
        var parser = new SyntaxParser();
        var syntaxTree = parser.Feed("0x0123456789abcdef")
            .GenerateSyntax();

        // Assert
        Assert.NotNull(syntaxTree);
        Assert.Equal(1, syntaxTree.Nodes.Count);
        Assert.IsType<NumberLiteralExpression>(syntaxTree.Nodes[0]);

        var expression = (NumberLiteralExpression)syntaxTree.Nodes[0];
        Assert.Equal(NumberType.Hexadecimal, expression.Type);
    }

    [Fact]
    public void GenerateSyntax_Numbers_Unsigned()
    {
        // Arrange
        // Act
        var parser = new SyntaxParser();
        var syntaxTree = parser.Feed("123u")
            .GenerateSyntax();

        // Assert
        Assert.NotNull(syntaxTree);
        Assert.Equal(1, syntaxTree.Nodes.Count);
        Assert.IsType<NumberLiteralExpression>(syntaxTree.Nodes[0]);

        var expression = (NumberLiteralExpression)syntaxTree.Nodes[0];
        Assert.Equal(NumberType.UnsignedInteger, expression.Type);
    }

    [Fact]
    public void GenerateSyntax_Numbers_Long()
    {
        // Arrange
        // Act
        var parser = new SyntaxParser();
        var syntaxTree = parser.Feed("123l")
            .GenerateSyntax();

        // Assert
        Assert.NotNull(syntaxTree);
        Assert.Equal(1, syntaxTree.Nodes.Count);
        Assert.IsType<NumberLiteralExpression>(syntaxTree.Nodes[0]);

        var expression = (NumberLiteralExpression)syntaxTree.Nodes[0];
        Assert.Equal(NumberType.Long, expression.Type);
    }

    [Fact]
    public void GenerateSyntax_Numbers_UnsignedLong()
    {
        // Arrange
        // Act
        var parser = new SyntaxParser();
        var syntaxTree = parser.Feed("123ul")
            .GenerateSyntax();

        // Assert
        Assert.NotNull(syntaxTree);
        Assert.Equal(1, syntaxTree.Nodes.Count);
        Assert.IsType<NumberLiteralExpression>(syntaxTree.Nodes[0]);

        var expression = (NumberLiteralExpression)syntaxTree.Nodes[0];
        Assert.Equal(NumberType.UnsignedLong, expression.Type);
    }

    [Fact]
    public void GenerateSyntax_Numbers_Float()
    {
        // Arrange
        // Act
        var parser = new SyntaxParser();
        var syntaxTree = parser.Feed("123.123f")
            .GenerateSyntax();

        // Assert
        Assert.NotNull(syntaxTree);
        Assert.Equal(1, syntaxTree.Nodes.Count);
        Assert.IsType<NumberLiteralExpression>(syntaxTree.Nodes[0]);

        var expression = (NumberLiteralExpression)syntaxTree.Nodes[0];
        Assert.Equal(NumberType.Float, expression.Type);
    }

    [Fact]
    public void GenerateSyntax_Numbers_Double()
    {
        // Arrange
        // Act
        var parser = new SyntaxParser();
        var syntaxTree = parser.Feed("123.123d")
            .GenerateSyntax();

        // Assert
        Assert.NotNull(syntaxTree);
        Assert.Equal(1, syntaxTree.Nodes.Count);
        Assert.IsType<NumberLiteralExpression>(syntaxTree.Nodes[0]);

        var expression = (NumberLiteralExpression)syntaxTree.Nodes[0];
        Assert.Equal(NumberType.Double, expression.Type);
    }

    [Fact]
    public void GenerateSyntax_Numbers_Decimal()
    {
        // Arrange
        // Act
        var parser = new SyntaxParser();
        var syntaxTree = parser.Feed("123.123m")
            .GenerateSyntax();

        // Assert
        Assert.NotNull(syntaxTree);
        Assert.Equal(1, syntaxTree.Nodes.Count);
        Assert.IsType<NumberLiteralExpression>(syntaxTree.Nodes[0]);

        var expression = (NumberLiteralExpression)syntaxTree.Nodes[0];
        Assert.Equal(NumberType.Decimal, expression.Type);
    }

    [Fact]
    public void GenerateSyntax_Numbers_WithExponent()
    {
        // Arrange
        // Act
        var parser = new SyntaxParser();
        var syntaxTree = parser.Feed("123.12e-10")
            .GenerateSyntax();

        // Assert
        Assert.NotNull(syntaxTree);
        Assert.Equal(1, syntaxTree.Nodes.Count);
        Assert.IsType<NumberLiteralExpression>(syntaxTree.Nodes[0]);

        var expression = (NumberLiteralExpression)syntaxTree.Nodes[0];
        Assert.Equal(NumberType.WithExponent, expression.Type);
    }

    [Fact]
    public void GenerateSyntax_Numbers_RealShouldHaveDot()
    {
        // Arrange
        // Act
        var parser = new SyntaxParser();
        var syntaxTree = parser.Feed("123f")
            .GenerateSyntax();

        // Assert
        Assert.NotNull(syntaxTree);
        Assert.Equal(2, syntaxTree.Nodes.Count);
        Assert.IsType<NumberLiteralExpression>(syntaxTree.Nodes[0]);
        Assert.Equal(NumberType.Integer, ((NumberLiteralExpression)syntaxTree.Nodes[0]).Type);
        Assert.IsType<IdentifierExpression>(syntaxTree.Nodes[1]);

        // Act again
        syntaxTree = parser.Feed("123e10")
            .GenerateSyntax();

        // Assert
        Assert.NotNull(syntaxTree);
        Assert.Equal(2, syntaxTree.Nodes.Count);
        Assert.IsType<NumberLiteralExpression>(syntaxTree.Nodes[0]);
        Assert.Equal(NumberType.Integer, ((NumberLiteralExpression)syntaxTree.Nodes[0]).Type);
        Assert.IsType<IdentifierExpression>(syntaxTree.Nodes[1]);
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