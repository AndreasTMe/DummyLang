using DummyLang.SyntacticAnalysis.Expressions;
using DummyLang.SyntacticAnalysis.Parsers;
using DummyLang.SyntacticAnalysis.Utilities;
using Xunit;

namespace DummyLang.SyntacticAnalysis.Tests.Expressions;

public class CharacterSyntaxParserTests
{
    [Fact]
    public void ParseExpression_Character_ReadSuccessfully()
    {
        // Arrange
        // Act
        var tokens     = ParsingUtilities.ReadAllTokens("'a'");
        var index      = 0;
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(2, tokens.Length);
        Assert.Equal(1, index);
        Assert.IsType<CharacterLiteralExpression>(expression);
    }

    [Fact]
    public void ParseExpression_Empty_InvalidExpression()
    {
        // Arrange
        // Act
        var tokens     = ParsingUtilities.ReadAllTokens("''");
        var index      = 0;
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(2, tokens.Length);
        Assert.Equal(1, index);
    }

    [Fact]
    public void ParseExpression_NoClosingSingleQuote_InvalidExpression()
    {
        // Arrange
        // Act
        var tokens     = ParsingUtilities.ReadAllTokens("'");
        var index      = 0;
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(2, tokens.Length);
        Assert.Equal(1, index);
    }

    [Fact]
    public void ParseExpression_EscapedCharacter_ReadSuccessfully()
    {
        // Arrange
        // Act
        var tokens     = ParsingUtilities.ReadAllTokens("'\\n'");
        var index      = 0;
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(2, tokens.Length);
        Assert.Equal(1, index);
        Assert.IsType<CharacterLiteralExpression>(expression);
    }

    [Fact]
    public void ParseExpression_HexCharacter_ReadSuccessfully()
    {
        // Arrange
        // Act
        var tokens     = ParsingUtilities.ReadAllTokens("'\\x123'");
        var index      = 0;
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(2, tokens.Length);
        Assert.Equal(1, index);
        Assert.IsType<CharacterLiteralExpression>(expression);
    }
}