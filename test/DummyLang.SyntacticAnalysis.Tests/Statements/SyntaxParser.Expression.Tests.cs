using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Expressions;
using DummyLang.SyntacticAnalysis.Parsers;
using DummyLang.SyntacticAnalysis.Statements;
using DummyLang.SyntacticAnalysis.Utilities;
using DummyLang.SyntacticAnalysis.Visitors;
using Xunit;

namespace DummyLang.SyntacticAnalysis.Tests.Statements;

public class ExpressionSyntaxParserTests
{
    [Fact]
    public void ParseStatement_Expression_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source    = "foo = 1 + 2;";
        var          tokens    = ParsingUtilities.ReadAllTokens(source);
        var          index     = 0;
        var          validator = new SyntaxNodeValidationVisitor();

        // Act
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(6, index);
        Assert.Equal(TokenType.Eof, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<ExpressionStatement>(statement);
        var variableAssignment = (ExpressionStatement)statement;

        Assert.IsType<BinaryExpression>(variableAssignment.Expression);

        Assert.Equal(TokenType.Semicolon, variableAssignment.Terminator.Type);
        Assert.Equal(";", variableAssignment.Terminator.Value);
        
        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.False(validator.HasErrors);
    }
    
    [Fact]
    public void ParseStatement_ExpressionPlusEqual_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source    = "foo += 1 + 2;";
        var          tokens    = ParsingUtilities.ReadAllTokens(source);
        var          index     = 0;
        var          validator = new SyntaxNodeValidationVisitor();

        // Act
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(6, index);
        Assert.Equal(TokenType.Eof, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<ExpressionStatement>(statement);
        var variableAssignment = (ExpressionStatement)statement;

        Assert.IsType<BinaryExpression>(variableAssignment.Expression);

        Assert.Equal(TokenType.Semicolon, variableAssignment.Terminator.Type);
        Assert.Equal(";", variableAssignment.Terminator.Value);
        
        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.False(validator.HasErrors);
    }
    
    [Fact]
    public void ParseStatement_ExpressionInvalidToken_StopAtColon()
    {
        // Arrange
        const string source    = "foo := 1 + 2;";
        var          tokens    = ParsingUtilities.ReadAllTokens(source);
        var          index     = 0;
        var          validator = new SyntaxNodeValidationVisitor();

        // Act
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(1, index);
        Assert.Equal(TokenType.Colon, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<ExpressionStatement>(statement);
        var variableAssignment = (ExpressionStatement)statement;

        Assert.IsType<IdentifierExpression>(variableAssignment.Expression);

        Assert.Equal(TokenType.None, variableAssignment.Terminator.Type);
        Assert.Equal("", variableAssignment.Terminator.Value);
        
        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(
            validator.Diagnostics,
            d => d.Message.EndsWith(ExpressionStatement.SemicolonExpected));
    }
    
    [Fact]
    public void ParseStatement_ExpressionNoSemicolon1_SemicolonExpected()
    {
        // Arrange
        const string source    = "foo = 1 + 2";
        var          tokens    = ParsingUtilities.ReadAllTokens(source);
        var          index     = 0;
        var          validator = new SyntaxNodeValidationVisitor();

        // Act
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(5, index);
        Assert.Equal(TokenType.Eof, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<ExpressionStatement>(statement);
        var variableAssignment = (ExpressionStatement)statement;

        Assert.IsType<BinaryExpression>(variableAssignment.Expression);

        Assert.Equal(TokenType.None, variableAssignment.Terminator.Type);
        Assert.Equal("", variableAssignment.Terminator.Value);
        
        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(
            validator.Diagnostics,
            d => d.Message.EndsWith(ExpressionStatement.SemicolonExpected));
    }
    
    [Fact]
    public void ParseStatement_ExpressionNoSemicolon2_SemicolonExpected()
    {
        // Arrange
        const string source = """
                              foo = 1 + 2
                              var test := 1;
                              """;
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var validator = new SyntaxNodeValidationVisitor();

        // Act
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(5, index);
        Assert.Equal(TokenType.Var, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<ExpressionStatement>(statement);
        var variableAssignment = (ExpressionStatement)statement;

        Assert.IsType<BinaryExpression>(variableAssignment.Expression);

        Assert.Equal(TokenType.None, variableAssignment.Terminator.Type);
        Assert.Equal("", variableAssignment.Terminator.Value);
        
        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(
            validator.Diagnostics,
            d => d.Message.EndsWith(ExpressionStatement.SemicolonExpected));
    }
}