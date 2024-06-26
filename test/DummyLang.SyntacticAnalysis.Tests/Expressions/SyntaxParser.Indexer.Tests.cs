using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Expressions;
using Xunit;

namespace DummyLang.SyntacticAnalysis.Tests.Expressions;

public class IndexerSyntaxParserTests
{
    [Fact]
    public void GenerateSyntax_Indexer_ReadSuccessfully()
    {
        // Arrange
        const string source = "test[1]";

        // Act
        var parser = new SyntaxParser();
        var syntaxTree = parser.Feed(source)
                               .GenerateSyntax();

        // Assert
        Assert.NotNull(syntaxTree);
        Assert.Equal(1, syntaxTree.Nodes.Count);
        Assert.IsType<PrimaryExpression>(syntaxTree.Nodes[0]);

        var primary = (PrimaryExpression)syntaxTree.Nodes[0];
        Assert.Equal(Token.None, primary.Token);
        Assert.IsType<IndexerExpression>(primary.Expression);

        var indexer = (IndexerExpression)primary.Expression;
        Assert.Equal(TokenType.Identifier, indexer.Identifier.Type);
        Assert.Equal("test", indexer.Identifier.Value);

        Assert.Equal(TokenType.LeftBracket, indexer.LeftBracket.Type);
        Assert.Equal("[", indexer.LeftBracket.Value);

        Assert.Equal(TokenType.RightBracket, indexer.RightBracket.Type);
        Assert.Equal("]", indexer.RightBracket.Value);
        
        Assert.NotNull(indexer.Index);
        Assert.IsType<NumberLiteralExpression>(indexer.Index);
    }
    
    [Fact]
    public void GenerateSyntax_Indexer_NoIndex()
    {
        // Arrange
        const string source = "test[]";

        // Act
        var parser = new SyntaxParser();
        var syntaxTree = parser.Feed(source)
                               .GenerateSyntax();

        // Assert
        Assert.NotNull(syntaxTree);
        Assert.Equal(1, syntaxTree.Nodes.Count);
        Assert.IsType<PrimaryExpression>(syntaxTree.Nodes[0]);

        var primary = (PrimaryExpression)syntaxTree.Nodes[0];
        Assert.Equal(Token.None, primary.Token);
        Assert.IsType<IndexerExpression>(primary.Expression);

        var indexer = (IndexerExpression)primary.Expression;
        Assert.Equal(TokenType.Identifier, indexer.Identifier.Type);
        Assert.Equal("test", indexer.Identifier.Value);

        Assert.Equal(TokenType.LeftBracket, indexer.LeftBracket.Type);
        Assert.Equal("[", indexer.LeftBracket.Value);

        Assert.Equal(TokenType.RightBracket, indexer.RightBracket.Type);
        Assert.Equal("]", indexer.RightBracket.Value);
        
        Assert.NotNull(indexer.Index);
        Assert.IsType<InvalidExpression>(indexer.Index);
    }

    [Fact]
    public void GenerateSyntax_Indexer_NoClosingBracket()
    {
        // Arrange
        const string source = "test[";
    
        // Act
        var parser = new SyntaxParser();
        var syntaxTree = parser.Feed(source)
                               .GenerateSyntax();
    
        // Assert
        Assert.NotNull(syntaxTree);
        Assert.Equal(1, syntaxTree.Nodes.Count);
        Assert.IsType<InvalidExpression>(syntaxTree.Nodes[0]);
    
        var invalid = (InvalidExpression)syntaxTree.Nodes[0];
        Assert.Equal(TokenType.LeftBracket, invalid.Token.Type);
        Assert.Equal("[", invalid.Token.Value);
        Assert.IsType<IndexerExpression>(invalid.Expression);
    
        var indexer = (IndexerExpression)invalid.Expression;
        Assert.Equal(TokenType.Identifier, indexer.Identifier.Type);
        Assert.Equal("test", indexer.Identifier.Value);
    
        Assert.Equal(TokenType.LeftBracket, indexer.LeftBracket.Type);
        Assert.Equal("[", indexer.LeftBracket.Value);
    
        Assert.Equal(TokenType.None, indexer.RightBracket.Type);
        Assert.Equal("", indexer.RightBracket.Value);
    
        Assert.Null(indexer.Index);
    }
    
    [Fact]
    public void GenerateSyntax_Indexer_NoClosingBracketWithIndex()
    {
        // Arrange
        const string source = "test[1";
    
        // Act
        var parser = new SyntaxParser();
        var syntaxTree = parser.Feed(source)
                               .GenerateSyntax();
    
        // Assert
        Assert.NotNull(syntaxTree);
        Assert.Equal(1, syntaxTree.Nodes.Count);
        Assert.IsType<InvalidExpression>(syntaxTree.Nodes[0]);
    
        var invalid = (InvalidExpression)syntaxTree.Nodes[0];
        Assert.Equal(TokenType.LeftBracket, invalid.Token.Type);
        Assert.Equal("[", invalid.Token.Value);
        Assert.IsType<IndexerExpression>(invalid.Expression);
    
        var indexer = (IndexerExpression)invalid.Expression;
        Assert.Equal(TokenType.Identifier, indexer.Identifier.Type);
        Assert.Equal("test", indexer.Identifier.Value);
    
        Assert.Equal(TokenType.LeftBracket, indexer.LeftBracket.Type);
        Assert.Equal("[", indexer.LeftBracket.Value);
    
        Assert.Equal(TokenType.None, indexer.RightBracket.Type);
        Assert.Equal("", indexer.RightBracket.Value);
    
        Assert.NotNull(indexer.Index);
        Assert.IsType<NumberLiteralExpression>(indexer.Index);
    }
    
    [Fact]
    public void GenerateSyntax_Indexer_ReservedKeywordFound()
    {
        // Arrange
        const string source = """
                              test[1 + 2
                              var t = 1;
                              """;
    
        // Act
        var parser = new SyntaxParser();
        var syntaxTree = parser.Feed(source)
                               .GenerateSyntax();
    
        // Assert
        Assert.NotNull(syntaxTree);
        Assert.Equal(3, syntaxTree.Nodes.Count); // TODO: Revisit this when declarations are implemented
        Assert.IsType<InvalidExpression>(syntaxTree.Nodes[0]);
    
        var invalid = (InvalidExpression)syntaxTree.Nodes[0];
        Assert.Equal(TokenType.LeftBracket, invalid.Token.Type);
        Assert.Equal("[", invalid.Token.Value);
        Assert.IsType<IndexerExpression>(invalid.Expression);
    
        var indexer = (IndexerExpression)invalid.Expression;
        Assert.Equal(TokenType.Identifier, indexer.Identifier.Type);
        Assert.Equal("test", indexer.Identifier.Value);
    
        Assert.Equal(TokenType.LeftBracket, indexer.LeftBracket.Type);
        Assert.Equal("[", indexer.LeftBracket.Value);
    
        Assert.Equal(TokenType.None, indexer.RightBracket.Type);
        Assert.Equal("", indexer.RightBracket.Value);
    
        Assert.NotNull(indexer.Index);
        Assert.IsType<BinaryExpression>(indexer.Index);
    }
}