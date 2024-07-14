using DummyLang.SyntacticAnalysis.Expressions;
using DummyLang.SyntacticAnalysis.Parsers;
using DummyLang.SyntacticAnalysis.Utilities;
using DummyLang.SyntacticAnalysis.Visitors;
using Xunit;

namespace DummyLang.SyntacticAnalysis.Tests.Expressions;

public class CharacterSyntaxParserTests
{
    [Fact]
    public void ParseExpression_Character_ReadSuccessfully()
    {
        // Arrange
        var validator = new SyntaxNodeValidationVisitor();

        // Act
        var tokens     = ParsingUtilities.ReadAllTokens("'a'");
        var index      = 0;
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(2, tokens.Length);
        Assert.Equal(1, index);
        Assert.IsType<CharacterLiteralExpression>(expression);

        Assert.False(validator.HasErrors);
        expression.Accept(validator);
        Assert.False(validator.HasErrors);
    }

    [Fact]
    public void ParseExpression_EscapedCharacter_ReadSuccessfully()
    {
        // Arrange
        var validator = new SyntaxNodeValidationVisitor();

        // Act
        var tokens     = ParsingUtilities.ReadAllTokens("'\\n'");
        var index      = 0;
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(2, tokens.Length);
        Assert.Equal(1, index);
        Assert.IsType<CharacterLiteralExpression>(expression);

        Assert.False(validator.HasErrors);
        expression.Accept(validator);
        Assert.False(validator.HasErrors);
    }

    [Fact]
    public void ParseExpression_HexCharacter_ReadSuccessfully()
    {
        // Arrange
        var validator = new SyntaxNodeValidationVisitor();

        // Act
        var tokens     = ParsingUtilities.ReadAllTokens("'\\x123'");
        var index      = 0;
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(2, tokens.Length);
        Assert.Equal(1, index);
        Assert.IsType<CharacterLiteralExpression>(expression);

        Assert.False(validator.HasErrors);
        expression.Accept(validator);
        Assert.False(validator.HasErrors);
    }
    
    [Fact]
    public void ParseExpression_Empty_InvalidExpression()
    {
        // Arrange
        var validator = new SyntaxNodeValidationVisitor();

        // Act
        var tokens     = ParsingUtilities.ReadAllTokens("''");
        var index      = 0;
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(2, tokens.Length);
        Assert.Equal(1, index);

        Assert.False(validator.HasErrors);
        expression.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(validator.Diagnostics, d => d.Message.EndsWith(CharacterLiteralExpression.InvalidLength));
    }

    [Fact]
    public void ParseExpression_NoClosingSingleQuote_InvalidExpression()
    {
        // Arrange
        var validator = new SyntaxNodeValidationVisitor();

        // Act
        var tokens     = ParsingUtilities.ReadAllTokens("'a");
        var index      = 0;
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(2, tokens.Length);
        Assert.Equal(1, index);

        Assert.False(validator.HasErrors);
        expression.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(validator.Diagnostics, d => d.Message.EndsWith(CharacterLiteralExpression.InvalidSingleQuotes));
    }
    
    [Fact]
    public void ParseExpression_NonEscapedCharacter_InvalidExpression()
    {
        // Arrange
        var validator = new SyntaxNodeValidationVisitor();

        // Act
        var tokens     = ParsingUtilities.ReadAllTokens("'\\'");
        var index      = 0;
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(2, tokens.Length);
        Assert.Equal(1, index);

        Assert.False(validator.HasErrors);
        expression.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(validator.Diagnostics, d => d.Message.EndsWith(CharacterLiteralExpression.NonEscapedCharacter));
    }
    
    [Fact]
    public void ParseExpression_InvalidEscapedCharacter_InvalidExpression()
    {
        // Arrange
        var validator = new SyntaxNodeValidationVisitor();

        // Act
        var tokens     = ParsingUtilities.ReadAllTokens("'\\e'");
        var index      = 0;
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(2, tokens.Length);
        Assert.Equal(1, index);

        Assert.False(validator.HasErrors);
        expression.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(validator.Diagnostics, d => d.Message.EndsWith(CharacterLiteralExpression.InvalidEscapedCharacter));
    }
    
    [Fact]
    public void ParseExpression_InvalidHex_InvalidExpression()
    {
        // Arrange
        var validator = new SyntaxNodeValidationVisitor();

        // Act
        var tokens     = ParsingUtilities.ReadAllTokens("'\\xZ'");
        var index      = 0;
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(2, tokens.Length);
        Assert.Equal(1, index);

        Assert.False(validator.HasErrors);
        expression.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(validator.Diagnostics, d => d.Message.EndsWith(CharacterLiteralExpression.InvalidHex));
    }
}