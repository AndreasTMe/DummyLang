using DummyLang.SyntacticAnalysis.Expressions;
using DummyLang.SyntacticAnalysis.Parsers;
using DummyLang.SyntacticAnalysis.Utilities;
using DummyLang.SyntacticAnalysis.Visitors;
using Xunit;

namespace DummyLang.SyntacticAnalysis.Tests.Expressions;

public class StringSyntaxParserTests
{
    [Fact]
    public void ParseExpression_String_ReadSuccessfully()
    {
        // Arrange
        var validator = new SyntaxNodeValidationVisitor();
        var tokens    = ParsingUtilities.ReadAllTokens("\"some string 123\"");
        var index     = 0;

        // Act
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(2, tokens.Length);
        Assert.Equal(1, index);
        Assert.IsType<StringLiteralExpression>(expression);

        Assert.False(validator.HasErrors);
        expression.Accept(validator);
        Assert.False(validator.HasErrors);
    }

    [Fact]
    public void ParseExpression_Empty_ReadSuccessfully()
    {
        // Arrange
        var validator = new SyntaxNodeValidationVisitor();
        var tokens    = ParsingUtilities.ReadAllTokens("\"\"");
        var index     = 0;

        // Act
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(2, tokens.Length);
        Assert.Equal(1, index);
        Assert.IsType<StringLiteralExpression>(expression);

        Assert.False(validator.HasErrors);
        expression.Accept(validator);
        Assert.False(validator.HasErrors);
    }

    [Fact]
    public void ParseExpression_EscapedCharacters_ReadSuccessfully()
    {
        // Arrange
        var validator = new SyntaxNodeValidationVisitor();
        var tokens    = ParsingUtilities.ReadAllTokens("\"some\\\" string\\n \\xab12 123\"");
        var index     = 0;

        // Act
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(2, tokens.Length);
        Assert.Equal(1, index);
        Assert.IsType<StringLiteralExpression>(expression);

        Assert.False(validator.HasErrors);
        expression.Accept(validator);
        Assert.False(validator.HasErrors);
    }

    [Fact]
    public void ParseExpression_NoClosingDoubleQuote_InvalidExpression()
    {
        // Arrange
        var validator = new SyntaxNodeValidationVisitor();
        var tokens    = ParsingUtilities.ReadAllTokens("\"");
        var index     = 0;

        // Act
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(2, tokens.Length);
        Assert.Equal(1, index);

        Assert.False(validator.HasErrors);
        expression.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(validator.Diagnostics, d => d.Message.EndsWith(StringLiteralExpression.InvalidDoubleQuotes));
    }

    [Fact]
    public void ParseExpression_EscapedClosingDoubleQuote_InvalidExpression()
    {
        // Arrange
        var validator = new SyntaxNodeValidationVisitor();
        var tokens    = ParsingUtilities.ReadAllTokens("\"\\\"");
        var index     = 0;

        // Act
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(2, tokens.Length);
        Assert.Equal(1, index);

        Assert.False(validator.HasErrors);
        expression.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(validator.Diagnostics, d => d.Message.EndsWith(StringLiteralExpression.EscapedLastDoubleQuote));
    }

    [Fact]
    public void ParseExpression_InvalidEscapedCharacter_InvalidExpression()
    {
        // Arrange
        var validator = new SyntaxNodeValidationVisitor();
        var tokens    = ParsingUtilities.ReadAllTokens("\"\\w\"");
        var index     = 0;

        // Act
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(2, tokens.Length);
        Assert.Equal(1, index);

        Assert.False(validator.HasErrors);
        expression.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(
            validator.Diagnostics,
            d => d.Message.EndsWith(StringLiteralExpression.InvalidEscapedCharacters));
    }

    [Fact]
    public void ParseExpression_InvalidHexCharacter_InvalidExpression()
    {
        // Arrange
        var validator = new SyntaxNodeValidationVisitor();
        var tokens    = ParsingUtilities.ReadAllTokens("\"\\xZ\"");
        var index     = 0;

        // Act
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(2, tokens.Length);
        Assert.Equal(1, index);

        Assert.False(validator.HasErrors);
        expression.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(
            validator.Diagnostics,
            d => d.Message.EndsWith(StringLiteralExpression.InvalidEscapedCharacters));
    }
}