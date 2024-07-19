using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Parsers;
using DummyLang.SyntacticAnalysis.Statements;
using DummyLang.SyntacticAnalysis.Utilities;
using DummyLang.SyntacticAnalysis.Visitors;
using Xunit;

namespace DummyLang.SyntacticAnalysis.Tests.Statements;

public class CompoundSyntaxParserTests
{
    [Fact]
    public void ParseStatement_EmptyBlock_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source    = "{ }";
        var          tokens    = ParsingUtilities.ReadAllTokens(source);
        var          index     = 0;
        var          validator = new SyntaxNodeValidationVisitor();

        // Act
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

        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.False(validator.HasErrors);
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
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var validator = new SyntaxNodeValidationVisitor();

        // Act
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

        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.False(validator.HasErrors);
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
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var validator = new SyntaxNodeValidationVisitor();

        // Act
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

        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.False(validator.HasErrors);
    }

    [Fact]
    public void ParseStatement_EmptyBlockNoClosingBrace_RightBraceExpected()
    {
        // Arrange
        const string source = """
                              {
                                  var a := 1 + 2;
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
        Assert.IsType<CompoundStatement>(statement);
        var compoundStatement = (CompoundStatement)statement;

        Assert.Equal(TokenType.LeftBrace, compoundStatement.LeftBrace.Type);
        Assert.Equal("{", compoundStatement.LeftBrace.Value);

        Assert.NotNull(compoundStatement.Statements);
        Assert.Equal(1, compoundStatement.Statements.Count);
        Assert.IsType<VariableDeclarationStatement>(compoundStatement.Statements[0]);

        Assert.Equal(TokenType.None, compoundStatement.RightBrace.Type);
        Assert.Equal("", compoundStatement.RightBrace.Value);

        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(
            validator.Diagnostics,
            d => d.Message.EndsWith(CompoundStatement.RightBraceExpected));
    }
}