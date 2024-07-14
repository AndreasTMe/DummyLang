using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Expressions;
using DummyLang.SyntacticAnalysis.Parsers;
using DummyLang.SyntacticAnalysis.Utilities;
using DummyLang.SyntacticAnalysis.Visitors;
using Xunit;

namespace DummyLang.SyntacticAnalysis.Tests.Expressions;

public class IndexerSyntaxParserTests
{
    [Fact]
    public void ParseExpression_Indexer_ReadSuccessfully()
    {
        // Arrange
        const string source    = "test[1]";
        var          validator = new SyntaxNodeValidationVisitor();
        var          tokens    = ParsingUtilities.ReadAllTokens(source);
        var          index     = 0;

        // Act
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

        Assert.False(validator.HasErrors);
        expression.Accept(validator);
        Assert.False(validator.HasErrors);
    }

    [Fact]
    public void ParseExpression_Indexer_NoIndex()
    {
        // Arrange
        const string source    = "test[]";
        var          validator = new SyntaxNodeValidationVisitor();
        var          tokens    = ParsingUtilities.ReadAllTokens(source);
        var          index     = 0;

        // Act
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

        Assert.False(validator.HasErrors);
        expression.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(validator.Diagnostics, d => d.Message.EndsWith(IndexerExpression.IndexerExpected));
    }

    [Fact]
    public void ParseExpression_Indexer_NoIndexer()
    {
        // Arrange
        const string source    = "test[";
        var          validator = new SyntaxNodeValidationVisitor();
        var          tokens    = ParsingUtilities.ReadAllTokens(source);
        var          index     = 0;

        // Act
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(3, tokens.Length);
        Assert.Equal(2, index);
        Assert.IsType<PrimaryExpression>(expression);

        var primary = (PrimaryExpression)expression;
        Assert.Equal(Token.None, primary.Token);
        Assert.IsType<IndexerExpression>(primary.Expression);

        var indexer = (IndexerExpression)primary.Expression;
        Assert.Equal(TokenType.Identifier, indexer.Identifier.Type);
        Assert.Equal("test", indexer.Identifier.Value);

        Assert.Equal(TokenType.LeftBracket, indexer.LeftBracket.Type);
        Assert.Equal("[", indexer.LeftBracket.Value);

        Assert.Equal(TokenType.None, indexer.RightBracket.Type);
        Assert.Equal("", indexer.RightBracket.Value);

        Assert.Null(indexer.Index);

        Assert.False(validator.HasErrors);
        expression.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(validator.Diagnostics, d => d.Message.EndsWith(IndexerExpression.IndexerExpected));
    }

    [Fact]
    public void ParseExpression_Indexer_NoClosingBracketWithIndex()
    {
        // Arrange
        const string source    = "test[1";
        var          validator = new SyntaxNodeValidationVisitor();
        var          tokens    = ParsingUtilities.ReadAllTokens(source);
        var          index     = 0;

        // Act
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

        Assert.Equal(TokenType.None, indexer.RightBracket.Type);
        Assert.Equal("", indexer.RightBracket.Value);

        Assert.NotNull(indexer.Index);
        Assert.IsType<NumberLiteralExpression>(indexer.Index);

        Assert.False(validator.HasErrors);
        expression.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(validator.Diagnostics, d => d.Message.EndsWith(IndexerExpression.RightBracketExpected));
    }

    [Fact]
    public void ParseExpression_Indexer_ReservedKeywordFound()
    {
        // Arrange
        const string source = """
                              test[1 + 2
                              var t = 1;
                              """;
        var validator = new SyntaxNodeValidationVisitor();
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;

        // Act
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(11, tokens.Length);
        Assert.Equal(5, index);
        Assert.IsType<PrimaryExpression>(expression);

        var primary = (PrimaryExpression)expression;
        Assert.Equal(Token.None, primary.Token);
        Assert.IsType<IndexerExpression>(primary.Expression);

        var indexer = (IndexerExpression)primary.Expression;
        Assert.Equal(TokenType.Identifier, indexer.Identifier.Type);
        Assert.Equal("test", indexer.Identifier.Value);

        Assert.Equal(TokenType.LeftBracket, indexer.LeftBracket.Type);
        Assert.Equal("[", indexer.LeftBracket.Value);

        Assert.Equal(TokenType.None, indexer.RightBracket.Type);
        Assert.Equal("", indexer.RightBracket.Value);

        Assert.NotNull(indexer.Index);
        Assert.IsType<BinaryExpression>(indexer.Index);

        Assert.False(validator.HasErrors);
        expression.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(validator.Diagnostics, d => d.Message.EndsWith(IndexerExpression.RightBracketExpected));
    }
}