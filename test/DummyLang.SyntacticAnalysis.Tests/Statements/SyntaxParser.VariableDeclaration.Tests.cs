using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Expressions;
using DummyLang.SyntacticAnalysis.Parsers;
using DummyLang.SyntacticAnalysis.Statements;
using DummyLang.SyntacticAnalysis.Utilities;
using DummyLang.SyntacticAnalysis.Visitors;
using Xunit;

namespace DummyLang.SyntacticAnalysis.Tests.Statements;

public class VariableDeclarationSyntaxParserTests
{
    [Fact]
    public void ParseStatement_VarDeclaration_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source    = "var foo: i32 = 1 + 2;";
        var          tokens    = ParsingUtilities.ReadAllTokens(source);
        var          index     = 0;
        var          validator = new SyntaxNodeValidationVisitor();

        // Act
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(9, index);
        Assert.Equal(TokenType.Eof, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<VariableDeclarationStatement>(statement);
        var variableDeclaration = (VariableDeclarationStatement)statement;

        Assert.Equal(TokenType.Var, variableDeclaration.DeclarationKeyword.Type);
        Assert.Equal("var", variableDeclaration.DeclarationKeyword.Value);

        Assert.Equal(TokenType.Identifier, variableDeclaration.Identifier.Type);
        Assert.Equal("foo", variableDeclaration.Identifier.Value);

        Assert.Equal(TokenType.Colon, variableDeclaration.TypeAssignment.Type);
        Assert.Equal(":", variableDeclaration.TypeAssignment.Value);

        Assert.IsType<TypeIdentifierExpression>(variableDeclaration.Type);
        var identifier = (TypeIdentifierExpression)variableDeclaration.Type;
        Assert.Equal(TokenType.Identifier, identifier.Token.Type);
        Assert.Equal("i32", identifier.Token.Value);

        Assert.Equal(TokenType.Assign, variableDeclaration.ValueAssignment.Type);
        Assert.Equal("=", variableDeclaration.ValueAssignment.Value);

        Assert.IsType<BinaryExpression>(variableDeclaration.Value);

        Assert.Equal(TokenType.Semicolon, variableDeclaration.Terminator.Type);
        Assert.Equal(";", variableDeclaration.Terminator.Value);

        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.False(validator.HasErrors);
    }

    [Fact]
    public void ParseStatement_ConstDeclaration_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source    = "const foo: i32 = 1 + 2;";
        var          tokens    = ParsingUtilities.ReadAllTokens(source);
        var          index     = 0;
        var          validator = new SyntaxNodeValidationVisitor();

        // Act
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(9, index);
        Assert.Equal(TokenType.Eof, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<VariableDeclarationStatement>(statement);
        var variableDeclaration = (VariableDeclarationStatement)statement;

        Assert.Equal(TokenType.Const, variableDeclaration.DeclarationKeyword.Type);
        Assert.Equal("const", variableDeclaration.DeclarationKeyword.Value);

        Assert.Equal(TokenType.Identifier, variableDeclaration.Identifier.Type);
        Assert.Equal("foo", variableDeclaration.Identifier.Value);

        Assert.Equal(TokenType.Colon, variableDeclaration.TypeAssignment.Type);
        Assert.Equal(":", variableDeclaration.TypeAssignment.Value);

        Assert.IsType<TypeIdentifierExpression>(variableDeclaration.Type);
        var identifier = (TypeIdentifierExpression)variableDeclaration.Type;
        Assert.Equal(TokenType.Identifier, identifier.Token.Type);
        Assert.Equal("i32", identifier.Token.Value);

        Assert.Equal(TokenType.Assign, variableDeclaration.ValueAssignment.Type);
        Assert.Equal("=", variableDeclaration.ValueAssignment.Value);

        Assert.IsType<BinaryExpression>(variableDeclaration.Value);

        Assert.Equal(TokenType.Semicolon, variableDeclaration.Terminator.Type);
        Assert.Equal(";", variableDeclaration.Terminator.Value);

        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.False(validator.HasErrors);
    }

    [Fact]
    public void ParseStatement_DeclarationInferredType_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source    = "var foo := 1 + 2;";
        var          tokens    = ParsingUtilities.ReadAllTokens(source);
        var          index     = 0;
        var          validator = new SyntaxNodeValidationVisitor();

        // Act
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(8, index);
        Assert.Equal(TokenType.Eof, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<VariableDeclarationStatement>(statement);
        var variableDeclaration = (VariableDeclarationStatement)statement;

        Assert.Equal(TokenType.Var, variableDeclaration.DeclarationKeyword.Type);
        Assert.Equal("var", variableDeclaration.DeclarationKeyword.Value);

        Assert.Equal(TokenType.Identifier, variableDeclaration.Identifier.Type);
        Assert.Equal("foo", variableDeclaration.Identifier.Value);

        Assert.Equal(TokenType.Colon, variableDeclaration.TypeAssignment.Type);
        Assert.Equal(":", variableDeclaration.TypeAssignment.Value);

        Assert.Null(variableDeclaration.Type);

        Assert.Equal(TokenType.Assign, variableDeclaration.ValueAssignment.Type);
        Assert.Equal("=", variableDeclaration.ValueAssignment.Value);

        Assert.IsType<BinaryExpression>(variableDeclaration.Value);

        Assert.Equal(TokenType.Semicolon, variableDeclaration.Terminator.Type);
        Assert.Equal(";", variableDeclaration.Terminator.Value);

        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.False(validator.HasErrors);
    }

    [Fact]
    public void ParseStatement_DeclarationNoAssignment_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source    = "var foo: i32;";
        var          tokens    = ParsingUtilities.ReadAllTokens(source);
        var          index     = 0;
        var          validator = new SyntaxNodeValidationVisitor();

        // Act
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(5, index);
        Assert.Equal(TokenType.Eof, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<VariableDeclarationStatement>(statement);
        var variableDeclaration = (VariableDeclarationStatement)statement;

        Assert.Equal(TokenType.Var, variableDeclaration.DeclarationKeyword.Type);
        Assert.Equal("var", variableDeclaration.DeclarationKeyword.Value);

        Assert.Equal(TokenType.Identifier, variableDeclaration.Identifier.Type);
        Assert.Equal("foo", variableDeclaration.Identifier.Value);

        Assert.Equal(TokenType.Colon, variableDeclaration.TypeAssignment.Type);
        Assert.Equal(":", variableDeclaration.TypeAssignment.Value);

        Assert.IsType<TypeIdentifierExpression>(variableDeclaration.Type);
        var identifier = (TypeIdentifierExpression)variableDeclaration.Type;
        Assert.Equal(TokenType.Identifier, identifier.Token.Type);
        Assert.Equal("i32", identifier.Token.Value);

        Assert.Equal(TokenType.None, variableDeclaration.ValueAssignment.Type);
        Assert.Equal("", variableDeclaration.ValueAssignment.Value);

        Assert.Null(variableDeclaration.Value);

        Assert.Equal(TokenType.Semicolon, variableDeclaration.Terminator.Type);
        Assert.Equal(";", variableDeclaration.Terminator.Value);

        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.False(validator.HasErrors);
    }

    [Fact]
    public void ParseStatement_DeclarationGenericNoAssignment_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source    = "var foo: SomeType<Type1, Type2>;";
        var          tokens    = ParsingUtilities.ReadAllTokens(source);
        var          index     = 0;
        var          validator = new SyntaxNodeValidationVisitor();

        // Act
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(10, index);
        Assert.Equal(TokenType.Eof, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<VariableDeclarationStatement>(statement);
        var variableDeclaration = (VariableDeclarationStatement)statement;

        Assert.Equal(TokenType.Var, variableDeclaration.DeclarationKeyword.Type);
        Assert.Equal("var", variableDeclaration.DeclarationKeyword.Value);

        Assert.Equal(TokenType.Identifier, variableDeclaration.Identifier.Type);
        Assert.Equal("foo", variableDeclaration.Identifier.Value);

        Assert.Equal(TokenType.Colon, variableDeclaration.TypeAssignment.Type);
        Assert.Equal(":", variableDeclaration.TypeAssignment.Value);

        Assert.IsType<TypeGenericExpression>(variableDeclaration.Type);
        var generic = (TypeGenericExpression)variableDeclaration.Type;
        Assert.Equal(TokenType.Identifier, generic.Identifier.Type);
        Assert.Equal("SomeType", generic.Identifier.Value);
        Assert.Equal(TokenType.LessThan, generic.LessThan.Type);
        Assert.Equal("<", generic.LessThan.Value);
        Assert.Equal(TokenType.GreaterThan, generic.GreaterThan.Type);
        Assert.Equal(">", generic.GreaterThan.Value);
        Assert.NotNull(generic.TypeArguments);
        Assert.Equal(2, generic.TypeArguments.Count);
        Assert.IsType<TypeArgumentExpression>(generic.TypeArguments[0]);
        Assert.IsType<TypeArgumentExpression>(generic.TypeArguments[1]);

        Assert.Equal(TokenType.None, variableDeclaration.ValueAssignment.Type);
        Assert.Equal("", variableDeclaration.ValueAssignment.Value);

        Assert.Null(variableDeclaration.Value);

        Assert.Equal(TokenType.Semicolon, variableDeclaration.Terminator.Type);
        Assert.Equal(";", variableDeclaration.Terminator.Value);

        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.False(validator.HasErrors);
    }

    [Fact]
    public void ParseStatement_VarDeclarationNoIdentifier_IdentifierExpected()
    {
        // Arrange
        const string source    = "var := 1 + 2;";
        var          tokens    = ParsingUtilities.ReadAllTokens(source);
        var          index     = 0;
        var          validator = new SyntaxNodeValidationVisitor();

        // Act
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(7, index);
        Assert.Equal(TokenType.Eof, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<VariableDeclarationStatement>(statement);
        var variableDeclaration = (VariableDeclarationStatement)statement;

        Assert.Equal(TokenType.Var, variableDeclaration.DeclarationKeyword.Type);
        Assert.Equal("var", variableDeclaration.DeclarationKeyword.Value);

        Assert.Equal(TokenType.None, variableDeclaration.Identifier.Type);
        Assert.Equal("", variableDeclaration.Identifier.Value);

        Assert.Equal(TokenType.Colon, variableDeclaration.TypeAssignment.Type);
        Assert.Equal(":", variableDeclaration.TypeAssignment.Value);

        Assert.Null(variableDeclaration.Type);

        Assert.Equal(TokenType.Assign, variableDeclaration.ValueAssignment.Type);
        Assert.Equal("=", variableDeclaration.ValueAssignment.Value);

        Assert.IsType<BinaryExpression>(variableDeclaration.Value);

        Assert.Equal(TokenType.Semicolon, variableDeclaration.Terminator.Type);
        Assert.Equal(";", variableDeclaration.Terminator.Value);

        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(
            validator.Diagnostics,
            d => d.Message.EndsWith(VariableDeclarationStatement.IdentifierExpected));
    }

    [Fact]
    public void ParseStatement_VarDeclarationSwapColonAndAssignment_ColonExpected()
    {
        // Arrange
        const string source    = "var foo =: 1 + 2;";
        var          tokens    = ParsingUtilities.ReadAllTokens(source);
        var          index     = 0;
        var          validator = new SyntaxNodeValidationVisitor();

        // Act
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(2, index);
        Assert.Equal(TokenType.Assign, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<VariableDeclarationStatement>(statement);
        var variableDeclaration = (VariableDeclarationStatement)statement;

        Assert.Equal(TokenType.Var, variableDeclaration.DeclarationKeyword.Type);
        Assert.Equal("var", variableDeclaration.DeclarationKeyword.Value);

        Assert.Equal(TokenType.Identifier, variableDeclaration.Identifier.Type);
        Assert.Equal("foo", variableDeclaration.Identifier.Value);

        Assert.Equal(TokenType.None, variableDeclaration.TypeAssignment.Type);
        Assert.Equal("", variableDeclaration.TypeAssignment.Value);

        Assert.Null(variableDeclaration.Type);

        Assert.Equal(TokenType.None, variableDeclaration.ValueAssignment.Type);
        Assert.Equal("", variableDeclaration.ValueAssignment.Value);

        Assert.Null(variableDeclaration.Value);

        Assert.Equal(TokenType.None, variableDeclaration.Terminator.Type);
        Assert.Equal("", variableDeclaration.Terminator.Value);

        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(
            validator.Diagnostics,
            d => d.Message.EndsWith(VariableDeclarationStatement.ColonExpected));
    }

    [Fact]
    public void ParseStatement_VarDeclarationNoColon_ColonExpected()
    {
        // Arrange
        const string source    = "var foo = 1 + 2;";
        var          tokens    = ParsingUtilities.ReadAllTokens(source);
        var          index     = 0;
        var          validator = new SyntaxNodeValidationVisitor();

        // Act
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(2, index);
        Assert.Equal(TokenType.Assign, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<VariableDeclarationStatement>(statement);
        var variableDeclaration = (VariableDeclarationStatement)statement;

        Assert.Equal(TokenType.Var, variableDeclaration.DeclarationKeyword.Type);
        Assert.Equal("var", variableDeclaration.DeclarationKeyword.Value);

        Assert.Equal(TokenType.Identifier, variableDeclaration.Identifier.Type);
        Assert.Equal("foo", variableDeclaration.Identifier.Value);

        Assert.Equal(TokenType.None, variableDeclaration.TypeAssignment.Type);
        Assert.Equal("", variableDeclaration.TypeAssignment.Value);

        Assert.Null(variableDeclaration.Type);

        Assert.Equal(TokenType.None, variableDeclaration.ValueAssignment.Type);
        Assert.Equal("", variableDeclaration.ValueAssignment.Value);

        Assert.Null(variableDeclaration.Value);

        Assert.Equal(TokenType.None, variableDeclaration.Terminator.Type);
        Assert.Equal("", variableDeclaration.Terminator.Value);

        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(
            validator.Diagnostics,
            d => d.Message.EndsWith(VariableDeclarationStatement.ColonExpected));
    }

    [Fact]
    public void ParseStatement_VarDeclarationNoColonNoAssignment_ColonExpected()
    {
        // Arrange
        const string source    = "var foo i32 1 + 2;";
        var          tokens    = ParsingUtilities.ReadAllTokens(source);
        var          index     = 0;
        var          validator = new SyntaxNodeValidationVisitor();

        // Act
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(3, index);
        Assert.Equal(TokenType.Integer, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<VariableDeclarationStatement>(statement);
        var variableDeclaration = (VariableDeclarationStatement)statement;

        Assert.Equal(TokenType.Var, variableDeclaration.DeclarationKeyword.Type);
        Assert.Equal("var", variableDeclaration.DeclarationKeyword.Value);

        Assert.Equal(TokenType.Identifier, variableDeclaration.Identifier.Type);
        Assert.Equal("foo", variableDeclaration.Identifier.Value);

        Assert.Equal(TokenType.None, variableDeclaration.TypeAssignment.Type);
        Assert.Equal("", variableDeclaration.TypeAssignment.Value);

        Assert.IsType<TypeIdentifierExpression>(variableDeclaration.Type);

        Assert.Equal(TokenType.None, variableDeclaration.ValueAssignment.Type);
        Assert.Equal("", variableDeclaration.ValueAssignment.Value);

        Assert.Null(variableDeclaration.Value);

        Assert.Equal(TokenType.None, variableDeclaration.Terminator.Type);
        Assert.Equal("", variableDeclaration.Terminator.Value);

        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(
            validator.Diagnostics,
            d => d.Message.EndsWith(VariableDeclarationStatement.ColonExpected));
    }

    [Fact]
    public void ParseStatement_VarDeclarationNoSemicolon1_SemicolonExpected()
    {
        // Arrange
        const string source = """
                              var foo: i32 = 1 + 2
                              var bar := 2;
                              """;
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var validator = new SyntaxNodeValidationVisitor();

        // Act
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(8, index);
        Assert.Equal(TokenType.Var, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<VariableDeclarationStatement>(statement);
        var variableDeclaration = (VariableDeclarationStatement)statement;

        Assert.Equal(TokenType.Var, variableDeclaration.DeclarationKeyword.Type);
        Assert.Equal("var", variableDeclaration.DeclarationKeyword.Value);

        Assert.Equal(TokenType.Identifier, variableDeclaration.Identifier.Type);
        Assert.Equal("foo", variableDeclaration.Identifier.Value);

        Assert.Equal(TokenType.Colon, variableDeclaration.TypeAssignment.Type);
        Assert.Equal(":", variableDeclaration.TypeAssignment.Value);

        Assert.IsType<TypeIdentifierExpression>(variableDeclaration.Type);

        Assert.Equal(TokenType.Assign, variableDeclaration.ValueAssignment.Type);
        Assert.Equal("=", variableDeclaration.ValueAssignment.Value);

        Assert.IsType<BinaryExpression>(variableDeclaration.Value);

        Assert.Equal(TokenType.None, variableDeclaration.Terminator.Type);
        Assert.Equal("", variableDeclaration.Terminator.Value);

        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(
            validator.Diagnostics,
            d => d.Message.EndsWith(VariableDeclarationStatement.SemicolonExpected));
    }

    [Fact]
    public void ParseStatement_VarDeclarationNoSemicolon2_SemicolonExpected()
    {
        // Arrange
        const string source = """
                              var foo: i32 = 1 + 2
                              bar = 2;
                              """;
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var validator = new SyntaxNodeValidationVisitor();

        // Act
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(8, index);
        Assert.Equal(TokenType.Identifier, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<VariableDeclarationStatement>(statement);
        var variableDeclaration = (VariableDeclarationStatement)statement;

        Assert.Equal(TokenType.Var, variableDeclaration.DeclarationKeyword.Type);
        Assert.Equal("var", variableDeclaration.DeclarationKeyword.Value);

        Assert.Equal(TokenType.Identifier, variableDeclaration.Identifier.Type);
        Assert.Equal("foo", variableDeclaration.Identifier.Value);

        Assert.Equal(TokenType.Colon, variableDeclaration.TypeAssignment.Type);
        Assert.Equal(":", variableDeclaration.TypeAssignment.Value);

        Assert.IsType<TypeIdentifierExpression>(variableDeclaration.Type);

        Assert.Equal(TokenType.Assign, variableDeclaration.ValueAssignment.Type);
        Assert.Equal("=", variableDeclaration.ValueAssignment.Value);

        Assert.IsType<BinaryExpression>(variableDeclaration.Value);

        Assert.Equal(TokenType.None, variableDeclaration.Terminator.Type);
        Assert.Equal("", variableDeclaration.Terminator.Value);

        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(
            validator.Diagnostics,
            d => d.Message.EndsWith(VariableDeclarationStatement.SemicolonExpected));
    }

    [Fact]
    public void ParseStatement_VarDeclarationNoAssignmentNoSemicolon1_SemicolonExpected()
    {
        // Arrange
        const string source = """
                              var foo: i32
                              var bar := 2;
                              """;
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var validator = new SyntaxNodeValidationVisitor();

        // Act
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(4, index);
        Assert.Equal(TokenType.Var, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<VariableDeclarationStatement>(statement);
        var variableDeclaration = (VariableDeclarationStatement)statement;

        Assert.Equal(TokenType.Var, variableDeclaration.DeclarationKeyword.Type);
        Assert.Equal("var", variableDeclaration.DeclarationKeyword.Value);

        Assert.Equal(TokenType.Identifier, variableDeclaration.Identifier.Type);
        Assert.Equal("foo", variableDeclaration.Identifier.Value);

        Assert.Equal(TokenType.Colon, variableDeclaration.TypeAssignment.Type);
        Assert.Equal(":", variableDeclaration.TypeAssignment.Value);

        Assert.IsType<TypeIdentifierExpression>(variableDeclaration.Type);

        Assert.Equal(TokenType.None, variableDeclaration.ValueAssignment.Type);
        Assert.Equal("", variableDeclaration.ValueAssignment.Value);

        Assert.Null(variableDeclaration.Value);

        Assert.Equal(TokenType.None, variableDeclaration.Terminator.Type);
        Assert.Equal("", variableDeclaration.Terminator.Value);

        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(
            validator.Diagnostics,
            d => d.Message.EndsWith(VariableDeclarationStatement.SemicolonExpected));
    }

    [Fact]
    public void ParseStatement_VarDeclarationNoAssignmentNoSemicolon2_SemicolonExpected()
    {
        // Arrange
        const string source = """
                              var foo: i32
                              bar = 2;
                              """;
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var validator = new SyntaxNodeValidationVisitor();

        // Act
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(4, index);
        Assert.Equal(TokenType.Identifier, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<VariableDeclarationStatement>(statement);
        var variableDeclaration = (VariableDeclarationStatement)statement;

        Assert.Equal(TokenType.Var, variableDeclaration.DeclarationKeyword.Type);
        Assert.Equal("var", variableDeclaration.DeclarationKeyword.Value);

        Assert.Equal(TokenType.Identifier, variableDeclaration.Identifier.Type);
        Assert.Equal("foo", variableDeclaration.Identifier.Value);

        Assert.Equal(TokenType.Colon, variableDeclaration.TypeAssignment.Type);
        Assert.Equal(":", variableDeclaration.TypeAssignment.Value);

        Assert.IsType<TypeIdentifierExpression>(variableDeclaration.Type);

        Assert.Equal(TokenType.None, variableDeclaration.ValueAssignment.Type);
        Assert.Equal("", variableDeclaration.ValueAssignment.Value);

        Assert.Null(variableDeclaration.Value);

        Assert.Equal(TokenType.None, variableDeclaration.Terminator.Type);
        Assert.Equal("", variableDeclaration.Terminator.Value);

        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(
            validator.Diagnostics,
            d => d.Message.EndsWith(VariableDeclarationStatement.SemicolonExpected));
    }
}