using DummyLang.SyntacticAnalysis.Expressions;
using Xunit;

namespace DummyLang.SyntacticAnalysis.Tests.Expressions;

// ReSharper disable ParameterOnlyUsedForPreconditionCheck.Local
public class StringSyntaxParserTests
{
    [Fact]
    public void GenerateSyntax_String_ReadSuccessfully()
    {
        // Arrange
        // Act
        var parser = new SyntaxParser();
        var syntaxTree = parser.Feed("\"some string 123\"")
                               .GenerateSyntax();

        // Assert
        Assert.NotNull(syntaxTree);
        Assert.Equal(1, syntaxTree.Nodes.Count);
        Assert.IsType<StringLiteralExpression>(syntaxTree.Nodes[0]);
    }

    [Fact]
    public void GenerateSyntax_Empty_ReadSuccessfully()
    {
        // Arrange
        // Act
        var parser = new SyntaxParser();
        var syntaxTree = parser.Feed("\"\"")
                               .GenerateSyntax();

        // Assert
        Assert.NotNull(syntaxTree);
        Assert.Equal(1, syntaxTree.Nodes.Count);
        Assert.IsType<StringLiteralExpression>(syntaxTree.Nodes[0]);
    }

    [Fact]
    public void GenerateSyntax_NoClosingDoubleQuote_InvalidExpression()
    {
        // Arrange
        // Act
        var parser = new SyntaxParser();
        var syntaxTree = parser.Feed("\"")
                               .GenerateSyntax();

        // Assert
        Assert.NotNull(syntaxTree);
        Assert.Equal(1, syntaxTree.Nodes.Count);
        Assert.IsType<InvalidExpression>(syntaxTree.Nodes[0]);

        Assert.Single(syntaxTree.Diagnostics);
        Assert.All(
            syntaxTree.Diagnostics,
            info => { Assert.Contains(StringLiteralExpression.ShouldBeSurroundedByDoubleQuote, info.Message); });
    }

    [Fact]
    public void GenerateSyntax_EscapedClosingDoubleQuote_InvalidExpression()
    {
        // Arrange
        // Act
        var parser = new SyntaxParser();
        var syntaxTree = parser.Feed("\"\\\"")
                               .GenerateSyntax();

        // Assert
        Assert.NotNull(syntaxTree);
        Assert.Equal(1, syntaxTree.Nodes.Count);
        Assert.IsType<InvalidExpression>(syntaxTree.Nodes[0]);

        Assert.Single(syntaxTree.Diagnostics);
        Assert.All(
            syntaxTree.Diagnostics,
            info => { Assert.Contains(StringLiteralExpression.ShouldNotEscapeLastDoubleQuote, info.Message); });
    }

    [Fact]
    public void GenerateSyntax_EscapedCharacters_ReadSuccessfully()
    {
        // Arrange
        // Act
        var parser = new SyntaxParser();
        var syntaxTree = parser.Feed("\"some\\\" string\\n \\xab12 123\"")
                               .GenerateSyntax();

        // Assert
        Assert.NotNull(syntaxTree);
        Assert.Equal(1, syntaxTree.Nodes.Count);
        Assert.IsType<StringLiteralExpression>(syntaxTree.Nodes[0]);
    }

    [Fact]
    public void GenerateSyntax_InvalidEscapedCharacter_InvalidExpression()
    {
        // Arrange
        // Act
        var parser = new SyntaxParser();
        var syntaxTree = parser.Feed("\"\\w\"")
                               .GenerateSyntax();

        // Assert
        Assert.NotNull(syntaxTree);
        Assert.Equal(1, syntaxTree.Nodes.Count);
        Assert.IsType<InvalidExpression>(syntaxTree.Nodes[0]);

        Assert.Single(syntaxTree.Diagnostics);
        Assert.All(
            syntaxTree.Diagnostics,
            info => { Assert.Contains(StringLiteralExpression.InvalidEscapedCharacter, info.Message); });
    }

    [Fact]
    public void GenerateSyntax_InvalidHexCharacter_InvalidExpression()
    {
        // Arrange
        // Act
        var parser = new SyntaxParser();
        var syntaxTree = parser.Feed("\"\\xZ\"")
                               .GenerateSyntax();

        // Assert
        Assert.NotNull(syntaxTree);
        Assert.Equal(1, syntaxTree.Nodes.Count);
        Assert.IsType<InvalidExpression>(syntaxTree.Nodes[0]);

        Assert.Single(syntaxTree.Diagnostics);
        Assert.All(
            syntaxTree.Diagnostics,
            info => { Assert.Contains(StringLiteralExpression.InvalidEscapedCharacter, info.Message); });
    }
}