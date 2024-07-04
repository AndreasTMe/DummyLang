using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Expressions;
using DummyLang.SyntacticAnalysis.Parsers;
using DummyLang.SyntacticAnalysis.Utilities;
using Xunit;

namespace DummyLang.SyntacticAnalysis.Tests.Expressions;

public class IndexerSyntaxParserTests
{
    [Fact]
    public void ParseExpression_Indexer_ReadSuccessfully()
    {
        // Arrange
        const string source = "test[1]";

        // Act
        var tokens     = ParsingUtilities.ReadAllTokens(source);
        var index      = 0;
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(5, tokens.Length);
        Assert.Equal(4, index);
        Assert.IsType<PrimaryExpression>(expression);

        var primary = (PrimaryExpression)expression;
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
    public void ParseExpression_Indexer_NoIndex()
    {
        // Arrange
        const string source = "test[]";

        // Act
        var tokens     = ParsingUtilities.ReadAllTokens(source);
        var index      = 0;
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(4, tokens.Length);
        Assert.Equal(3, index);
        Assert.IsType<PrimaryExpression>(expression);

        var primary = (PrimaryExpression)expression;
        Assert.Equal(Token.None, primary.Token);
        Assert.IsType<IndexerExpression>(primary.Expression);

        var indexer = (IndexerExpression)primary.Expression;
        Assert.Equal(TokenType.Identifier, indexer.Identifier.Type);
        Assert.Equal("test", indexer.Identifier.Value);

        Assert.Equal(TokenType.LeftBracket, indexer.LeftBracket.Type);
        Assert.Equal("[", indexer.LeftBracket.Value);

        Assert.Equal(TokenType.RightBracket, indexer.RightBracket.Type);
        Assert.Equal("]", indexer.RightBracket.Value);
        
        Assert.Null(indexer.Index);
    }

    [Fact]
    public void ParseExpression_Indexer_NoClosingBracket()
    {
        // Arrange
        const string source = "test[";
    
        // Act
        var tokens     = ParsingUtilities.ReadAllTokens(source);
        var index      = 0;
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(3, tokens.Length);
        Assert.Equal(2, index);
        Assert.IsType<InvalidExpression>(expression);

        var invalid = (InvalidExpression)expression;
        Assert.Single(invalid.Tokens);
        Assert.Equal(TokenType.RightBracket, invalid.Tokens[0].Type);
        Assert.Null(invalid.Expression);
        Assert.NotNull(invalid.ExpressionType);
        Assert.Equal(typeof(IndexerExpression), invalid.ExpressionType);
    }
    
    [Fact]
    public void ParseExpression_Indexer_NoClosingBracketWithIndex()
    {
        // Arrange
        const string source = "test[1";
    
        // Act
        var tokens     = ParsingUtilities.ReadAllTokens(source);
        var index      = 0;
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(4, tokens.Length);
        Assert.Equal(3, index);
        Assert.IsType<InvalidExpression>(expression);

        var invalid = (InvalidExpression)expression;
        Assert.Empty(invalid.Tokens);
        Assert.NotNull(invalid.Expression);
        Assert.IsType<IndexerExpression>(invalid.Expression);
        Assert.NotNull(invalid.ExpressionType);
        Assert.Equal(typeof(IndexerExpression), invalid.ExpressionType);
    
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
    public void ParseExpression_Indexer_ReservedKeywordFound()
    {
        // Arrange
        const string source = """
                              test[1 + 2
                              var t = 1;
                              """;
    
        // Act
        var tokens     = ParsingUtilities.ReadAllTokens(source);
        var index      = 0;
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(11, tokens.Length);
        Assert.Equal(5, index);
        Assert.IsType<InvalidExpression>(expression);
        
        var invalid = (InvalidExpression)expression;
        Assert.Empty(invalid.Tokens);
        Assert.NotNull(invalid.Expression);
        Assert.IsType<IndexerExpression>(invalid.Expression);
        Assert.NotNull(invalid.ExpressionType);
        Assert.Equal(typeof(IndexerExpression), invalid.ExpressionType);
    
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