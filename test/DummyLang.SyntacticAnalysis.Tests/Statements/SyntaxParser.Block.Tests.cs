using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Parsers;
using DummyLang.SyntacticAnalysis.Statements;
using DummyLang.SyntacticAnalysis.Utilities;
using Xunit;

namespace DummyLang.SyntacticAnalysis.Tests.Statements;

public class CompoundSyntaxParserTests
{
    [Fact]
    public void ParseStatement_EmptyBlock_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source = "{ }";

        // Act
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(2, index);
        Assert.Equal(TokenType.Eof, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<CompoundStatement>(statement);
        var compoundStatement = (CompoundStatement)statement;

        Assert.Equal(TokenType.LeftBrace, compoundStatement.LeftBrace.Type);
        Assert.Equal("{", compoundStatement.LeftBrace.Value);

        Assert.Null(compoundStatement.Statements);

        Assert.Equal(TokenType.RightBrace, compoundStatement.RightBrace.Type);
        Assert.Equal("}", compoundStatement.RightBrace.Value);
    }

    [Fact]
    public void ParseStatement_BlockWithThreeStatements_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source = """
                              {
                                  var a := 1 + 2;
                                  a++;
                                  
                                  return a;
                              }
                              """;

        // Act
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(16, index);
        Assert.Equal(TokenType.Eof, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<CompoundStatement>(statement);
        var compoundStatement = (CompoundStatement)statement;

        Assert.Equal(TokenType.LeftBrace, compoundStatement.LeftBrace.Type);
        Assert.Equal("{", compoundStatement.LeftBrace.Value);

        Assert.NotNull(compoundStatement.Statements);
        Assert.Equal(3, compoundStatement.Statements.Count);
        Assert.IsType<VariableDeclarationStatement>(compoundStatement.Statements[0]);
        Assert.IsType<ExpressionStatement>(compoundStatement.Statements[1]);
        Assert.IsType<ReturnStatement>(compoundStatement.Statements[2]);

        Assert.Equal(TokenType.RightBrace, compoundStatement.RightBrace.Type);
        Assert.Equal("}", compoundStatement.RightBrace.Value);
    }

    [Fact]
    public void ParseStatement_BlockWithInnerBlock_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source = """
                              {
                                  var a := 1 + 2;
                                  a++;
                                  
                                  {
                                      Print("Hello");
                                  }
                                  
                                  return a;
                              }
                              """;

        // Act
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(23, index);
        Assert.Equal(TokenType.Eof, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<CompoundStatement>(statement);
        var compoundStatement = (CompoundStatement)statement;

        Assert.Equal(TokenType.LeftBrace, compoundStatement.LeftBrace.Type);
        Assert.Equal("{", compoundStatement.LeftBrace.Value);

        Assert.NotNull(compoundStatement.Statements);
        Assert.Equal(4, compoundStatement.Statements.Count);
        Assert.IsType<VariableDeclarationStatement>(compoundStatement.Statements[0]);
        Assert.IsType<ExpressionStatement>(compoundStatement.Statements[1]);

        Assert.IsType<CompoundStatement>(compoundStatement.Statements[2]);
        var innerCompoundStatement = (CompoundStatement)compoundStatement.Statements[2];
        Assert.Equal(TokenType.LeftBrace, innerCompoundStatement.LeftBrace.Type);
        Assert.Equal("{", innerCompoundStatement.LeftBrace.Value);
        Assert.NotNull(innerCompoundStatement.Statements);
        Assert.Single(innerCompoundStatement.Statements);
        Assert.IsType<ExpressionStatement>(innerCompoundStatement.Statements[0]);
        Assert.Equal(TokenType.RightBrace, innerCompoundStatement.RightBrace.Type);
        Assert.Equal("}", innerCompoundStatement.RightBrace.Value);

        Assert.IsType<ReturnStatement>(compoundStatement.Statements[3]);

        Assert.Equal(TokenType.RightBrace, compoundStatement.RightBrace.Type);
        Assert.Equal("}", compoundStatement.RightBrace.Value);
    }

    [Fact]
    public void ParseStatement_EmptyBlockNoClosingBrace_ShouldThrowLanguageSyntaxException()
    {
        // Arrange
        const string source = """
                              {
                                  var a := 1 + 2;
                              """;

        // Act
        var tokens = ParsingUtilities.ReadAllTokens(source);
        var index  = 0;

        // Assert
        Assert.Throws<LanguageSyntaxException>(() => StatementParser.Parse(ref index, in tokens));
    }
}