using DummyLang.SyntacticAnalysis.Expressions;
using DummyLang.SyntacticAnalysis.Parsers;
using DummyLang.SyntacticAnalysis.Utilities;
using Xunit;

namespace DummyLang.SyntacticAnalysis.Tests.Expressions;

public class StringSyntaxParserTests
{
    [Fact]
    public void ParseExpression_String_ReadSuccessfully()
    {
        // Arrange
        // Act
        var tokens     = ParsingUtilities.ReadAllTokens("\"some string 123\"");
        var index      = 0;
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(2, tokens.Length);
        Assert.Equal(1, index);
        Assert.IsType<StringLiteralExpression>(expression);
    }

    [Fact]
    public void ParseExpression_Empty_ReadSuccessfully()
    {
        // Arrange
        // Act
        var tokens     = ParsingUtilities.ReadAllTokens("\"\"");
        var index      = 0;
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(2, tokens.Length);
        Assert.Equal(1, index);
        Assert.IsType<StringLiteralExpression>(expression);
    }

    [Fact]
    public void ParseExpression_NoClosingDoubleQuote_InvalidExpression()
    {
        // Arrange
        // Act
        var tokens     = ParsingUtilities.ReadAllTokens("\"");
        var index      = 0;
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(2, tokens.Length);
        Assert.Equal(1, index);
        // TODO: Handle invalid string in "Visitor.Visit"
    }

    [Fact]
    public void ParseExpression_EscapedClosingDoubleQuote_InvalidExpression()
    {
        // Arrange
        // Act
        var tokens     = ParsingUtilities.ReadAllTokens("\"\\\"");
        var index      = 0;
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(2, tokens.Length);
        Assert.Equal(1, index);
        // TODO: Handle invalid string in "Visitor.Visit"
    }

    [Fact]
    public void ParseExpression_EscapedCharacters_ReadSuccessfully()
    {
        // Arrange
        // Act
        var tokens     = ParsingUtilities.ReadAllTokens("\"some\\\" string\\n \\xab12 123\"");
        var index      = 0;
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(2, tokens.Length);
        Assert.Equal(1, index);
        Assert.IsType<StringLiteralExpression>(expression);
    }

    [Fact]
    public void ParseExpression_InvalidEscapedCharacter_InvalidExpression()
    {
        // Arrange
        // Act
        var tokens     = ParsingUtilities.ReadAllTokens("\"\\w\"");
        var index      = 0;
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(2, tokens.Length);
        Assert.Equal(1, index);
        // TODO: Handle invalid string in "Visitor.Visit"
    }

    [Fact]
    public void ParseExpression_InvalidHexCharacter_InvalidExpression()
    {
        // Arrange
        // Act
        var tokens     = ParsingUtilities.ReadAllTokens("\"\\xZ\"");
        var index      = 0;
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(2, tokens.Length);
        Assert.Equal(1, index);
        // TODO: Handle invalid string in "Visitor.Visit"
    }
}