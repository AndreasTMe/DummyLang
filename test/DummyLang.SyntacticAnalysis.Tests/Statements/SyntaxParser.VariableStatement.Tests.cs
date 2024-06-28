using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Expressions;
using DummyLang.SyntacticAnalysis.Statements;
using Xunit;

namespace DummyLang.SyntacticAnalysis.Tests.Statements;

public class VariableDeclarationSyntaxParserTests
{
    [Fact]
    public void ParseExpression_VarDeclaration_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source = "var foo: i32 = 1 + 2;";

        // Act
        var parser = new SyntaxTreeBuilder();
        var syntaxTree = parser.Feed(source)
                               .Build();

        // Assert
        Assert.NotNull(syntaxTree);
        Assert.NotEmpty(syntaxTree.Nodes);
        Assert.IsType<VariableDeclarationStatement>(syntaxTree.Nodes[0]);
        var variableDeclaration = (VariableDeclarationStatement)syntaxTree.Nodes[0];

        Assert.Equal(TokenType.Var, variableDeclaration.Declaration.Type);
        Assert.Equal("var", variableDeclaration.Declaration.Value);

        Assert.IsType<IdentifierExpression>(variableDeclaration.Identifier);

        Assert.Equal(TokenType.Colon, variableDeclaration.TypeAssignment.Type);
        Assert.Equal(":", variableDeclaration.TypeAssignment.Value);

        Assert.IsType<IdentifierExpression>(variableDeclaration.Type);
        var identifier = (IdentifierExpression)variableDeclaration.Type;
        Assert.Equal(TokenType.Identifier, identifier.Token.Type);
        Assert.Equal("i32", identifier.Token.Value);

        Assert.Equal(TokenType.Assign, variableDeclaration.ValueAssignment.Type);
        Assert.Equal("=", variableDeclaration.ValueAssignment.Value);

        Assert.IsType<BinaryExpression>(variableDeclaration.Value);

        Assert.Equal(TokenType.Semicolon, variableDeclaration.Terminator.Type);
        Assert.Equal(";", variableDeclaration.Terminator.Value);
    }

    [Fact]
    public void ParseExpression_ConstDeclaration_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source = "const foo: i32 = 1 + 2;";

        // Act
        var parser = new SyntaxTreeBuilder();
        var syntaxTree = parser.Feed(source)
                               .Build();

        // Assert
        Assert.NotNull(syntaxTree);
        Assert.NotEmpty(syntaxTree.Nodes);
        Assert.IsType<VariableDeclarationStatement>(syntaxTree.Nodes[0]);
        var variableDeclaration = (VariableDeclarationStatement)syntaxTree.Nodes[0];

        Assert.Equal(TokenType.Const, variableDeclaration.Declaration.Type);
        Assert.Equal("const", variableDeclaration.Declaration.Value);

        Assert.IsType<IdentifierExpression>(variableDeclaration.Identifier);

        Assert.Equal(TokenType.Colon, variableDeclaration.TypeAssignment.Type);
        Assert.Equal(":", variableDeclaration.TypeAssignment.Value);

        Assert.IsType<IdentifierExpression>(variableDeclaration.Type);
        var identifier = (IdentifierExpression)variableDeclaration.Type;
        Assert.Equal(TokenType.Identifier, identifier.Token.Type);
        Assert.Equal("i32", identifier.Token.Value);

        Assert.Equal(TokenType.Assign, variableDeclaration.ValueAssignment.Type);
        Assert.Equal("=", variableDeclaration.ValueAssignment.Value);

        Assert.IsType<BinaryExpression>(variableDeclaration.Value);

        Assert.Equal(TokenType.Semicolon, variableDeclaration.Terminator.Type);
        Assert.Equal(";", variableDeclaration.Terminator.Value);
    }

    [Fact]
    public void ParseExpression_DeclarationInferredType_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source = "var foo := 1 + 2;";

        // Act
        var parser = new SyntaxTreeBuilder();
        var syntaxTree = parser.Feed(source)
                               .Build();

        // Assert
        Assert.NotNull(syntaxTree);
        Assert.NotEmpty(syntaxTree.Nodes);
        Assert.IsType<VariableDeclarationStatement>(syntaxTree.Nodes[0]);
        var variableDeclaration = (VariableDeclarationStatement)syntaxTree.Nodes[0];

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

    // TODO: Fix the ones below
    // [Fact]
    // public void ParseExpression_DeclarationNoAssignment_ShouldBeReadCorrectly()
    // {
    //     // Arrange
    //     const string source = "var foo: i32;";
    //
    //     // Act
    //     var parser = new SyntaxTreeBuilder();
    //     var syntaxTree = parser.Feed(source)
    //                            .Build();
    //
    //     // Assert
    //     Assert.NotNull(syntaxTree);
    //     Assert.NotEmpty(syntaxTree.Nodes);
    //     Assert.IsType<VariableDeclarationStatement>(syntaxTree.Nodes[0]);
    //     var variableDeclaration = (VariableDeclarationStatement)syntaxTree.Nodes[0];
    //
    //     Assert.Equal(TokenType.Var, variableDeclaration.Declaration.Type);
    //     Assert.Equal("var", variableDeclaration.Declaration.Value);
    //
    //     Assert.IsType<IdentifierExpression>(variableDeclaration.Identifier);
    //
    //     Assert.Equal(TokenType.Colon, variableDeclaration.TypeAssignment.Type);
    //     Assert.Equal(":", variableDeclaration.TypeAssignment.Value);
    //
    //     Assert.IsType<IdentifierExpression>(variableDeclaration.Type);
    //     var identifier = (IdentifierExpression)variableDeclaration.Type;
    //     Assert.Equal(TokenType.Identifier, identifier.Token.Type);
    //     Assert.Equal("i32", identifier.Token.Value);
    //
    //     Assert.Equal(TokenType.None, variableDeclaration.ValueAssignment.Type);
    //     Assert.Equal("", variableDeclaration.ValueAssignment.Value);
    //
    //     Assert.Null(variableDeclaration.Value);
    //
    //     Assert.Equal(TokenType.Semicolon, variableDeclaration.Terminator.Type);
    //     Assert.Equal(";", variableDeclaration.Terminator.Value);
    // }
    //
    // [Fact]
    // public void ParseExpression_VarDeclarationNoColon_ShouldBeReadCorrectly()
    // {
    //     // Arrange
    //     const string source = "var foo = 1 + 2;";
    //
    //     // Act
    //     var parser = new SyntaxTreeBuilder();
    //     var syntaxTree = parser.Feed(source)
    //                            .Build();
    //
    //     Assert
    //     Assert.NotNull(syntaxTree);
    //     Assert.NotEmpty(syntaxTree.Nodes);
    //     Assert.IsType<VariableDeclarationStatement>(syntaxTree.Nodes[0]);
    //     var variableDeclaration = (VariableDeclarationStatement)syntaxTree.Nodes[0];
    //     
    //     Assert.Equal(TokenType.Var, variableDeclaration.Declaration.Type);
    //     Assert.Equal("var", variableDeclaration.Declaration.Value);
    //     
    //     Assert.IsType<IdentifierExpression>(variableDeclaration.Identifier);
    //     
    //     Assert.Equal(TokenType.None, variableDeclaration.TypeAssignment.Type);
    //     Assert.Equal("", variableDeclaration.TypeAssignment.Value);
    //     
    //     Assert.Null(variableDeclaration.Type);
    //     
    //     Assert.Equal(TokenType.Assign, variableDeclaration.ValueAssignment.Type);
    //     Assert.Equal("=", variableDeclaration.ValueAssignment.Value);
    //     
    //     Assert.IsType<BinaryExpression>(variableDeclaration.Value);
    //     
    //     Assert.Equal(TokenType.Semicolon, variableDeclaration.Terminator.Type);
    //     Assert.Equal(";", variableDeclaration.Terminator.Value);
    // }
    //
    // [Fact]
    // public void ParseExpression_VarDeclarationNoSemicolon_ShouldUseNextStatementAsStop()
    // {
    //     // Arrange
    //     const string source = """
    //                           var foo: i32 = 1 + 2
    //                           var bar := 2;
    //                           """;
    //
    //     // Act
    //     var parser = new SyntaxTreeBuilder();
    //     var syntaxTree = parser.Feed(source)
    //                            .Build();
    //
    //     Assert
    //     Assert.NotNull(syntaxTree);
    //     Assert.NotEmpty(syntaxTree.Nodes);
    //     Assert.IsType<VariableDeclarationStatement>(syntaxTree.Nodes[0]);
    //     var variableDeclaration = (VariableDeclarationStatement)syntaxTree.Nodes[0];
    //     
    //     Assert.Equal(TokenType.Var, variableDeclaration.DeclarationKeyword.Type);
    //     Assert.Equal("var", variableDeclaration.DeclarationKeyword.Value);
    //     
    //     Assert.IsType<IdentifierExpression>(variableDeclaration.Identifier);
    //     
    //     Assert.Equal(TokenType.Colon, variableDeclaration.TypeAssignmentOperator.Type);
    //     Assert.Equal(":", variableDeclaration.TypeAssignmentOperator.Value);
    //     
    //     Assert.IsType<IdentifierExpression>(variableDeclaration.Type);
    //     
    //     Assert.Equal(TokenType.Assign, variableDeclaration.ValueAssignmentOperator.Type);
    //     Assert.Equal("=", variableDeclaration.ValueAssignmentOperator.Value);
    //     
    //     Assert.IsType<BinaryExpression>(variableDeclaration.Value);
    //     
    //     Assert.Equal(TokenType.Semicolon, variableDeclaration.Terminator.Type);
    //     Assert.Equal(";", variableDeclaration.Terminator.Value);
    // }
}