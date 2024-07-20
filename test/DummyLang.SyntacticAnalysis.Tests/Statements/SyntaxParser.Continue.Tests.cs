using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Parsers;
using DummyLang.SyntacticAnalysis.Statements;
using DummyLang.SyntacticAnalysis.Utilities;
using DummyLang.SyntacticAnalysis.Visitors;
using Xunit;

namespace DummyLang.SyntacticAnalysis.Tests.Statements;

public class ContinueSyntaxParserTests
{
    [Fact]
    public void ParseStatement_Continue_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source    = "continue;";
        var          tokens    = ParsingUtilities.ReadAllTokens(source);
        var          index     = 0;
        var          validator = new SyntaxNodeValidationVisitor();

        // Act
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(2, index);
        Assert.Equal(TokenType.Eof, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<ContinueStatement>(statement);
        var continueStatement = (ContinueStatement)statement;

        Assert.Equal(TokenType.Continue, continueStatement.ContinueKeyword.Type);
        Assert.Equal("continue", continueStatement.ContinueKeyword.Value);

        Assert.Equal(TokenType.None, continueStatement.Label.Type);
        Assert.Equal("", continueStatement.Label.Value);

        Assert.Equal(TokenType.Semicolon, continueStatement.Terminator.Type);
        Assert.Equal(";", continueStatement.Terminator.Value);
        
        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.False(validator.HasErrors);
    }

    [Fact]
    public void ParseStatement_ContinueWithLabel_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source    = "continue someLabel;";
        var          tokens    = ParsingUtilities.ReadAllTokens(source);
        var          index     = 0;
        var          validator = new SyntaxNodeValidationVisitor();

        // Act
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(3, index);
        Assert.Equal(TokenType.Eof, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<ContinueStatement>(statement);
        var continueStatement = (ContinueStatement)statement;

        Assert.Equal(TokenType.Continue, continueStatement.ContinueKeyword.Type);
        Assert.Equal("continue", continueStatement.ContinueKeyword.Value);

        Assert.Equal(TokenType.Identifier, continueStatement.Label.Type);
        Assert.Equal("someLabel", continueStatement.Label.Value);

        Assert.Equal(TokenType.Semicolon, continueStatement.Terminator.Type);
        Assert.Equal(";", continueStatement.Terminator.Value);
        
        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.False(validator.HasErrors);
    }

    [Fact]
    public void ParseStatement_ContinueInvalidLabel_StopAtInvalidTokenSemicolonExpected()
    {
        // Arrange
        const string source    = "continue 1;";
        var          tokens    = ParsingUtilities.ReadAllTokens(source);
        var          index     = 0;
        var          validator = new SyntaxNodeValidationVisitor();

        // Act
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(1, index);
        Assert.Equal(TokenType.Integer, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<ContinueStatement>(statement);
        var continueStatement = (ContinueStatement)statement;

        Assert.Equal(TokenType.Continue, continueStatement.ContinueKeyword.Type);
        Assert.Equal("continue", continueStatement.ContinueKeyword.Value);

        Assert.Equal(TokenType.None, continueStatement.Label.Type);
        Assert.Equal("", continueStatement.Label.Value);

        Assert.Equal(TokenType.None, continueStatement.Terminator.Type);
        Assert.Equal("", continueStatement.Terminator.Value);
        
        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(
            validator.Diagnostics,
            d => d.Message.EndsWith(BreakStatement.SemicolonExpected));
    }

    [Fact]
    public void ParseStatement_ContinueNoSemicolon_SemicolonExpected()
    {
        // Arrange
        const string source    = "continue";
        var          tokens    = ParsingUtilities.ReadAllTokens(source);
        var          index     = 0;
        var          validator = new SyntaxNodeValidationVisitor();

        // Act
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(1, index);
        Assert.Equal(TokenType.Eof, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<ContinueStatement>(statement);
        var continueStatement = (ContinueStatement)statement;

        Assert.Equal(TokenType.Continue, continueStatement.ContinueKeyword.Type);
        Assert.Equal("continue", continueStatement.ContinueKeyword.Value);

        Assert.Equal(TokenType.None, continueStatement.Label.Type);
        Assert.Equal("", continueStatement.Label.Value);

        Assert.Equal(TokenType.None, continueStatement.Terminator.Type);
        Assert.Equal("", continueStatement.Terminator.Value);
        
        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(
            validator.Diagnostics,
            d => d.Message.EndsWith(BreakStatement.SemicolonExpected));
    }
}