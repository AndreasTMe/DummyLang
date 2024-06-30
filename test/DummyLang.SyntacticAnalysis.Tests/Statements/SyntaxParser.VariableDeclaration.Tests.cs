using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Expressions;
using DummyLang.SyntacticAnalysis.Parsers;
using DummyLang.SyntacticAnalysis.Statements;
using DummyLang.SyntacticAnalysis.Utilities;
using Xunit;

namespace DummyLang.SyntacticAnalysis.Tests.Statements;

public class VariableDeclarationSyntaxParserTests
{
    [Fact]
    public void ParseStatement_VarDeclaration_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source = "var foo: i32 = 1 + 2;";

        // Act
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(9, index);
        Assert.Equal(TokenType.Eof, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<VariableDeclarationStatement>(statement);
        var variableDeclaration = (VariableDeclarationStatement)statement;

        Assert.Equal(TokenType.Var, variableDeclaration.Declaration.Type);
        Assert.Equal("var", variableDeclaration.Declaration.Value);

        Assert.IsType<IdentifierExpression>(variableDeclaration.Identifier);

        Assert.Equal(TokenType.Colon, variableDeclaration.TypeAssignment.Type);
        Assert.Equal(":", variableDeclaration.TypeAssignment.Value);

        Assert.IsType<TypeIdentifierExpression>(variableDeclaration.Type);
        var identifier = (TypeIdentifierExpression)variableDeclaration.Type;
        Assert.Single(identifier.Tokens);
        Assert.Equal(TokenType.Identifier, identifier.Tokens[0].Type);
        Assert.Equal("i32", identifier.Tokens[0].Value);

        Assert.Equal(TokenType.Assign, variableDeclaration.ValueAssignment.Type);
        Assert.Equal("=", variableDeclaration.ValueAssignment.Value);

        Assert.IsType<BinaryExpression>(variableDeclaration.Value);

        Assert.Equal(TokenType.Semicolon, variableDeclaration.Terminator.Type);
        Assert.Equal(";", variableDeclaration.Terminator.Value);
    }

    [Fact]
    public void ParseStatement_ConstDeclaration_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source = "const foo: i32 = 1 + 2;";

        // Act
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(9, index);
        Assert.Equal(TokenType.Eof, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<VariableDeclarationStatement>(statement);
        var variableDeclaration = (VariableDeclarationStatement)statement;

        Assert.Equal(TokenType.Const, variableDeclaration.Declaration.Type);
        Assert.Equal("const", variableDeclaration.Declaration.Value);

        Assert.IsType<IdentifierExpression>(variableDeclaration.Identifier);

        Assert.Equal(TokenType.Colon, variableDeclaration.TypeAssignment.Type);
        Assert.Equal(":", variableDeclaration.TypeAssignment.Value);

        Assert.IsType<TypeIdentifierExpression>(variableDeclaration.Type);
        var identifier = (TypeIdentifierExpression)variableDeclaration.Type;
        Assert.Single(identifier.Tokens);
        Assert.Equal(TokenType.Identifier, identifier.Tokens[0].Type);
        Assert.Equal("i32", identifier.Tokens[0].Value);

        Assert.Equal(TokenType.Assign, variableDeclaration.ValueAssignment.Type);
        Assert.Equal("=", variableDeclaration.ValueAssignment.Value);

        Assert.IsType<BinaryExpression>(variableDeclaration.Value);

        Assert.Equal(TokenType.Semicolon, variableDeclaration.Terminator.Type);
        Assert.Equal(";", variableDeclaration.Terminator.Value);
    }

    [Fact]
    public void ParseStatement_DeclarationInferredType_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source = "var foo := 1 + 2;";

        // Act
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(8, index);
        Assert.Equal(TokenType.Eof, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<VariableDeclarationStatement>(statement);
        var variableDeclaration = (VariableDeclarationStatement)statement;

        Assert.Equal(TokenType.Var, variableDeclaration.Declaration.Type);
        Assert.Equal("var", variableDeclaration.Declaration.Value);

        Assert.IsType<IdentifierExpression>(variableDeclaration.Identifier);

        Assert.Equal(TokenType.Colon, variableDeclaration.TypeAssignment.Type);
        Assert.Equal(":", variableDeclaration.TypeAssignment.Value);

        Assert.Null(variableDeclaration.Type);

        Assert.Equal(TokenType.Assign, variableDeclaration.ValueAssignment.Type);
        Assert.Equal("=", variableDeclaration.ValueAssignment.Value);

        Assert.IsType<BinaryExpression>(variableDeclaration.Value);

        Assert.Equal(TokenType.Semicolon, variableDeclaration.Terminator.Type);
        Assert.Equal(";", variableDeclaration.Terminator.Value);
    }

    [Fact]
    public void ParseStatement_DeclarationNoAssignment_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source = "var foo: i32;";

        // Act
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(5, index);
        Assert.Equal(TokenType.Eof, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<VariableDeclarationStatement>(statement);
        var variableDeclaration = (VariableDeclarationStatement)statement;

        Assert.Equal(TokenType.Var, variableDeclaration.Declaration.Type);
        Assert.Equal("var", variableDeclaration.Declaration.Value);

        Assert.IsType<IdentifierExpression>(variableDeclaration.Identifier);

        Assert.Equal(TokenType.Colon, variableDeclaration.TypeAssignment.Type);
        Assert.Equal(":", variableDeclaration.TypeAssignment.Value);

        Assert.IsType<TypeIdentifierExpression>(variableDeclaration.Type);
        var identifier = (TypeIdentifierExpression)variableDeclaration.Type;
        Assert.Single(identifier.Tokens);
        Assert.Equal(TokenType.Identifier, identifier.Tokens[0].Type);
        Assert.Equal("i32", identifier.Tokens[0].Value);

        Assert.Equal(TokenType.None, variableDeclaration.ValueAssignment.Type);
        Assert.Equal("", variableDeclaration.ValueAssignment.Value);

        Assert.Null(variableDeclaration.Value);

        Assert.Equal(TokenType.Semicolon, variableDeclaration.Terminator.Type);
        Assert.Equal(";", variableDeclaration.Terminator.Value);
    }
    
    [Fact]
    public void ParseStatement_VarDeclarationNoIdentifier_ShouldThrowLanguageSyntaxException()
    {
        // Arrange
        const string source = "var := 1 + 2;";

        // Act
        var tokens = ParsingUtilities.ReadAllTokens(source);
        var index  = 0;

        // Assert
        var exception = Assert.Throws<LanguageSyntaxException>(() => StatementParser.Parse(ref index, in tokens));
        Assert.Equal(TokenType.Identifier, exception.Expected);
        Assert.Equal(TokenType.Colon, exception.Found.Type);
    }

    [Fact]
    public void ParseStatement_VarDeclarationSwapColonAndAssignment_ShouldThrowLanguageSyntaxException()
    {
        // Arrange
        const string source = "var foo =: 1 + 2;";

        // Act
        var tokens = ParsingUtilities.ReadAllTokens(source);
        var index  = 0;

        // Assert
        var exception = Assert.Throws<LanguageSyntaxException>(() => StatementParser.Parse(ref index, in tokens));
        Assert.Equal(TokenType.Colon, exception.Expected);
        Assert.Equal(TokenType.Assign, exception.Found.Type);
    }

    [Fact]
    public void ParseStatement_VarDeclarationNoColon_ShouldThrowLanguageSyntaxException()
    {
        // Arrange
        const string source = "var foo = 1 + 2;";

        // Act
        var tokens = ParsingUtilities.ReadAllTokens(source);
        var index  = 0;

        // Assert
        var exception = Assert.Throws<LanguageSyntaxException>(() => StatementParser.Parse(ref index, in tokens));
        Assert.Equal(TokenType.Colon, exception.Expected);
        Assert.Equal(TokenType.Assign, exception.Found.Type);
    }

    [Fact]
    public void ParseStatement_VarDeclarationNoColonNoAssignment_ShouldThrowLanguageSyntaxException()
    {
        // Arrange
        const string source = "var foo i32 1 + 2;";

        // Act
        var tokens = ParsingUtilities.ReadAllTokens(source);
        var index  = 0;

        // Assert
        var exception = Assert.Throws<LanguageSyntaxException>(() => StatementParser.Parse(ref index, in tokens));
        Assert.Equal(TokenType.Colon, exception.Expected);
        Assert.Equal(TokenType.Identifier, exception.Found.Type);
        Assert.Equal("i32", exception.Found.Value);
    }

    [Fact]
    public void ParseStatement_VarDeclarationNoSemicolon1_ShouldThrowLanguageSyntaxException()
    {
        // Arrange
        const string source = """
                              var foo: i32 = 1 + 2
                              var bar := 2;
                              """;

        // Act
        var tokens = ParsingUtilities.ReadAllTokens(source);
        var index  = 0;

        // Assert
        var exception = Assert.Throws<LanguageSyntaxException>(() => StatementParser.Parse(ref index, in tokens));
        Assert.Equal(TokenType.Semicolon, exception.Expected);
        Assert.Equal(TokenType.Var, exception.Found.Type);
    }

    [Fact]
    public void ParseStatement_VarDeclarationNoSemicolon2_ShouldThrowLanguageSyntaxException()
    {
        // Arrange
        const string source = """
                              var foo: i32 = 1 + 2
                              bar = 2;
                              """;

        // Act
        var tokens = ParsingUtilities.ReadAllTokens(source);
        var index  = 0;

        // Assert
        var exception = Assert.Throws<LanguageSyntaxException>(() => StatementParser.Parse(ref index, in tokens));
        Assert.Equal(TokenType.Semicolon, exception.Expected);
        Assert.Equal(TokenType.Identifier, exception.Found.Type);
        Assert.Equal("bar", exception.Found.Value);
    }

    [Fact]
    public void ParseStatement_VarDeclarationNoAssignmentNoSemicolon1_ShouldThrowLanguageSyntaxException()
    {
        // Arrange
        const string source = """
                              var foo: i32
                              var bar := 2;
                              """;

        // Act
        var tokens = ParsingUtilities.ReadAllTokens(source);
        var index  = 0;

        // Assert
        var exception = Assert.Throws<LanguageSyntaxException>(() => StatementParser.Parse(ref index, in tokens));
        Assert.Equal(TokenType.Semicolon, exception.Expected);
        Assert.Equal(TokenType.Var, exception.Found.Type);
    }

    [Fact]
    public void ParseStatement_VarDeclarationNoAssignmentNoSemicolon2_ShouldThrowLanguageSyntaxException()
    {
        // Arrange
        const string source = """
                              var foo: i32
                              bar = 2;
                              """;

        // Act
        var tokens = ParsingUtilities.ReadAllTokens(source);
        var index  = 0;

        // Assert
        var exception = Assert.Throws<LanguageSyntaxException>(() => StatementParser.Parse(ref index, in tokens));
        Assert.Equal(TokenType.Semicolon, exception.Expected);
        Assert.Equal(TokenType.Identifier, exception.Found.Type);
        Assert.Equal("bar", exception.Found.Value);
    }
}