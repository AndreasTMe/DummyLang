using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Expressions;
using DummyLang.SyntacticAnalysis.Parsers;
using DummyLang.SyntacticAnalysis.Statements;
using DummyLang.SyntacticAnalysis.Utilities;
using DummyLang.SyntacticAnalysis.Visitors;
using Xunit;

namespace DummyLang.SyntacticAnalysis.Tests.Statements;

public class ReturnSyntaxParserTests
{
    [Fact]
    public void ParseStatement_ReturnSingleValue_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source    = "return 1;";
        var          tokens    = ParsingUtilities.ReadAllTokens(source);
        var          index     = 0;
        var          validator = new SyntaxNodeValidationVisitor();

        // Act
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
        Assert.IsType<ArgumentExpression>(returnStatement.ReturnArguments[0]);

        Assert.Equal(TokenType.Semicolon, returnStatement.Terminator.Type);
        Assert.Equal(";", returnStatement.Terminator.Value);

        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.False(validator.HasErrors);
    }

    [Fact]
    public void ParseStatement_ReturnSingleExpression_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source    = "return 1 + 2;";
        var          tokens    = ParsingUtilities.ReadAllTokens(source);
        var          index     = 0;
        var          validator = new SyntaxNodeValidationVisitor();

        // Act
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
        Assert.IsType<ArgumentExpression>(returnStatement.ReturnArguments[0]);

        Assert.Equal(TokenType.Semicolon, returnStatement.Terminator.Type);
        Assert.Equal(";", returnStatement.Terminator.Value);

        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.False(validator.HasErrors);
    }

    [Fact]
    public void ParseStatement_ReturnMultipleValues_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source    = "return 1, 2 * foo, \"some string\";";
        var          tokens    = ParsingUtilities.ReadAllTokens(source);
        var          index     = 0;
        var          validator = new SyntaxNodeValidationVisitor();

        // Act
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
        Assert.IsType<ArgumentExpression>(returnStatement.ReturnArguments[0]);
        Assert.IsType<ArgumentExpression>(returnStatement.ReturnArguments[1]);
        Assert.IsType<ArgumentExpression>(returnStatement.ReturnArguments[2]);

        Assert.Equal(TokenType.Semicolon, returnStatement.Terminator.Type);
        Assert.Equal(";", returnStatement.Terminator.Value);

        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.False(validator.HasErrors);
    }

    [Fact]
    public void ParseStatement_ReturnSingleValueNoSemicolon_SemicolonExpected()
    {
        // Arrange
        const string source    = "return foo";
        var          tokens    = ParsingUtilities.ReadAllTokens(source);
        var          index     = 0;
        var          validator = new SyntaxNodeValidationVisitor();

        // Act
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(2, index);
        Assert.Equal(TokenType.Eof, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<ReturnStatement>(statement);
        var returnStatement = (ReturnStatement)statement;

        Assert.Equal(TokenType.Return, returnStatement.ReturnKeyword.Type);
        Assert.Equal("return", returnStatement.ReturnKeyword.Value);

        Assert.NotNull(returnStatement.ReturnArguments);
        Assert.Single(returnStatement.ReturnArguments);
        Assert.IsType<ArgumentExpression>(returnStatement.ReturnArguments[0]);

        Assert.Equal(TokenType.None, returnStatement.Terminator.Type);
        Assert.Equal("", returnStatement.Terminator.Value);

        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(
            validator.Diagnostics,
            d => d.Message.EndsWith(ReturnStatement.SemicolonExpected));
    }

    [Fact]
    public void ParseStatement_ReturnMultipleValuesNoSemicolon_SemicolonExpected()
    {
        // Arrange
        const string source    = "return foo, bar";
        var          tokens    = ParsingUtilities.ReadAllTokens(source);
        var          index     = 0;
        var          validator = new SyntaxNodeValidationVisitor();

        // Act
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(4, index);
        Assert.Equal(TokenType.Eof, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<ReturnStatement>(statement);
        var returnStatement = (ReturnStatement)statement;

        Assert.Equal(TokenType.Return, returnStatement.ReturnKeyword.Type);
        Assert.Equal("return", returnStatement.ReturnKeyword.Value);

        Assert.NotNull(returnStatement.ReturnArguments);
        Assert.Equal(2, returnStatement.ReturnArguments.Count);
        Assert.IsType<ArgumentExpression>(returnStatement.ReturnArguments[0]);
        Assert.IsType<ArgumentExpression>(returnStatement.ReturnArguments[1]);

        Assert.Equal(TokenType.None, returnStatement.Terminator.Type);
        Assert.Equal("", returnStatement.Terminator.Value);

        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(
            validator.Diagnostics,
            d => d.Message.EndsWith(ReturnStatement.SemicolonExpected));
    }

    [Fact]
    public void ParseStatement_ReturnMultipleValuesNoComma1_StopAtMissingCommaSemicolonExpected()
    {
        // Arrange
        const string source    = "return foo bar";
        var          tokens    = ParsingUtilities.ReadAllTokens(source);
        var          index     = 0;
        var          validator = new SyntaxNodeValidationVisitor();

        // Act
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(2, index);
        Assert.Equal(TokenType.Identifier, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<ReturnStatement>(statement);
        var returnStatement = (ReturnStatement)statement;

        Assert.Equal(TokenType.Return, returnStatement.ReturnKeyword.Type);
        Assert.Equal("return", returnStatement.ReturnKeyword.Value);

        Assert.NotNull(returnStatement.ReturnArguments);
        Assert.Single(returnStatement.ReturnArguments);
        Assert.IsType<ArgumentExpression>(returnStatement.ReturnArguments[0]);

        Assert.Equal(TokenType.None, returnStatement.Terminator.Type);
        Assert.Equal("", returnStatement.Terminator.Value);

        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(
            validator.Diagnostics,
            d => d.Message.EndsWith(ReturnStatement.SemicolonExpected));
    }

    [Fact]
    public void ParseStatement_ReturnMultipleValuesNoComma2_StopAtMissingCommaSemicolonExpected()
    {
        // Arrange
        const string source    = "return foo, 1 + 2 \"something\"";
        var          tokens    = ParsingUtilities.ReadAllTokens(source);
        var          index     = 0;
        var          validator = new SyntaxNodeValidationVisitor();

        // Act
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(6, index);
        Assert.Equal(TokenType.String, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<ReturnStatement>(statement);
        var returnStatement = (ReturnStatement)statement;

        Assert.Equal(TokenType.Return, returnStatement.ReturnKeyword.Type);
        Assert.Equal("return", returnStatement.ReturnKeyword.Value);

        Assert.NotNull(returnStatement.ReturnArguments);
        Assert.Equal(2, returnStatement.ReturnArguments.Count);
        Assert.IsType<ArgumentExpression>(returnStatement.ReturnArguments[0]);
        Assert.IsType<ArgumentExpression>(returnStatement.ReturnArguments[1]);

        Assert.Equal(TokenType.None, returnStatement.Terminator.Type);
        Assert.Equal("", returnStatement.Terminator.Value);

        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(
            validator.Diagnostics,
            d => d.Message.EndsWith(ReturnStatement.SemicolonExpected));
    }
}