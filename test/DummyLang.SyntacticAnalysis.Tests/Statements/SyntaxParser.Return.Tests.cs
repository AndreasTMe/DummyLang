using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Expressions;
using DummyLang.SyntacticAnalysis.Parsers;
using DummyLang.SyntacticAnalysis.Statements;
using DummyLang.SyntacticAnalysis.Utilities;
using Xunit;

namespace DummyLang.SyntacticAnalysis.Tests.Statements;

public class ReturnSyntaxParserTests
{
    [Fact]
    public void ParseStatement_ReturnSingleValue_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source = "return 1;";

        // Act
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(3, index);
        Assert.Equal(TokenType.Eof, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<ReturnStatement>(statement);
        var returnStatement = (ReturnStatement)statement;

        Assert.Equal(TokenType.Return, returnStatement.ReturnKeyword.Type);
        Assert.Equal("return", returnStatement.ReturnKeyword.Value);

        Assert.NotNull(returnStatement.ReturnArguments);
        Assert.Single(returnStatement.ReturnArguments);
        Assert.IsType<NumberLiteralExpression>(returnStatement.ReturnArguments[0]);

        Assert.Equal(TokenType.Semicolon, returnStatement.Terminator.Type);
        Assert.Equal(";", returnStatement.Terminator.Value);
    }

    [Fact]
    public void ParseStatement_ReturnSingleExpression_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source = "return 1 + 2;";

        // Act
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(5, index);
        Assert.Equal(TokenType.Eof, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<ReturnStatement>(statement);
        var returnStatement = (ReturnStatement)statement;

        Assert.Equal(TokenType.Return, returnStatement.ReturnKeyword.Type);
        Assert.Equal("return", returnStatement.ReturnKeyword.Value);

        Assert.NotNull(returnStatement.ReturnArguments);
        Assert.Single(returnStatement.ReturnArguments);
        Assert.IsType<BinaryExpression>(returnStatement.ReturnArguments[0]);

        Assert.Equal(TokenType.Semicolon, returnStatement.Terminator.Type);
        Assert.Equal(";", returnStatement.Terminator.Value);
    }

    [Fact]
    public void ParseStatement_ReturnMultipleValues_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source = "return 1, 2 * foo, \"some string\";";

        // Act
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(9, index);
        Assert.Equal(TokenType.Eof, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<ReturnStatement>(statement);
        var returnStatement = (ReturnStatement)statement;

        Assert.Equal(TokenType.Return, returnStatement.ReturnKeyword.Type);
        Assert.Equal("return", returnStatement.ReturnKeyword.Value);

        Assert.NotNull(returnStatement.ReturnArguments);
        Assert.Equal(3, returnStatement.ReturnArguments.Count);
        Assert.IsType<NumberLiteralExpression>(returnStatement.ReturnArguments[0]);
        Assert.IsType<BinaryExpression>(returnStatement.ReturnArguments[1]);
        Assert.IsType<StringLiteralExpression>(returnStatement.ReturnArguments[2]);

        Assert.Equal(TokenType.Semicolon, returnStatement.Terminator.Type);
        Assert.Equal(";", returnStatement.Terminator.Value);
    }
    
    [Fact]
    public void ParseStatement_ReturnSingleValueNoSemicolon_ShouldThrowLanguageSyntaxException()
    {
        // Arrange
        const string source = "return foo";

        // Act
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;

        // Assert
        Assert.Throws<LanguageSyntaxException>(() => StatementParser.Parse(ref index, in tokens));
    }
    
    [Fact]
    public void ParseStatement_ReturnMultipleValuesNoSemicolon_ShouldThrowLanguageSyntaxException()
    {
        // Arrange
        const string source = "return foo, bar";

        // Act
        var tokens = ParsingUtilities.ReadAllTokens(source);
        var index  = 0;

        // Assert
        Assert.Throws<LanguageSyntaxException>(() => StatementParser.Parse(ref index, in tokens));
    }
    
    [Fact]
    public void ParseStatement_ReturnMultipleValuesNoComma1_ShouldThrowLanguageSyntaxException()
    {
        // Arrange
        const string source = "return foo bar";

        // Act
        var tokens = ParsingUtilities.ReadAllTokens(source);
        var index  = 0;

        // Assert
        Assert.Throws<LanguageSyntaxException>(() => StatementParser.Parse(ref index, in tokens));
    }
    
    [Fact]
    public void ParseStatement_ReturnMultipleValuesNoComma2_ShouldThrowLanguageSyntaxException()
    {
        // Arrange
        const string source = "return foo, 1 + 2 \"something\"";

        // Act
        var tokens = ParsingUtilities.ReadAllTokens(source);
        var index  = 0;

        // Assert
        Assert.Throws<LanguageSyntaxException>(() => StatementParser.Parse(ref index, in tokens));
    }
}