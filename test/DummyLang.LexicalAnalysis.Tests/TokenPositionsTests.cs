using Xunit;

namespace DummyLang.LexicalAnalysis.Tests;

public class TokenPositionsTests
{
    [Fact]
    public void Indexer_SixItems_ShouldReturnCorrectPositions()
    {
        // Arrange
        var positions = new TokenPositions(
            TokenPosition.At(1, 1),
            TokenPosition.At(1, 4),
            TokenPosition.At(2, 1),
            null,
            TokenPosition.Zero,
            TokenPosition.At(3, 6));

        // Act
        // Assert
        Assert.Equal(6, positions.Length);

        Assert.Equal(TokenPosition.At(1, 1), positions[0]);
        Assert.Equal(TokenPosition.At(1, 4), positions[1]);
        Assert.Equal(TokenPosition.At(2, 1), positions[2]);
        Assert.Equal(TokenPosition.At(3, 6), positions[3]);
        Assert.Equal(TokenPosition.At(3, 6), positions[4]);
        Assert.Equal(TokenPosition.At(3, 6), positions[5]);

        Assert.Equal(TokenPosition.At(1, 1), positions[0, false]);
        Assert.Equal(TokenPosition.At(1, 4), positions[1, false]);
        Assert.Equal(TokenPosition.At(2, 1), positions[2, false]);
        Assert.Equal(TokenPosition.At(2, 1), positions[3, false]);
        Assert.Equal(TokenPosition.At(2, 1), positions[4, false]);
        Assert.Equal(TokenPosition.At(3, 6), positions[5, false]);
    }
    
    [Fact]
    public void Indexer_NoPositions_ShouldReturnZero()
    {
        // Arrange
        var positions = new TokenPositions();

        // Act
        // Assert
        Assert.Equal(0, positions.Length);

        Assert.Equal(TokenPosition.Zero, positions[0]);
        Assert.Equal(TokenPosition.Zero, positions[0, false]);
    }
    
    [Fact]
    public void Indexer_Null_ShouldReturnZero()
    {
        // Arrange
        var positions = new TokenPositions(null);

        // Act
        // Assert
        Assert.Equal(0, positions.Length);

        Assert.Equal(TokenPosition.Zero, positions[0]);
        Assert.Equal(TokenPosition.Zero, positions[0, false]);
    }
}