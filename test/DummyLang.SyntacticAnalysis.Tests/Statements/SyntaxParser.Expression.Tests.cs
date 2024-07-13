using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Expressions;
using DummyLang.SyntacticAnalysis.Parsers;
using DummyLang.SyntacticAnalysis.Statements;
using DummyLang.SyntacticAnalysis.Utilities;
using Xunit;

namespace DummyLang.SyntacticAnalysis.Tests.Statements;

public class ExpressionSyntaxParserTests
{
    [Fact]
    public void ParseStatement_Expression_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source = "foo = 1 + 2;";

        // Act
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
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
    }
    
    [Fact]
    public void ParseStatement_ExpressionPlusEqual_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source = "foo += 1 + 2;";

        // Act
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
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
    }
    
    [Fact]
    public void ParseStatement_ExpressionInvalidToken_ShouldThrowLanguageSyntaxException()
    {
        // Arrange
        const string source = "foo := 1 + 2;";

        // Act
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;

        // Assert
        Assert.Throws<LanguageSyntaxException>(() => StatementParser.Parse(ref index, in tokens));
    }
    
    [Fact]
    public void ParseStatement_ExpressionNoSemicolon1_ShouldThrowLanguageSyntaxException()
    {
        // Arrange
        const string source = "foo = 1 + 2";

        // Act
        var tokens = ParsingUtilities.ReadAllTokens(source);
        var index  = 0;

        // Assert
        Assert.Throws<LanguageSyntaxException>(() => StatementParser.Parse(ref index, in tokens));
    }
    
    [Fact]
    public void ParseStatement_ExpressionNoSemicolon2_ShouldThrowLanguageSyntaxException()
    {
        // Arrange
        const string source = """
                              foo = 1 + 2
                              var test := 1;
                              """;

        // Act
        var tokens = ParsingUtilities.ReadAllTokens(source);
        var index  = 0;

        // Assert
        Assert.Throws<LanguageSyntaxException>(() => StatementParser.Parse(ref index, in tokens));
    }
}