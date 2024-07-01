using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Parsers;
using DummyLang.SyntacticAnalysis.Statements;
using DummyLang.SyntacticAnalysis.Utilities;
using Xunit;

namespace DummyLang.SyntacticAnalysis.Tests.Statements;

public class BreakSyntaxParserTests
{
    [Fact]
    public void ParseStatement_Break_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source = "break;";

        // Act
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(2, index);
        Assert.Equal(TokenType.Eof, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<BreakStatement>(statement);
        var breakStatement = (BreakStatement)statement;

        Assert.Equal(TokenType.Break, breakStatement.BreakKeyword.Type);
        Assert.Equal("break", breakStatement.BreakKeyword.Value);

        Assert.Equal(TokenType.None, breakStatement.Label.Type);
        Assert.Equal("", breakStatement.Label.Value);

        Assert.Equal(TokenType.Semicolon, breakStatement.Terminator.Type);
        Assert.Equal(";", breakStatement.Terminator.Value);
    }

    [Fact]
    public void ParseStatement_BreakWithLabel_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source = "break someLabel;";

        // Act
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(3, index);
        Assert.Equal(TokenType.Eof, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<BreakStatement>(statement);
        var breakStatement = (BreakStatement)statement;

        Assert.Equal(TokenType.Break, breakStatement.BreakKeyword.Type);
        Assert.Equal("break", breakStatement.BreakKeyword.Value);

        Assert.Equal(TokenType.Identifier, breakStatement.Label.Type);
        Assert.Equal("someLabel", breakStatement.Label.Value);

        Assert.Equal(TokenType.Semicolon, breakStatement.Terminator.Type);
        Assert.Equal(";", breakStatement.Terminator.Value);
    }

    [Fact]
    public void ParseStatement_BreakInvalidLabel_ShouldThrowLanguageSyntaxException()
    {
        // Arrange
        const string source = "break 1;";

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
    public void ParseStatement_BreakNoSemicolon_ShouldThrowLanguageSyntaxException()
    {
        // Arrange
        const string source = "break";

        // Act
        var tokens = ParsingUtilities.ReadAllTokens(source);
        var index  = 0;

        // Assert
        var exception = Assert.Throws<LanguageSyntaxException>(() => StatementParser.Parse(ref index, in tokens));
        Assert.Equal(TokenType.Semicolon, exception.Expected);
        Assert.Equal(TokenType.Eof, exception.Found.Type);
    }
}