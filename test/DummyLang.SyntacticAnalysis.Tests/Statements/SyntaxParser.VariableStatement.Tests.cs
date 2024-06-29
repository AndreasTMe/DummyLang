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
    public void ParseStatement_VarDeclarationSwapColonAndAssignment_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source = "var foo =: 1 + 2;";

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

        Assert.IsType<InvalidExpression>(variableDeclaration.Identifier);
        var invalid = (InvalidExpression)variableDeclaration.Identifier;
        Assert.NotEmpty(invalid.Tokens);
        Assert.Equal(2, invalid.Tokens.Length);
        Assert.Equal(TokenType.Colon, invalid.Tokens[0].Type);
        Assert.Equal(TokenType.Assign, invalid.Tokens[1].Type);

        Assert.Equal(TokenType.None, variableDeclaration.TypeAssignment.Type);
        Assert.Equal("", variableDeclaration.TypeAssignment.Value);

        Assert.Null(variableDeclaration.Type);

        Assert.Equal(TokenType.None, variableDeclaration.ValueAssignment.Type);
        Assert.Equal("", variableDeclaration.ValueAssignment.Value);

        Assert.Null(variableDeclaration.Value);

        Assert.Equal(TokenType.Semicolon, variableDeclaration.Terminator.Type);
        Assert.Equal(";", variableDeclaration.Terminator.Value);
    }

    [Fact]
    public void ParseStatement_VarDeclarationNoColon_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source = "var foo = 1 + 2;";

        // Act
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(7, index);
        Assert.Equal(TokenType.Eof, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<VariableDeclarationStatement>(statement);
        var variableDeclaration = (VariableDeclarationStatement)statement;

        Assert.Equal(TokenType.Var, variableDeclaration.Declaration.Type);
        Assert.Equal("var", variableDeclaration.Declaration.Value);

        Assert.IsType<IdentifierExpression>(variableDeclaration.Identifier);

        Assert.Equal(TokenType.None, variableDeclaration.TypeAssignment.Type);
        Assert.Equal("", variableDeclaration.TypeAssignment.Value);

        Assert.IsType<InvalidExpression>(variableDeclaration.Type);
        var invalid = (InvalidExpression)variableDeclaration.Type;
        Assert.Single(invalid.Tokens);
        Assert.Equal(TokenType.Colon, invalid.Tokens[0].Type);

        Assert.Equal(TokenType.Assign, variableDeclaration.ValueAssignment.Type);
        Assert.Equal("=", variableDeclaration.ValueAssignment.Value);

        Assert.IsType<BinaryExpression>(variableDeclaration.Value);

        Assert.Equal(TokenType.Semicolon, variableDeclaration.Terminator.Type);
        Assert.Equal(";", variableDeclaration.Terminator.Value);
    }

    [Fact]
    public void ParseStatement_VarDeclarationNoColonNoAssignment_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source = "var foo i32 1 + 2;";

        // Act
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(7, index);
        Assert.Equal(TokenType.Eof, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<VariableDeclarationStatement>(statement);
        var variableDeclaration = (VariableDeclarationStatement)statement;

        Assert.Equal(TokenType.Var, variableDeclaration.Declaration.Type);
        Assert.Equal("var", variableDeclaration.Declaration.Value);

        Assert.IsType<InvalidExpression>(variableDeclaration.Identifier);
        var invalid = (InvalidExpression)variableDeclaration.Identifier;
        Assert.NotEmpty(invalid.Tokens);
        Assert.Equal(5, invalid.Tokens.Length);
        Assert.Equal(TokenType.Identifier, invalid.Tokens[0].Type);
        Assert.Equal(TokenType.Identifier, invalid.Tokens[1].Type);
        Assert.Equal(TokenType.Integer, invalid.Tokens[2].Type);
        Assert.Equal(TokenType.Plus, invalid.Tokens[3].Type);
        Assert.Equal(TokenType.Integer, invalid.Tokens[4].Type);

        Assert.Equal(TokenType.None, variableDeclaration.TypeAssignment.Type);
        Assert.Equal("", variableDeclaration.TypeAssignment.Value);

        Assert.Null(variableDeclaration.Type);

        Assert.Equal(TokenType.None, variableDeclaration.ValueAssignment.Type);
        Assert.Equal("", variableDeclaration.ValueAssignment.Value);

        Assert.Null(variableDeclaration.Value);

        Assert.Equal(TokenType.Semicolon, variableDeclaration.Terminator.Type);
        Assert.Equal(";", variableDeclaration.Terminator.Value);
    }

    [Fact]
    public void ParseStatement_VarDeclarationNoSemicolon_ShouldUseNextVariableDeclarationStatementAsStop()
    {
        // Arrange
        const string source = """
                              var foo: i32 = 1 + 2
                              var bar := 2;
                              """;

        // Act
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(8, index);
        Assert.Equal(TokenType.Var, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<VariableDeclarationStatement>(statement);
        var variableDeclaration = (VariableDeclarationStatement)statement;

        Assert.Equal(TokenType.Var, variableDeclaration.Declaration.Type);
        Assert.Equal("var", variableDeclaration.Declaration.Value);

        Assert.IsType<IdentifierExpression>(variableDeclaration.Identifier);

        Assert.Equal(TokenType.Colon, variableDeclaration.TypeAssignment.Type);
        Assert.Equal(":", variableDeclaration.TypeAssignment.Value);

        Assert.IsType<TypeIdentifierExpression>(variableDeclaration.Type);

        Assert.Equal(TokenType.Assign, variableDeclaration.ValueAssignment.Type);
        Assert.Equal("=", variableDeclaration.ValueAssignment.Value);

        Assert.IsType<BinaryExpression>(variableDeclaration.Value);

        Assert.Equal(TokenType.None, variableDeclaration.Terminator.Type);
        Assert.Equal("", variableDeclaration.Terminator.Value);
    }

    [Fact]
    public void ParseStatement_VarDeclarationNoSemicolon_ShouldUseNextVariableAssignmentStatementAsStop()
    {
        // Arrange
        const string source = """
                              var foo: i32 = 1 + 2
                              bar = 2;
                              """;

        // Act
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(8, index);
        Assert.Equal(TokenType.Identifier, tokens[index].Type);
        Assert.Equal("bar", tokens[index].Value);

        Assert.NotNull(statement);
        Assert.IsType<VariableDeclarationStatement>(statement);
        var variableDeclaration = (VariableDeclarationStatement)statement;

        Assert.Equal(TokenType.Var, variableDeclaration.Declaration.Type);
        Assert.Equal("var", variableDeclaration.Declaration.Value);

        Assert.IsType<IdentifierExpression>(variableDeclaration.Identifier);

        Assert.Equal(TokenType.Colon, variableDeclaration.TypeAssignment.Type);
        Assert.Equal(":", variableDeclaration.TypeAssignment.Value);

        Assert.IsType<TypeIdentifierExpression>(variableDeclaration.Type);

        Assert.Equal(TokenType.Assign, variableDeclaration.ValueAssignment.Type);
        Assert.Equal("=", variableDeclaration.ValueAssignment.Value);

        Assert.IsType<BinaryExpression>(variableDeclaration.Value);

        Assert.Equal(TokenType.None, variableDeclaration.Terminator.Type);
        Assert.Equal("", variableDeclaration.Terminator.Value);
    }

    [Fact]
    public void ParseStatement_VarDeclarationNoAssignmentNoSemicolon_ShouldUseNextVariableDeclarationStatementAsStop()
    {
        // Arrange
        const string source = """
                              var foo: i32
                              var bar := 2;
                              """;

        // Act
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(4, index);
        Assert.Equal(TokenType.Var, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<VariableDeclarationStatement>(statement);
        var variableDeclaration = (VariableDeclarationStatement)statement;

        Assert.Equal(TokenType.Var, variableDeclaration.Declaration.Type);
        Assert.Equal("var", variableDeclaration.Declaration.Value);

        Assert.IsType<IdentifierExpression>(variableDeclaration.Identifier);

        Assert.Equal(TokenType.Colon, variableDeclaration.TypeAssignment.Type);
        Assert.Equal(":", variableDeclaration.TypeAssignment.Value);

        Assert.IsType<TypeIdentifierExpression>(variableDeclaration.Type);

        Assert.Equal(TokenType.None, variableDeclaration.ValueAssignment.Type);
        Assert.Equal("", variableDeclaration.ValueAssignment.Value);

        Assert.Null(variableDeclaration.Value);

        Assert.Equal(TokenType.None, variableDeclaration.Terminator.Type);
        Assert.Equal("", variableDeclaration.Terminator.Value);
    }

    [Fact]
    public void ParseStatement_VarDeclarationNoAssignmentNoSemicolon_ShouldUseNextVariableAssignmentStatementAsStop()
    {
        // Arrange
        const string source = """
                              var foo: i32
                              bar = 2;
                              """;

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

        // TODO: This should be invalid type
        Assert.IsType<TypeIdentifierExpression>(variableDeclaration.Type);
        var typeIdentifierExpression = (TypeIdentifierExpression)variableDeclaration.Type;
        Assert.NotEmpty(typeIdentifierExpression.Tokens);
        Assert.Equal(2, typeIdentifierExpression.Tokens.Length);
        Assert.Equal(TokenType.Identifier, typeIdentifierExpression.Tokens[0].Type);
        Assert.Equal("i32", typeIdentifierExpression.Tokens[0].Value);
        Assert.Equal(TokenType.Identifier, typeIdentifierExpression.Tokens[1].Type);
        Assert.Equal("bar", typeIdentifierExpression.Tokens[1].Value);

        Assert.Equal(TokenType.Assign, variableDeclaration.ValueAssignment.Type);
        Assert.Equal("=", variableDeclaration.ValueAssignment.Value);

        Assert.IsType<NumberLiteralExpression>(variableDeclaration.Value);

        Assert.Equal(TokenType.Semicolon, variableDeclaration.Terminator.Type);
        Assert.Equal(";", variableDeclaration.Terminator.Value);
    }
}