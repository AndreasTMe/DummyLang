using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Expressions;
using DummyLang.SyntacticAnalysis.Parsers;
using DummyLang.SyntacticAnalysis.Statements;
using DummyLang.SyntacticAnalysis.Utilities;
using Xunit;

namespace DummyLang.SyntacticAnalysis.Tests.Statements;

public class WhileSyntaxParserTests
{
    [Fact]
    public void ParseStatement_While_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source = """
                              while (a > 0) {
                              }
                              """;

        // Act
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(8, index);
        Assert.Equal(TokenType.Eof, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<WhileStatement>(statement);
        var whileStatement = (WhileStatement)statement;

        Assert.Equal(TokenType.While, whileStatement.WhileKeyword.Type);
        Assert.Equal("while", whileStatement.WhileKeyword.Value);
        Assert.Equal(TokenType.None, whileStatement.Label.Type);
        Assert.Equal(TokenType.LeftParenthesis, whileStatement.LeftParenthesis.Type);
        Assert.Equal("(", whileStatement.LeftParenthesis.Value);
        Assert.IsType<BinaryExpression>(whileStatement.Condition);
        Assert.Equal(TokenType.RightParenthesis, whileStatement.RightParenthesis.Type);
        Assert.Equal(")", whileStatement.RightParenthesis.Value);
        Assert.NotNull(whileStatement.Block);
    }
    
    [Fact]
    public void ParseStatement_WhileWithLabel_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source = """
                              while test (a > 0) {
                              }
                              """;

        // Act
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(9, index);
        Assert.Equal(TokenType.Eof, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<WhileStatement>(statement);
        var whileStatement = (WhileStatement)statement;

        Assert.Equal(TokenType.While, whileStatement.WhileKeyword.Type);
        Assert.Equal("while", whileStatement.WhileKeyword.Value);
        Assert.Equal(TokenType.Identifier, whileStatement.Label.Type);
        Assert.Equal("test", whileStatement.Label.Value);
        Assert.Equal(TokenType.LeftParenthesis, whileStatement.LeftParenthesis.Type);
        Assert.Equal("(", whileStatement.LeftParenthesis.Value);
        Assert.IsType<BinaryExpression>(whileStatement.Condition);
        Assert.Equal(TokenType.RightParenthesis, whileStatement.RightParenthesis.Type);
        Assert.Equal(")", whileStatement.RightParenthesis.Value);
        Assert.NotNull(whileStatement.Block);
    }
    
    [Fact]
    public void ParseStatement_WhileNoLeftParenthesis_ShouldThrowLanguageSyntaxException()
    {
        // Arrange
        const string source = """
                              while 0 < a) {
                              }
                              """;

        // Act
        var tokens = ParsingUtilities.ReadAllTokens(source);
        var index  = 0;

        // Assert
        var exception = Assert.Throws<LanguageSyntaxException>(() => StatementParser.Parse(ref index, in tokens));
        Assert.Equal(TokenType.LeftParenthesis, exception.Expected);
        Assert.Equal(TokenType.Integer, exception.Found.Type);
        Assert.Equal("0", exception.Found.Value);
    }
    
    [Fact]
    public void ParseStatement_WhileNoCondition_ShouldThrowLanguageSyntaxException()
    {
        // Arrange
        const string source = """
                              while () {
                              }
                              """;

        // Act
        var tokens = ParsingUtilities.ReadAllTokens(source);
        var index  = 0;

        // Assert
        var exception = Assert.Throws<LanguageSyntaxException>(() => StatementParser.Parse(ref index, in tokens));
        Assert.Equal(TokenType.RightParenthesis, exception.Found.Type);
        Assert.Equal(")", exception.Found.Value);
    }
    
    [Fact]
    public void ParseStatement_WhileNoRightParenthesis_ShouldThrowLanguageSyntaxException()
    {
        // Arrange
        const string source = """
                              while (a > 0 {
                              }
                              """;

        // Act
        var tokens = ParsingUtilities.ReadAllTokens(source);
        var index  = 0;

        // Assert
        var exception = Assert.Throws<LanguageSyntaxException>(() => StatementParser.Parse(ref index, in tokens));
        Assert.Equal(TokenType.RightParenthesis, exception.Expected);
        Assert.Equal(TokenType.LeftBrace, exception.Found.Type);
        Assert.Equal("{", exception.Found.Value);
    }
    
    [Fact]
    public void ParseStatement_WhileNoLeftBrace_ShouldThrowLanguageSyntaxException()
    {
        // Arrange
        const string source = """
                              while (a > 0)
                              }
                              """;

        // Act
        var tokens = ParsingUtilities.ReadAllTokens(source);
        var index  = 0;

        // Assert
        var exception = Assert.Throws<LanguageSyntaxException>(() => StatementParser.Parse(ref index, in tokens));
        Assert.Equal(TokenType.LeftBrace, exception.Expected);
        Assert.Equal(TokenType.RightBrace, exception.Found.Type);
        Assert.Equal("}", exception.Found.Value);
    }
    
    [Fact]
    public void ParseStatement_WhileLabelNoLeftParenthesis_ShouldThrowLanguageSyntaxException()
    {
        // Arrange
        const string source = """
                              while test a > 0)
                              }
                              """;

        // Act
        var tokens = ParsingUtilities.ReadAllTokens(source);
        var index  = 0;

        // Assert
        var exception = Assert.Throws<LanguageSyntaxException>(() => StatementParser.Parse(ref index, in tokens));
        Assert.Equal(TokenType.LeftParenthesis, exception.Expected);
        Assert.Equal(TokenType.Identifier, exception.Found.Type);
        Assert.Equal("a", exception.Found.Value);
    }
}