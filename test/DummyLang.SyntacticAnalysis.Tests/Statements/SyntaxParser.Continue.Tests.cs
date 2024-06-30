using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Parsers;
using DummyLang.SyntacticAnalysis.Statements;
using DummyLang.SyntacticAnalysis.Utilities;
using Xunit;

namespace DummyLang.SyntacticAnalysis.Tests.Statements;

public class ContinueSyntaxParserTests
{
    [Fact]
    public void ParseStatement_Continue_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source = "continue;";

        // Act
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(2, index);
        Assert.Equal(TokenType.Eof, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<ContinueStatement>(statement);
        var continueStatement = (ContinueStatement)statement;

        Assert.Equal(TokenType.Continue, continueStatement.Continue.Type);
        Assert.Equal("continue", continueStatement.Continue.Value);

        Assert.Equal(TokenType.None, continueStatement.Label.Type);
        Assert.Equal("", continueStatement.Label.Value);

        Assert.Equal(TokenType.Semicolon, continueStatement.Terminator.Type);
        Assert.Equal(";", continueStatement.Terminator.Value);
    }

    [Fact]
    public void ParseStatement_ContinueWithLabel_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source = "continue someLabel;";

        // Act
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(3, index);
        Assert.Equal(TokenType.Eof, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<ContinueStatement>(statement);
        var continueStatement = (ContinueStatement)statement;

        Assert.Equal(TokenType.Continue, continueStatement.Continue.Type);
        Assert.Equal("continue", continueStatement.Continue.Value);

        Assert.Equal(TokenType.Identifier, continueStatement.Label.Type);
        Assert.Equal("someLabel", continueStatement.Label.Value);

        Assert.Equal(TokenType.Semicolon, continueStatement.Terminator.Type);
        Assert.Equal(";", continueStatement.Terminator.Value);
    }

    [Fact]
    public void ParseStatement_ContinueInvalidLabel_ShouldThrowLanguageSyntaxException()
    {
        // Arrange
        const string source = "continue 1;";

        // Act
        var tokens = ParsingUtilities.ReadAllTokens(source);
        var index  = 0;

        // Assert
        var exception = Assert.Throws<LanguageSyntaxException>(() => StatementParser.Parse(ref index, in tokens));
        Assert.Equal(TokenType.Semicolon, exception.Expected);
        Assert.Equal(TokenType.Integer, exception.Found.Type);
        Assert.Equal("1", exception.Found.Value);
    }

    [Fact]
    public void ParseStatement_ContinueNoSemicolon_ShouldThrowLanguageSyntaxException()
    {
        // Arrange
        const string source = "continue";

        // Act
        var tokens = ParsingUtilities.ReadAllTokens(source);
        var index  = 0;

        // Assert
        var exception = Assert.Throws<LanguageSyntaxException>(() => StatementParser.Parse(ref index, in tokens));
        Assert.Equal(TokenType.Semicolon, exception.Expected);
        Assert.Equal(TokenType.Eof, exception.Found.Type);
    }
}