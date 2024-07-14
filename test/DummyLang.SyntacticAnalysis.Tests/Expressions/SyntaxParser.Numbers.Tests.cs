using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Expressions;
using DummyLang.SyntacticAnalysis.Parsers;
using DummyLang.SyntacticAnalysis.Utilities;
using Xunit;

namespace DummyLang.SyntacticAnalysis.Tests.Expressions;

public class NumberSyntaxParserTests
{
    [Fact]
    public void ParseExpression_Numbers_Binary()
    {
        // Arrange
        var tokens = ParsingUtilities.ReadAllTokens("0b101");
        var index  = 0;

        // Act
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(2, tokens.Length);
        Assert.Equal(1, index);
        Assert.IsType<NumberLiteralExpression>(expression);

        var numberLiteralExpression = (NumberLiteralExpression)expression;
        Assert.Equal(NumberType.Binary, numberLiteralExpression.Type);
    }

    [Fact]
    public void ParseExpression_Numbers_Hexadecimal()
    {
        // Arrange
        var tokens = ParsingUtilities.ReadAllTokens("0x0123456789abcdef");
        var index  = 0;

        // Act
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(2, tokens.Length);
        Assert.Equal(1, index);
        Assert.IsType<NumberLiteralExpression>(expression);

        var numberLiteralExpression = (NumberLiteralExpression)expression;
        Assert.Equal(NumberType.Hexadecimal, numberLiteralExpression.Type);
    }

    [Fact]
    public void ParseExpression_Numbers_Unsigned()
    {
        // Arrange
        var tokens = ParsingUtilities.ReadAllTokens("123u");
        var index  = 0;

        // Act
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(2, tokens.Length);
        Assert.Equal(1, index);
        Assert.IsType<NumberLiteralExpression>(expression);

        var numberLiteralExpression = (NumberLiteralExpression)expression;
        Assert.Equal(NumberType.UnsignedInteger, numberLiteralExpression.Type);
    }

    [Fact]
    public void ParseExpression_Numbers_Long()
    {
        // Arrange
        var tokens = ParsingUtilities.ReadAllTokens("123l");
        var index  = 0;

        // Act
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(2, tokens.Length);
        Assert.Equal(1, index);
        Assert.IsType<NumberLiteralExpression>(expression);

        var numberLiteralExpression = (NumberLiteralExpression)expression;
        Assert.Equal(NumberType.Long, numberLiteralExpression.Type);
    }

    [Fact]
    public void ParseExpression_Numbers_UnsignedLong()
    {
        // Arrange
        var tokens = ParsingUtilities.ReadAllTokens("123ul");
        var index  = 0;

        // Act
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(2, tokens.Length);
        Assert.Equal(1, index);
        Assert.IsType<NumberLiteralExpression>(expression);

        var numberLiteralExpression = (NumberLiteralExpression)expression;
        Assert.Equal(NumberType.UnsignedLong, numberLiteralExpression.Type);
    }

    [Fact]
    public void ParseExpression_Numbers_Float()
    {
        // Arrange
        var tokens = ParsingUtilities.ReadAllTokens("123.123f");
        var index  = 0;

        // Act
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(2, tokens.Length);
        Assert.Equal(1, index);
        Assert.IsType<NumberLiteralExpression>(expression);

        var numberLiteralExpression = (NumberLiteralExpression)expression;
        Assert.Equal(NumberType.Float, numberLiteralExpression.Type);
    }

    [Fact]
    public void ParseExpression_Numbers_Double()
    {
        // Arrange
        var tokens = ParsingUtilities.ReadAllTokens("123.123d");
        var index  = 0;

        // Act
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(2, tokens.Length);
        Assert.Equal(1, index);
        Assert.IsType<NumberLiteralExpression>(expression);

        var numberLiteralExpression = (NumberLiteralExpression)expression;
        Assert.Equal(NumberType.Double, numberLiteralExpression.Type);
    }

    [Fact]
    public void ParseExpression_Numbers_Decimal()
    {
        // Arrange
        var tokens = ParsingUtilities.ReadAllTokens("123.123m");
        var index  = 0;

        // Act
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(2, tokens.Length);
        Assert.Equal(1, index);
        Assert.IsType<NumberLiteralExpression>(expression);

        var numberLiteralExpression = (NumberLiteralExpression)expression;
        Assert.Equal(NumberType.Decimal, numberLiteralExpression.Type);
    }

    [Fact]
    public void ParseExpression_Numbers_WithExponent()
    {
        // Arrange
        var tokens = ParsingUtilities.ReadAllTokens("123.12e-10");
        var index  = 0;

        // Act
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(2, tokens.Length);
        Assert.Equal(1, index);
        Assert.IsType<NumberLiteralExpression>(expression);

        var numberLiteralExpression = (NumberLiteralExpression)expression;
        Assert.Equal(NumberType.WithExponent, numberLiteralExpression.Type);
    }

    [Fact]
    public void ParseExpression_Numbers_RealShouldHaveDot()
    {
        // Arrange
        var tokens = ParsingUtilities.ReadAllTokens("123f");
        var index  = 0;

        // Act
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(3, tokens.Length);
        Assert.Equal(1, index);
        Assert.IsType<NumberLiteralExpression>(expression);

        var numberLiteralExpression = (NumberLiteralExpression)expression;
        Assert.Equal(NumberType.Integer, numberLiteralExpression.Type);
        Assert.Equal("123", numberLiteralExpression.NumberToken.Value);

        // Act again
        tokens     = ParsingUtilities.ReadAllTokens("123e10");
        index      = 0;
        expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(3, tokens.Length);
        Assert.Equal(1, index);
        Assert.IsType<NumberLiteralExpression>(expression);

        numberLiteralExpression = (NumberLiteralExpression)expression;
        Assert.Equal(NumberType.Integer, numberLiteralExpression.Type);
        Assert.Equal("123", numberLiteralExpression.NumberToken.Value);
    }

    [Fact]
    public void ParseExpression_Expressions_SimpleBinaryExpression()
    {
        // Arrange
        var tokens = ParsingUtilities.ReadAllTokens("1 + 2");
        var index  = 0;

        // Act
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(4, tokens.Length);
        Assert.Equal(3, index);
        Assert.IsType<BinaryExpression>(expression);

        var binaryExpression = (BinaryExpression)expression;
        Assert.IsType<NumberLiteralExpression>(binaryExpression.Left);
        Assert.IsType<NumberLiteralExpression>(binaryExpression.Right);
        Assert.Equal(TokenType.Plus, binaryExpression.Operator.Type);
    }

    [Fact]
    public void ParseExpression_Expressions_MoreComplexMathTree()
    {
        // Arrange
        var tokens = ParsingUtilities.ReadAllTokens("0b1010 + 2.0e-10f * (-3L - foo++)");
        var index  = 0;

        // Act
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        // addition = 0b1010 + multiplication
        Assert.NotNull(expression);
        Assert.Equal(12, tokens.Length);
        Assert.Equal(11, index);
        Assert.IsType<BinaryExpression>(expression);

        var binaryExpression = (BinaryExpression)expression;
        Assert.Equal(TokenType.Plus, binaryExpression.Operator.Type);
        Assert.IsType<NumberLiteralExpression>(binaryExpression.Left);
        var binary = (NumberLiteralExpression)binaryExpression.Left;
        Assert.Equal(NumberType.Binary, binary.Type);
        Assert.Equal("0b1010", binary.NumberToken.Value);

        // multiplication = 2.0e-10f * parenthesised
        Assert.IsType<BinaryExpression>(binaryExpression.Right);
        var multiplication = (BinaryExpression)binaryExpression.Right;
        Assert.Equal(TokenType.Star, multiplication.Operator.Type);

        Assert.IsType<NumberLiteralExpression>(multiplication.Left);
        var real = (NumberLiteralExpression)multiplication.Left;
        Assert.Equal(NumberType.Float, real.Type);
        Assert.Equal("2.0e-10f", real.NumberToken.Value);

        // parenthesised = ( subtraction )
        Assert.IsType<ParenthesisedExpression>(multiplication.Right);
        var parenthesised = (ParenthesisedExpression)multiplication.Right;
        Assert.Equal(TokenType.LeftParenthesis, parenthesised.LeftParen.Type);
        Assert.Equal(TokenType.RightParenthesis, parenthesised.RightParen.Type);

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