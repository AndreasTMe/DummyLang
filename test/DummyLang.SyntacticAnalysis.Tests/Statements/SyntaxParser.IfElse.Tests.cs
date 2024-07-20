using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Expressions;
using DummyLang.SyntacticAnalysis.Parsers;
using DummyLang.SyntacticAnalysis.Statements;
using DummyLang.SyntacticAnalysis.Utilities;
using DummyLang.SyntacticAnalysis.Visitors;
using Xunit;

namespace DummyLang.SyntacticAnalysis.Tests.Statements;

public class IfElseSyntaxParserTests
{
    [Fact]
    public void ParseStatement_IfCondition_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source = """
                              if (a > 0) {
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
        Assert.IsType<IfElseStatement>(statement);
        var ifElseStatement = (IfElseStatement)statement;

        Assert.NotNull(ifElseStatement.If);
        Assert.Equal(TokenType.If, ifElseStatement.If.IfKeyword.Type);
        Assert.Equal("if", ifElseStatement.If.IfKeyword.Value);
        Assert.Equal(TokenType.LeftParenthesis, ifElseStatement.If.LeftParenthesis.Type);
        Assert.Equal("(", ifElseStatement.If.LeftParenthesis.Value);
        Assert.IsType<BinaryExpression>(ifElseStatement.If.Condition);
        Assert.Equal(TokenType.RightParenthesis, ifElseStatement.If.RightParenthesis.Type);
        Assert.Equal(")", ifElseStatement.If.RightParenthesis.Value);
        Assert.NotNull(ifElseStatement.If.Block);

        Assert.Null(ifElseStatement.ElseIfs);
        Assert.Null(ifElseStatement.Else);

        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.False(validator.HasErrors);
    }

    [Fact]
    public void ParseStatement_IfConditionElseIfCondition_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source = """
                              if (a > 0) {
                              } else if (a < 0) {
                              }
                              """;
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var validator = new SyntaxNodeValidationVisitor();

        // Act
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(17, index);
        Assert.Equal(TokenType.Eof, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<IfElseStatement>(statement);
        var ifElseStatement = (IfElseStatement)statement;

        Assert.NotNull(ifElseStatement.If);
        Assert.Equal(TokenType.If, ifElseStatement.If.IfKeyword.Type);
        Assert.Equal("if", ifElseStatement.If.IfKeyword.Value);
        Assert.Equal(TokenType.LeftParenthesis, ifElseStatement.If.LeftParenthesis.Type);
        Assert.Equal("(", ifElseStatement.If.LeftParenthesis.Value);
        Assert.IsType<BinaryExpression>(ifElseStatement.If.Condition);
        Assert.Equal(TokenType.RightParenthesis, ifElseStatement.If.RightParenthesis.Type);
        Assert.Equal(")", ifElseStatement.If.RightParenthesis.Value);
        Assert.NotNull(ifElseStatement.If.Block);

        Assert.NotNull(ifElseStatement.ElseIfs);
        Assert.Single(ifElseStatement.ElseIfs);
        Assert.Equal(TokenType.Else, ifElseStatement.ElseIfs[0].ElseKeyword.Type);
        Assert.Equal("else", ifElseStatement.ElseIfs[0].ElseKeyword.Value);
        Assert.NotNull(ifElseStatement.ElseIfs);
        Assert.Single(ifElseStatement.ElseIfs);
        Assert.NotNull(ifElseStatement.ElseIfs[0].IfBlock);
        Assert.Equal(TokenType.If, ifElseStatement.ElseIfs[0].IfBlock!.IfKeyword.Type);
        Assert.Equal("if", ifElseStatement.ElseIfs[0].IfBlock!.IfKeyword.Value);
        Assert.Equal(TokenType.LeftParenthesis, ifElseStatement.ElseIfs[0].IfBlock!.LeftParenthesis.Type);
        Assert.Equal("(", ifElseStatement.ElseIfs[0].IfBlock!.LeftParenthesis.Value);
        Assert.IsType<BinaryExpression>(ifElseStatement.ElseIfs[0].IfBlock!.Condition);
        Assert.Equal(TokenType.RightParenthesis, ifElseStatement.ElseIfs[0].IfBlock!.RightParenthesis.Type);
        Assert.Equal(")", ifElseStatement.ElseIfs[0].IfBlock!.RightParenthesis.Value);
        Assert.NotNull(ifElseStatement.ElseIfs[0].IfBlock!.Block);

        Assert.Null(ifElseStatement.Else);

        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.False(validator.HasErrors);
    }

    [Fact]
    public void ParseStatement_IfConditionElseIfConditionElse_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source = """
                              if (a > 0) {
                              } else if (a < 0) {
                              } else {
                              }
                              """;
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var validator = new SyntaxNodeValidationVisitor();

        // Act
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(20, index);
        Assert.Equal(TokenType.Eof, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<IfElseStatement>(statement);
        var ifElseStatement = (IfElseStatement)statement;

        Assert.NotNull(ifElseStatement.If);
        Assert.Equal(TokenType.If, ifElseStatement.If.IfKeyword.Type);
        Assert.Equal("if", ifElseStatement.If.IfKeyword.Value);
        Assert.Equal(TokenType.LeftParenthesis, ifElseStatement.If.LeftParenthesis.Type);
        Assert.Equal("(", ifElseStatement.If.LeftParenthesis.Value);
        Assert.IsType<BinaryExpression>(ifElseStatement.If.Condition);
        Assert.Equal(TokenType.RightParenthesis, ifElseStatement.If.RightParenthesis.Type);
        Assert.Equal(")", ifElseStatement.If.RightParenthesis.Value);
        Assert.NotNull(ifElseStatement.If.Block);

        Assert.NotNull(ifElseStatement.ElseIfs);
        Assert.Single(ifElseStatement.ElseIfs);
        Assert.Equal(TokenType.Else, ifElseStatement.ElseIfs[0].ElseKeyword.Type);
        Assert.Equal("else", ifElseStatement.ElseIfs[0].ElseKeyword.Value);
        Assert.NotNull(ifElseStatement.ElseIfs);
        Assert.Single(ifElseStatement.ElseIfs);
        Assert.NotNull(ifElseStatement.ElseIfs[0].IfBlock);
        Assert.Equal(TokenType.If, ifElseStatement.ElseIfs[0].IfBlock!.IfKeyword.Type);
        Assert.Equal("if", ifElseStatement.ElseIfs[0].IfBlock!.IfKeyword.Value);
        Assert.Equal(TokenType.LeftParenthesis, ifElseStatement.ElseIfs[0].IfBlock!.LeftParenthesis.Type);
        Assert.Equal("(", ifElseStatement.ElseIfs[0].IfBlock!.LeftParenthesis.Value);
        Assert.IsType<BinaryExpression>(ifElseStatement.ElseIfs[0].IfBlock!.Condition);
        Assert.Equal(TokenType.RightParenthesis, ifElseStatement.ElseIfs[0].IfBlock!.RightParenthesis.Type);
        Assert.Equal(")", ifElseStatement.ElseIfs[0].IfBlock!.RightParenthesis.Value);
        Assert.NotNull(ifElseStatement.ElseIfs[0].IfBlock!.Block);

        Assert.NotNull(ifElseStatement.Else);
        Assert.Equal(TokenType.Else, ifElseStatement.Else.ElseKeyword.Type);
        Assert.Equal("else", ifElseStatement.Else.ElseKeyword.Value);
        Assert.NotNull(ifElseStatement.Else.Block);

        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.False(validator.HasErrors);
    }

    [Fact]
    public void ParseStatement_IfConditionElseIfConditionElseIfConditionElse_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source = """
                              if (a > 10) {
                              } else if (a > 0) {
                              } else if (a < 0) {
                              } else {
                              }
                              """;
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var validator = new SyntaxNodeValidationVisitor();

        // Act
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(29, index);
        Assert.Equal(TokenType.Eof, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<IfElseStatement>(statement);
        var ifElseStatement = (IfElseStatement)statement;

        Assert.NotNull(ifElseStatement.If);
        Assert.Equal(TokenType.If, ifElseStatement.If.IfKeyword.Type);
        Assert.Equal("if", ifElseStatement.If.IfKeyword.Value);
        Assert.Equal(TokenType.LeftParenthesis, ifElseStatement.If.LeftParenthesis.Type);
        Assert.Equal("(", ifElseStatement.If.LeftParenthesis.Value);
        Assert.IsType<BinaryExpression>(ifElseStatement.If.Condition);
        Assert.Equal(TokenType.RightParenthesis, ifElseStatement.If.RightParenthesis.Type);
        Assert.Equal(")", ifElseStatement.If.RightParenthesis.Value);
        Assert.NotNull(ifElseStatement.If.Block);

        Assert.NotNull(ifElseStatement.ElseIfs);
        Assert.Equal(2, ifElseStatement.ElseIfs.Count);
        Assert.Equal(TokenType.Else, ifElseStatement.ElseIfs[0].ElseKeyword.Type);
        Assert.Equal("else", ifElseStatement.ElseIfs[0].ElseKeyword.Value);
        Assert.NotNull(ifElseStatement.ElseIfs);
        Assert.Equal(2, ifElseStatement.ElseIfs.Count);
        Assert.NotNull(ifElseStatement.ElseIfs[0].IfBlock);
        Assert.Equal(TokenType.If, ifElseStatement.ElseIfs[0].IfBlock!.IfKeyword.Type);
        Assert.Equal("if", ifElseStatement.ElseIfs[0].IfBlock!.IfKeyword.Value);
        Assert.Equal(TokenType.LeftParenthesis, ifElseStatement.ElseIfs[0].IfBlock!.LeftParenthesis.Type);
        Assert.Equal("(", ifElseStatement.ElseIfs[0].IfBlock!.LeftParenthesis.Value);
        Assert.IsType<BinaryExpression>(ifElseStatement.ElseIfs[0].IfBlock!.Condition);
        Assert.Equal(TokenType.RightParenthesis, ifElseStatement.ElseIfs[0].IfBlock!.RightParenthesis.Type);
        Assert.Equal(")", ifElseStatement.ElseIfs[0].IfBlock!.RightParenthesis.Value);
        Assert.NotNull(ifElseStatement.ElseIfs[0].IfBlock!.Block);
        Assert.Equal(TokenType.Else, ifElseStatement.ElseIfs[1].ElseKeyword.Type);
        Assert.Equal("else", ifElseStatement.ElseIfs[1].ElseKeyword.Value);
        Assert.NotNull(ifElseStatement.ElseIfs[1].IfBlock);
        Assert.Equal(TokenType.If, ifElseStatement.ElseIfs[1].IfBlock!.IfKeyword.Type);
        Assert.Equal("if", ifElseStatement.ElseIfs[1].IfBlock!.IfKeyword.Value);
        Assert.Equal(TokenType.LeftParenthesis, ifElseStatement.ElseIfs[1].IfBlock!.LeftParenthesis.Type);
        Assert.Equal("(", ifElseStatement.ElseIfs[1].IfBlock!.LeftParenthesis.Value);
        Assert.IsType<BinaryExpression>(ifElseStatement.ElseIfs[1].IfBlock!.Condition);
        Assert.Equal(TokenType.RightParenthesis, ifElseStatement.ElseIfs[1].IfBlock!.RightParenthesis.Type);
        Assert.Equal(")", ifElseStatement.ElseIfs[1].IfBlock!.RightParenthesis.Value);
        Assert.NotNull(ifElseStatement.ElseIfs[1].IfBlock!.Block);

        Assert.NotNull(ifElseStatement.Else);
        Assert.Equal(TokenType.Else, ifElseStatement.Else.ElseKeyword.Type);
        Assert.Equal("else", ifElseStatement.Else.ElseKeyword.Value);
        Assert.NotNull(ifElseStatement.Else.Block);

        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.False(validator.HasErrors);
    }

    [Fact]
    public void ParseStatement_IfConditionNoLeftParenthesis_LeftParenthesisExpected()
    {
        // Arrange
        const string source = """
                              if a > 0) {
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
        Assert.IsType<IfElseStatement>(statement);
        var ifElseStatement = (IfElseStatement)statement;

        Assert.NotNull(ifElseStatement.If);
        Assert.Equal(TokenType.If, ifElseStatement.If.IfKeyword.Type);
        Assert.Equal("if", ifElseStatement.If.IfKeyword.Value);
        Assert.Equal(TokenType.None, ifElseStatement.If.LeftParenthesis.Type);
        Assert.Equal("", ifElseStatement.If.LeftParenthesis.Value);
        Assert.IsType<BinaryExpression>(ifElseStatement.If.Condition);
        Assert.Equal(TokenType.RightParenthesis, ifElseStatement.If.RightParenthesis.Type);
        Assert.Equal(")", ifElseStatement.If.RightParenthesis.Value);
        Assert.NotNull(ifElseStatement.If.Block);

        Assert.Null(ifElseStatement.ElseIfs);
        Assert.Null(ifElseStatement.Else);

        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(
            validator.Diagnostics,
            d => d.Message.EndsWith(IfElseStatement.LeftParenthesisExpected));
    }

    [Fact]
    public void ParseStatement_IfConditionNoCondition_ExpressionExpected()
    {
        // Arrange
        const string source = """
                              if () {
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
        Assert.IsType<IfElseStatement>(statement);
        var ifElseStatement = (IfElseStatement)statement;

        Assert.NotNull(ifElseStatement.If);
        Assert.Equal(TokenType.If, ifElseStatement.If.IfKeyword.Type);
        Assert.Equal("if", ifElseStatement.If.IfKeyword.Value);
        Assert.Equal(TokenType.LeftParenthesis, ifElseStatement.If.LeftParenthesis.Type);
        Assert.Equal("(", ifElseStatement.If.LeftParenthesis.Value);
        Assert.Null(ifElseStatement.If.Condition);
        Assert.Equal(TokenType.RightParenthesis, ifElseStatement.If.RightParenthesis.Type);
        Assert.Equal(")", ifElseStatement.If.RightParenthesis.Value);
        Assert.NotNull(ifElseStatement.If.Block);

        Assert.Null(ifElseStatement.ElseIfs);
        Assert.Null(ifElseStatement.Else);

        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(
            validator.Diagnostics,
            d => d.Message.EndsWith(IfElseStatement.ExpressionExpected));
    }

    [Fact]
    public void ParseStatement_IfConditionNoRightParenthesis_RightParenthesisExpected()
    {
        // Arrange
        const string source = """
                              if (a > 0 {
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
        Assert.IsType<IfElseStatement>(statement);
        var ifElseStatement = (IfElseStatement)statement;

        Assert.NotNull(ifElseStatement.If);
        Assert.Equal(TokenType.If, ifElseStatement.If.IfKeyword.Type);
        Assert.Equal("if", ifElseStatement.If.IfKeyword.Value);
        Assert.Equal(TokenType.LeftParenthesis, ifElseStatement.If.LeftParenthesis.Type);
        Assert.Equal("(", ifElseStatement.If.LeftParenthesis.Value);
        Assert.IsType<BinaryExpression>(ifElseStatement.If.Condition);
        Assert.Equal(TokenType.None, ifElseStatement.If.RightParenthesis.Type);
        Assert.Equal("", ifElseStatement.If.RightParenthesis.Value);
        Assert.NotNull(ifElseStatement.If.Block);

        Assert.Null(ifElseStatement.ElseIfs);
        Assert.Null(ifElseStatement.Else);

        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(
            validator.Diagnostics,
            d => d.Message.EndsWith(IfElseStatement.RightParenthesisExpected));
    }

    [Fact]
    public void ParseStatement_IfConditionNoLeftBrace_CompoundStatementExpected()
    {
        // Arrange
        const string source = """
                              if (a > 0)
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
        Assert.IsType<IfElseStatement>(statement);
        var ifElseStatement = (IfElseStatement)statement;

        Assert.NotNull(ifElseStatement.If);
        Assert.Equal(TokenType.If, ifElseStatement.If.IfKeyword.Type);
        Assert.Equal("if", ifElseStatement.If.IfKeyword.Value);
        Assert.Equal(TokenType.LeftParenthesis, ifElseStatement.If.LeftParenthesis.Type);
        Assert.Equal("(", ifElseStatement.If.LeftParenthesis.Value);
        Assert.IsType<BinaryExpression>(ifElseStatement.If.Condition);
        Assert.Equal(TokenType.RightParenthesis, ifElseStatement.If.RightParenthesis.Type);
        Assert.Equal(")", ifElseStatement.If.RightParenthesis.Value);
        Assert.Null(ifElseStatement.If.Block);

        Assert.Null(ifElseStatement.ElseIfs);
        Assert.Null(ifElseStatement.Else);

        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(
            validator.Diagnostics,
            d => d.Message.EndsWith(IfElseStatement.CompoundStatementExpected));
    }

    [Fact]
    public void ParseStatement_IfConditionElseNoLeftBrace_CompoundStatementExpected()
    {
        // Arrange
        const string source = """
                              if (a > 0) {
                              } else
                              }
                              """;
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var validator = new SyntaxNodeValidationVisitor();

        // Act
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(9, index);
        Assert.Equal(TokenType.RightBrace, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<IfElseStatement>(statement);
        var ifElseStatement = (IfElseStatement)statement;

        Assert.NotNull(ifElseStatement.If);
        Assert.Equal(TokenType.If, ifElseStatement.If.IfKeyword.Type);
        Assert.Equal("if", ifElseStatement.If.IfKeyword.Value);
        Assert.Equal(TokenType.LeftParenthesis, ifElseStatement.If.LeftParenthesis.Type);
        Assert.Equal("(", ifElseStatement.If.LeftParenthesis.Value);
        Assert.IsType<BinaryExpression>(ifElseStatement.If.Condition);
        Assert.Equal(TokenType.RightParenthesis, ifElseStatement.If.RightParenthesis.Type);
        Assert.Equal(")", ifElseStatement.If.RightParenthesis.Value);
        Assert.NotNull(ifElseStatement.If.Block);

        Assert.Null(ifElseStatement.ElseIfs);

        Assert.NotNull(ifElseStatement.Else);
        Assert.Equal(TokenType.Else, ifElseStatement.Else.ElseKeyword.Type);
        Assert.Equal("else", ifElseStatement.Else.ElseKeyword.Value);
        Assert.Null(ifElseStatement.Else.Block);

        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(
            validator.Diagnostics,
            d => d.Message.EndsWith(IfElseStatement.CompoundStatementExpected));
    }

    [Fact]
    public void ParseStatement_ElseNoIfConditionBefore_ShouldThrowLanguageSyntaxException()
    {
        // Arrange
        const string source = """
                              else {
                              }
                              """;
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var validator = new SyntaxNodeValidationVisitor();

        // Act
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(3, index);
        Assert.Equal(TokenType.Eof, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<IfElseStatement>(statement);
        var ifElseStatement = (IfElseStatement)statement;

        Assert.Null(ifElseStatement.If);
        Assert.Null(ifElseStatement.ElseIfs);

        Assert.NotNull(ifElseStatement.Else);
        Assert.Equal(TokenType.Else, ifElseStatement.Else.ElseKeyword.Type);
        Assert.Equal("else", ifElseStatement.Else.ElseKeyword.Value);
        Assert.NotNull(ifElseStatement.Else.Block);

        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(
            validator.Diagnostics,
            d => d.Message.EndsWith(IfElseStatement.ElseBeforeIf));
    }
}