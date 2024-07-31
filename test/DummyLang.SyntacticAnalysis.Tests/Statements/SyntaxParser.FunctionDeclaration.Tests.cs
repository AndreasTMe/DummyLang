using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Expressions;
using DummyLang.SyntacticAnalysis.Parsers;
using DummyLang.SyntacticAnalysis.Statements;
using DummyLang.SyntacticAnalysis.Utilities;
using DummyLang.SyntacticAnalysis.Visitors;
using Xunit;

namespace DummyLang.SyntacticAnalysis.Tests.Statements;

public class FunctionDeclarationSyntaxParserTests
{
    [Fact]
    public void ParseStatement_FunctionDeclarationNoParams_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source = """
                              func test: () => void = () => {
                              }
                              """;
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var validator = new SyntaxNodeValidationVisitor();

        // Act
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(13, index);
        Assert.Equal(TokenType.Eof, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<FunctionDeclarationStatement>(statement);
        var functionDeclaration = (FunctionDeclarationStatement)statement;

        Assert.Equal(TokenType.Func, functionDeclaration.FuncKeyword.Type);
        Assert.Equal("func", functionDeclaration.FuncKeyword.Value);

        Assert.Equal(TokenType.Identifier, functionDeclaration.Identifier.Type);
        Assert.Equal("test", functionDeclaration.Identifier.Value);

        Assert.Equal(TokenType.Colon, functionDeclaration.TypeAssignment.Type);
        Assert.Equal(":", functionDeclaration.TypeAssignment.Value);

        Assert.IsType<TypeFunctionExpression>(functionDeclaration.Type);

        Assert.Equal(TokenType.Assign, functionDeclaration.ValueAssignment.Type);
        Assert.Equal("=", functionDeclaration.ValueAssignment.Value);

        Assert.Equal(TokenType.LeftParenthesis, functionDeclaration.LeftParenthesis.Type);
        Assert.Equal("(", functionDeclaration.LeftParenthesis.Value);

        Assert.Null(functionDeclaration.Parameters);

        Assert.Equal(TokenType.RightParenthesis, functionDeclaration.RightParenthesis.Type);
        Assert.Equal(")", functionDeclaration.RightParenthesis.Value);

        Assert.Equal(TokenType.LambdaAssign, functionDeclaration.LambdaAssign.Type);
        Assert.Equal("=>", functionDeclaration.LambdaAssign.Value);

        Assert.IsType<CompoundStatement>(functionDeclaration.Block);

        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.False(validator.HasErrors);
    }

    [Fact]
    public void ParseStatement_FunctionDeclarationWithParams_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source = """
                              func test: (i32, i32) => i32 = (a, b) => {
                                  return a + b;
                              }
                              """;
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var validator = new SyntaxNodeValidationVisitor();

        // Act
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(24, index);
        Assert.Equal(TokenType.Eof, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<FunctionDeclarationStatement>(statement);
        var functionDeclaration = (FunctionDeclarationStatement)statement;

        Assert.Equal(TokenType.Func, functionDeclaration.FuncKeyword.Type);
        Assert.Equal("func", functionDeclaration.FuncKeyword.Value);

        Assert.Equal(TokenType.Identifier, functionDeclaration.Identifier.Type);
        Assert.Equal("test", functionDeclaration.Identifier.Value);

        Assert.Equal(TokenType.Colon, functionDeclaration.TypeAssignment.Type);
        Assert.Equal(":", functionDeclaration.TypeAssignment.Value);

        Assert.IsType<TypeFunctionExpression>(functionDeclaration.Type);

        Assert.Equal(TokenType.Assign, functionDeclaration.ValueAssignment.Type);
        Assert.Equal("=", functionDeclaration.ValueAssignment.Value);

        Assert.Equal(TokenType.LeftParenthesis, functionDeclaration.LeftParenthesis.Type);
        Assert.Equal("(", functionDeclaration.LeftParenthesis.Value);

        Assert.NotNull(functionDeclaration.Parameters);
        Assert.Equal(2, functionDeclaration.Parameters.Count);
        Assert.IsType<ParameterExpression>(functionDeclaration.Parameters[0]);
        Assert.IsType<ParameterExpression>(functionDeclaration.Parameters[1]);

        Assert.Equal(TokenType.RightParenthesis, functionDeclaration.RightParenthesis.Type);
        Assert.Equal(")", functionDeclaration.RightParenthesis.Value);

        Assert.Equal(TokenType.LambdaAssign, functionDeclaration.LambdaAssign.Type);
        Assert.Equal("=>", functionDeclaration.LambdaAssign.Value);

        Assert.IsType<CompoundStatement>(functionDeclaration.Block);

        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.False(validator.HasErrors);
    }
    
    [Fact]
    public void ParseStatement_FunctionImplicitDeclarationNoParams_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source = """
                              func test := () => {
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
        Assert.IsType<FunctionDeclarationWithImplicitTypeStatement>(statement);
        var functionDeclaration = (FunctionDeclarationWithImplicitTypeStatement)statement;

        Assert.Equal(TokenType.Func, functionDeclaration.FuncKeyword.Type);
        Assert.Equal("func", functionDeclaration.FuncKeyword.Value);

        Assert.Equal(TokenType.Identifier, functionDeclaration.Identifier.Type);
        Assert.Equal("test", functionDeclaration.Identifier.Value);

        Assert.Equal(TokenType.Colon, functionDeclaration.TypeAssignment.Type);
        Assert.Equal(":", functionDeclaration.TypeAssignment.Value);

        Assert.Equal(TokenType.Assign, functionDeclaration.ValueAssignment.Type);
        Assert.Equal("=", functionDeclaration.ValueAssignment.Value);

        Assert.Equal(TokenType.LeftParenthesis, functionDeclaration.LeftParenthesis.Type);
        Assert.Equal("(", functionDeclaration.LeftParenthesis.Value);

        Assert.Null(functionDeclaration.TypedParameters);

        Assert.Equal(TokenType.RightParenthesis, functionDeclaration.RightParenthesis.Type);
        Assert.Equal(")", functionDeclaration.RightParenthesis.Value);

        Assert.Equal(TokenType.LambdaAssign, functionDeclaration.LambdaAssign.Type);
        Assert.Equal("=>", functionDeclaration.LambdaAssign.Value);

        Assert.IsType<CompoundStatement>(functionDeclaration.Block);

        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.False(validator.HasErrors);
    }

    [Fact]
    public void ParseStatement_FunctionImplicitDeclarationWithParams_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source = """
                              func test := (a: i32, b: i32) => {
                                  return a + b;
                              }
                              """;
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var validator = new SyntaxNodeValidationVisitor();

        // Act
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        Assert.Equal(21, index);
        Assert.Equal(TokenType.Eof, tokens[index].Type);

        Assert.NotNull(statement);
        Assert.IsType<FunctionDeclarationWithImplicitTypeStatement>(statement);
        var functionDeclaration = (FunctionDeclarationWithImplicitTypeStatement)statement;

        Assert.Equal(TokenType.Func, functionDeclaration.FuncKeyword.Type);
        Assert.Equal("func", functionDeclaration.FuncKeyword.Value);

        Assert.Equal(TokenType.Identifier, functionDeclaration.Identifier.Type);
        Assert.Equal("test", functionDeclaration.Identifier.Value);

        Assert.Equal(TokenType.Colon, functionDeclaration.TypeAssignment.Type);
        Assert.Equal(":", functionDeclaration.TypeAssignment.Value);

        Assert.Equal(TokenType.Assign, functionDeclaration.ValueAssignment.Type);
        Assert.Equal("=", functionDeclaration.ValueAssignment.Value);

        Assert.Equal(TokenType.LeftParenthesis, functionDeclaration.LeftParenthesis.Type);
        Assert.Equal("(", functionDeclaration.LeftParenthesis.Value);

        Assert.NotNull(functionDeclaration.TypedParameters);
        Assert.Equal(2, functionDeclaration.TypedParameters.Count);
        Assert.IsType<TypeParameterExpression>(functionDeclaration.TypedParameters[0]);
        Assert.IsType<TypeParameterExpression>(functionDeclaration.TypedParameters[1]);

        Assert.Equal(TokenType.RightParenthesis, functionDeclaration.RightParenthesis.Type);
        Assert.Equal(")", functionDeclaration.RightParenthesis.Value);

        Assert.Equal(TokenType.LambdaAssign, functionDeclaration.LambdaAssign.Type);
        Assert.Equal("=>", functionDeclaration.LambdaAssign.Value);

        Assert.IsType<CompoundStatement>(functionDeclaration.Block);

        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.False(validator.HasErrors);
    }
}