using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Expressions;
using DummyLang.SyntacticAnalysis.Parsers;
using DummyLang.SyntacticAnalysis.Statements;
using DummyLang.SyntacticAnalysis.Utilities;
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

        // Act
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(8, index);
        Assert.Equal(TokenType.Eof, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<IfElseStatement>(statement);
        var ifElseStatement = (IfElseStatement)statement;

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

        // Act
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(17, index);
        Assert.Equal(TokenType.Eof, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<IfElseStatement>(statement);
        var ifElseStatement = (IfElseStatement)statement;

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

        // Act
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(20, index);
        Assert.Equal(TokenType.Eof, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<IfElseStatement>(statement);
        var ifElseStatement = (IfElseStatement)statement;

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

        // Act
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(29, index);
        Assert.Equal(TokenType.Eof, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<IfElseStatement>(statement);
        var ifElseStatement = (IfElseStatement)statement;

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
    }

    [Fact]
    public void ParseStatement_IfConditionNoLeftParenthesis_ShouldThrowLanguageSyntaxException()
    {
        // Arrange
        const string source = """
                              if a > 0) {
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

    [Fact]
    public void ParseStatement_IfConditionNoCondition_ShouldThrowLanguageSyntaxException()
    {
        // Arrange
        const string source = """
                              if () {
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
    public void ParseStatement_IfConditionNoRightParenthesis_ShouldThrowLanguageSyntaxException()
    {
        // Arrange
        const string source = """
                              if (a > 0 {
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
    public void ParseStatement_IfConditionNoLeftBrace_ShouldThrowLanguageSyntaxException()
    {
        // Arrange
        const string source = """
                              if (a > 0)
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
    public void ParseStatement_IfConditionElseNoLeftBrace_ShouldThrowLanguageSyntaxException()
    {
        // Arrange
        const string source = """
                              if (a > 0) {
                              } else
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
    public void ParseStatement_ElseNoIfConditionBefore_ShouldThrowLanguageSyntaxException()
    {
        // Arrange
        const string source = """
                              else {
                              }
                              """;

        // Act
        var tokens = ParsingUtilities.ReadAllTokens(source);
        var index  = 0;

        // Assert
        var exception = Assert.Throws<LanguageSyntaxException>(() => StatementParser.Parse(ref index, in tokens));
        Assert.Equal(TokenType.Else, exception.Found.Type);
        Assert.Equal("else", exception.Found.Value);
    }
}