using DummyLang.SyntacticAnalysis.Expressions;
using Xunit;

// ReSharper disable ParameterOnlyUsedForPreconditionCheck.Local
namespace DummyLang.SyntacticAnalysis.Tests.Expressions;

public class CharacterSyntaxParserTests
{
    [Fact]
    public void GenerateSyntax_Character_ReadSuccessfully()
    {
        // Arrange
        // Act
        var parser = new SyntaxParser();
        var syntaxTree = parser.Feed("'a'")
                               .GenerateSyntax();

        // Assert
        Assert.NotNull(syntaxTree);
        Assert.Equal(1, syntaxTree.Nodes.Count);
        Assert.IsType<CharacterLiteralExpression>(syntaxTree.Nodes[0]);
    }

    [Fact]
    public void GenerateSyntax_Empty_InvalidExpression()
    {
        // Arrange
        // Act
        var parser = new SyntaxParser();
        var syntaxTree = parser.Feed("''")
                               .GenerateSyntax();

        // Assert
        Assert.NotNull(syntaxTree);
        Assert.Equal(1, syntaxTree.Nodes.Count);
        Assert.IsType<InvalidExpression>(syntaxTree.Nodes[0]);

        Assert.Single(syntaxTree.Diagnostics);
        Assert.All(
            syntaxTree.Diagnostics,
            info => { Assert.Contains(CharacterLiteralExpression.ShouldBeOfCertainLength, info.Message); });
    }

    [Fact]
    public void GenerateSyntax_NoClosingSingleQuote_InvalidExpression()
    {
        // Arrange
        // Act
        var parser = new SyntaxParser();
        var syntaxTree = parser.Feed("'")
                               .GenerateSyntax();

        // Assert
        Assert.NotNull(syntaxTree);
        Assert.Equal(1, syntaxTree.Nodes.Count);
        Assert.IsType<InvalidExpression>(syntaxTree.Nodes[0]);

        Assert.Single(syntaxTree.Diagnostics);
        Assert.All(
            syntaxTree.Diagnostics,
            info => { Assert.Contains(CharacterLiteralExpression.ShouldBeOfCertainLength, info.Message); });
    }

    [Fact]
    public void GenerateSyntax_EscapedCharacter_ReadSuccessfully()
    {
        // Arrange
        // Act
        var parser = new SyntaxParser();
        var syntaxTree = parser.Feed("'\\n'")
                               .GenerateSyntax();

        // Assert
        Assert.NotNull(syntaxTree);
        Assert.Equal(1, syntaxTree.Nodes.Count);
        Assert.IsType<CharacterLiteralExpression>(syntaxTree.Nodes[0]);
    }

    [Fact]
    public void GenerateSyntax_HexCharacter_ReadSuccessfully()
    {
        // Arrange
        // Act
        var parser = new SyntaxParser();
        var syntaxTree = parser.Feed("'\\x123'")
                               .GenerateSyntax();

        // Assert
        Assert.NotNull(syntaxTree);
        Assert.Equal(1, syntaxTree.Nodes.Count);
        Assert.IsType<CharacterLiteralExpression>(syntaxTree.Nodes[0]);
    }
}