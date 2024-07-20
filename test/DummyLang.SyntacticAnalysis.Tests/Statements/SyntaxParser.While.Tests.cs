using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Expressions;
using DummyLang.SyntacticAnalysis.Parsers;
using DummyLang.SyntacticAnalysis.Statements;
using DummyLang.SyntacticAnalysis.Utilities;
using DummyLang.SyntacticAnalysis.Visitors;
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
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var validator = new SyntaxNodeValidationVisitor();

        // Act
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

        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.False(validator.HasErrors);
    }

    [Fact]
    public void ParseStatement_WhileWithLabel_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source = """
                              while test (a > 0) {
                              }
                              """;
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var validator = new SyntaxNodeValidationVisitor();

        // Act
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

        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.False(validator.HasErrors);
    }

    [Fact]
    public void ParseStatement_WhileNoLeftParenthesis_LeftParenthesisExpected()
    {
        // Arrange
        const string source = """
                              while 0 < a) {
                              }
                              """;
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var validator = new SyntaxNodeValidationVisitor();

        // Act
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(7, index);
        Assert.Equal(TokenType.Eof, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<WhileStatement>(statement);
        var whileStatement = (WhileStatement)statement;

        Assert.Equal(TokenType.While, whileStatement.WhileKeyword.Type);
        Assert.Equal("while", whileStatement.WhileKeyword.Value);
        Assert.Equal(TokenType.None, whileStatement.Label.Type);
        Assert.Equal(TokenType.None, whileStatement.LeftParenthesis.Type);
        Assert.Equal("", whileStatement.LeftParenthesis.Value);
        Assert.IsType<BinaryExpression>(whileStatement.Condition);
        Assert.Equal(TokenType.RightParenthesis, whileStatement.RightParenthesis.Type);
        Assert.Equal(")", whileStatement.RightParenthesis.Value);
        Assert.NotNull(whileStatement.Block);

        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(
            validator.Diagnostics,
            d => d.Message.EndsWith(WhileStatement.LeftParenthesisExpected));
    }

    [Fact]
    public void ParseStatement_WhileNoCondition_ExpressionExpected()
    {
        // Arrange
        const string source = """
                              while () {
                              }
                              """;
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var validator = new SyntaxNodeValidationVisitor();

        // Act
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(5, index);
        Assert.Equal(TokenType.Eof, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<WhileStatement>(statement);
        var whileStatement = (WhileStatement)statement;

        Assert.Equal(TokenType.While, whileStatement.WhileKeyword.Type);
        Assert.Equal("while", whileStatement.WhileKeyword.Value);
        Assert.Equal(TokenType.None, whileStatement.Label.Type);
        Assert.Equal(TokenType.LeftParenthesis, whileStatement.LeftParenthesis.Type);
        Assert.Equal("(", whileStatement.LeftParenthesis.Value);
        Assert.Null(whileStatement.Condition);
        Assert.Equal(TokenType.RightParenthesis, whileStatement.RightParenthesis.Type);
        Assert.Equal(")", whileStatement.RightParenthesis.Value);
        Assert.NotNull(whileStatement.Block);

        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(
            validator.Diagnostics,
            d => d.Message.EndsWith(WhileStatement.ExpressionExpected));
    }

    [Fact]
    public void ParseStatement_WhileNoRightParenthesis_RightParenthesisExpected()
    {
        // Arrange
        const string source = """
                              while (a > 0 {
                              }
                              """;
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var validator = new SyntaxNodeValidationVisitor();

        // Act
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(7, index);
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
        Assert.Equal(TokenType.None, whileStatement.RightParenthesis.Type);
        Assert.Equal("", whileStatement.RightParenthesis.Value);
        Assert.NotNull(whileStatement.Block);

        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(
            validator.Diagnostics,
            d => d.Message.EndsWith(WhileStatement.RightParenthesisExpected));
    }

    [Fact]
    public void ParseStatement_WhileNoLeftBrace_WhileBlockExpected()
    {
        // Arrange
        const string source = """
                              while (a > 0)
                              }
                              """;
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var validator = new SyntaxNodeValidationVisitor();

        // Act
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(6, index);
        Assert.Equal(TokenType.RightBrace, tokens[index].Type);

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
        Assert.Null(whileStatement.Block);

        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(
            validator.Diagnostics,
            d => d.Message.EndsWith(WhileStatement.WhileBlockExpected));
    }

    [Fact]
    public void ParseStatement_WhileLabelNoLeftParenthesis_LeftParenthesisExpected()
    {
        // Arrange
        const string source = """
                              while test a > 0)
                              }
                              """;
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var validator = new SyntaxNodeValidationVisitor();

        // Act
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(6, index);
        Assert.Equal(TokenType.RightBrace, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<WhileStatement>(statement);
        var whileStatement = (WhileStatement)statement;

        Assert.Equal(TokenType.While, whileStatement.WhileKeyword.Type);
        Assert.Equal("while", whileStatement.WhileKeyword.Value);
        Assert.Equal(TokenType.Identifier, whileStatement.Label.Type);
        Assert.Equal("test", whileStatement.Label.Value);
        Assert.Equal(TokenType.None, whileStatement.LeftParenthesis.Type);
        Assert.Equal("", whileStatement.LeftParenthesis.Value);
        Assert.IsType<BinaryExpression>(whileStatement.Condition);
        Assert.Equal(TokenType.RightParenthesis, whileStatement.RightParenthesis.Type);
        Assert.Equal(")", whileStatement.RightParenthesis.Value);
        Assert.Null(whileStatement.Block);

        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(
            validator.Diagnostics,
            d => d.Message.EndsWith(WhileStatement.LeftParenthesisExpected));
    }
}