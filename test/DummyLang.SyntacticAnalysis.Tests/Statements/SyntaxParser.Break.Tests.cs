using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Parsers;
using DummyLang.SyntacticAnalysis.Statements;
using DummyLang.SyntacticAnalysis.Utilities;
using DummyLang.SyntacticAnalysis.Visitors;
using Xunit;

namespace DummyLang.SyntacticAnalysis.Tests.Statements;

public class BreakSyntaxParserTests
{
    [Fact]
    public void ParseStatement_Break_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source    = "break;";
        var          tokens    = ParsingUtilities.ReadAllTokens(source);
        var          index     = 0;
        var          validator = new SyntaxNodeValidationVisitor();

        // Act
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
        
        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.False(validator.HasErrors);
    }

    [Fact]
    public void ParseStatement_BreakWithLabel_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source    = "break someLabel;";
        var          tokens    = ParsingUtilities.ReadAllTokens(source);
        var          index     = 0;
        var          validator = new SyntaxNodeValidationVisitor();

        // Act
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
        
        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.False(validator.HasErrors);
    }

    [Fact]
    public void ParseStatement_BreakInvalidLabel_StopAtInvalidTokenSemicolonExpected()
    {
        // Arrange
        const string source    = "break 1;";
        var          tokens    = ParsingUtilities.ReadAllTokens(source);
        var          index     = 0;
        var          validator = new SyntaxNodeValidationVisitor();

        // Act
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(1, index);
        Assert.Equal(TokenType.Integer, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<BreakStatement>(statement);
        var breakStatement = (BreakStatement)statement;

        Assert.Equal(TokenType.Break, breakStatement.BreakKeyword.Type);
        Assert.Equal("break", breakStatement.BreakKeyword.Value);

        Assert.Equal(TokenType.None, breakStatement.Label.Type);
        Assert.Equal("", breakStatement.Label.Value);

        Assert.Equal(TokenType.None, breakStatement.Terminator.Type);
        Assert.Equal("", breakStatement.Terminator.Value);
        
        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(
            validator.Diagnostics,
            d => d.Message.EndsWith(BreakStatement.SemicolonExpected));
    }

    [Fact]
    public void ParseStatement_BreakNoSemicolon_SemicolonExpected()
    {
        // Arrange
        const string source    = "break";
        var          tokens    = ParsingUtilities.ReadAllTokens(source);
        var          index     = 0;
        var          validator = new SyntaxNodeValidationVisitor();

        // Act
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(1, index);
        Assert.Equal(TokenType.Eof, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<BreakStatement>(statement);
        var breakStatement = (BreakStatement)statement;

        Assert.Equal(TokenType.Break, breakStatement.BreakKeyword.Type);
        Assert.Equal("break", breakStatement.BreakKeyword.Value);

        Assert.Equal(TokenType.None, breakStatement.Label.Type);
        Assert.Equal("", breakStatement.Label.Value);

        Assert.Equal(TokenType.None, breakStatement.Terminator.Type);
        Assert.Equal("", breakStatement.Terminator.Value);
        
        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(
            validator.Diagnostics,
            d => d.Message.EndsWith(BreakStatement.SemicolonExpected));
    }
}