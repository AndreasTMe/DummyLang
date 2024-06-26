using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Expressions;
using Xunit;

namespace DummyLang.SyntacticAnalysis.Tests.Expressions;

public class FunctionCallSyntaxParserTests
{
    [Fact]
    public void GenerateSyntax_FunctionCallNoParams_ReadSuccessfully()
    {
        // Arrange
        const string source = "test()";

        // Act
        var parser = new SyntaxParser();
        var syntaxTree = parser.Feed(source)
                               .GenerateSyntax();

        // Assert
        Assert.NotNull(syntaxTree);
        Assert.Equal(1, syntaxTree.Nodes.Count);
        Assert.IsType<PrimaryExpression>(syntaxTree.Nodes[0]);

        var primary = (PrimaryExpression)syntaxTree.Nodes[0];
        Assert.Equal(Token.None, primary.Token);
        Assert.IsType<FunctionCallExpression>(primary.Expression);

        var functionCall = (FunctionCallExpression)primary.Expression;
        Assert.Equal(TokenType.Identifier, functionCall.Identifier.Type);
        Assert.Equal("test", functionCall.Identifier.Value);

        Assert.Equal(TokenType.LeftParenthesis, functionCall.LeftParenthesis.Type);
        Assert.Equal("(", functionCall.LeftParenthesis.Value);

        Assert.Equal(TokenType.RightParenthesis, functionCall.RightParenthesis.Type);
        Assert.Equal(")", functionCall.RightParenthesis.Value);

        Assert.Empty(functionCall.Parameters);
        Assert.Empty(functionCall.Commas);
    }

    [Fact]
    public void GenerateSyntax_FunctionCallWithParams_ReadSuccessfully()
    {
        // Arrange
        const string source = "test(1, (2 + 3), b => DoSomething())";

        // Act
        var parser = new SyntaxParser();
        var syntaxTree = parser.Feed(source)
                               .GenerateSyntax();

        // Assert
        Assert.NotNull(syntaxTree);
        Assert.Equal(1, syntaxTree.Nodes.Count);
        Assert.IsType<PrimaryExpression>(syntaxTree.Nodes[0]);

        var primary = (PrimaryExpression)syntaxTree.Nodes[0];
        Assert.Equal(Token.None, primary.Token);
        Assert.IsType<FunctionCallExpression>(primary.Expression);

        var functionCall = (FunctionCallExpression)primary.Expression;
        Assert.Equal(TokenType.Identifier, functionCall.Identifier.Type);
        Assert.Equal("test", functionCall.Identifier.Value);

        Assert.Equal(TokenType.LeftParenthesis, functionCall.LeftParenthesis.Type);
        Assert.Equal("(", functionCall.LeftParenthesis.Value);

        Assert.Equal(TokenType.RightParenthesis, functionCall.RightParenthesis.Type);
        Assert.Equal(")", functionCall.RightParenthesis.Value);

        Assert.Equal(3, functionCall.Parameters.Count);
        Assert.IsType<NumberLiteralExpression>(functionCall.Parameters[0]);
        Assert.IsType<ParenthesisedExpression>(functionCall.Parameters[1]);
        Assert.IsType<BinaryExpression>(functionCall.Parameters[2]); // TODO: Update when lambdas are here

        Assert.Equal(2, functionCall.Commas.Count);
        Assert.Equal(TokenType.Comma, functionCall.Commas[0].Type);
        Assert.Equal(",", functionCall.Commas[0].Value);
        Assert.Equal(TokenType.Comma, functionCall.Commas[1].Type);
        Assert.Equal(",", functionCall.Commas[1].Value);
    }
    
    [Fact]
    public void GenerateSyntax_FunctionCall_NoClosingParenthesis()
    {
        // Arrange
        const string source = "test(";

        // Act
        var parser = new SyntaxParser();
        var syntaxTree = parser.Feed(source)
                               .GenerateSyntax();

        // Assert
        Assert.NotNull(syntaxTree);
        Assert.Equal(1, syntaxTree.Nodes.Count);
        Assert.IsType<InvalidExpression>(syntaxTree.Nodes[0]);

        var invalid = (InvalidExpression)syntaxTree.Nodes[0];
        Assert.Equal(TokenType.LeftParenthesis, invalid.Token.Type);
        Assert.Equal("(", invalid.Token.Value);
        Assert.IsType<FunctionCallExpression>(invalid.Expression);

        var functionCall = (FunctionCallExpression)invalid.Expression;
        Assert.Equal(TokenType.Identifier, functionCall.Identifier.Type);
        Assert.Equal("test", functionCall.Identifier.Value);

        Assert.Equal(TokenType.LeftParenthesis, functionCall.LeftParenthesis.Type);
        Assert.Equal("(", functionCall.LeftParenthesis.Value);

        Assert.Equal(TokenType.None, functionCall.RightParenthesis.Type);
        Assert.Equal("", functionCall.RightParenthesis.Value);

        Assert.Empty(functionCall.Parameters);
        Assert.Empty(functionCall.Commas);
    }

    [Fact]
    public void GenerateSyntax_FunctionCall_NoClosingParenthesisWithParameter()
    {
        // Arrange
        const string source = "test(1";

        // Act
        var parser = new SyntaxParser();
        var syntaxTree = parser.Feed(source)
                               .GenerateSyntax();

        // Assert
        Assert.NotNull(syntaxTree);
        Assert.Equal(1, syntaxTree.Nodes.Count);
        Assert.IsType<InvalidExpression>(syntaxTree.Nodes[0]);

        var invalid = (InvalidExpression)syntaxTree.Nodes[0];
        Assert.Equal(TokenType.LeftParenthesis, invalid.Token.Type);
        Assert.Equal("(", invalid.Token.Value);
        Assert.IsType<FunctionCallExpression>(invalid.Expression);

        var functionCall = (FunctionCallExpression)invalid.Expression;
        Assert.Equal(TokenType.Identifier, functionCall.Identifier.Type);
        Assert.Equal("test", functionCall.Identifier.Value);

        Assert.Equal(TokenType.LeftParenthesis, functionCall.LeftParenthesis.Type);
        Assert.Equal("(", functionCall.LeftParenthesis.Value);

        Assert.Equal(TokenType.None, functionCall.RightParenthesis.Type);
        Assert.Equal("", functionCall.RightParenthesis.Value);

        Assert.Equal(1, functionCall.Parameters.Count);
        Assert.IsType<NumberLiteralExpression>(functionCall.Parameters[0]);

        Assert.Empty(functionCall.Commas);
    }

    [Fact]
    public void GenerateSyntax_FunctionCall_ReservedKeywordFound()
    {
        // Arrange
        const string source = """
                              test(1, 2
                              var t = 1;
                              """;

        // Act
        var parser = new SyntaxParser();
        var syntaxTree = parser.Feed(source)
                               .GenerateSyntax();

        // Assert
        Assert.NotNull(syntaxTree);
        Assert.Equal(3, syntaxTree.Nodes.Count); // TODO: Revisit this when declarations are implemented
        Assert.IsType<InvalidExpression>(syntaxTree.Nodes[0]);

        var invalid = (InvalidExpression)syntaxTree.Nodes[0];
        Assert.Equal(TokenType.LeftParenthesis, invalid.Token.Type);
        Assert.Equal("(", invalid.Token.Value);
        Assert.IsType<FunctionCallExpression>(invalid.Expression);

        var functionCall = (FunctionCallExpression)invalid.Expression;
        Assert.Equal(TokenType.Identifier, functionCall.Identifier.Type);
        Assert.Equal("test", functionCall.Identifier.Value);

        Assert.Equal(TokenType.LeftParenthesis, functionCall.LeftParenthesis.Type);
        Assert.Equal("(", functionCall.LeftParenthesis.Value);

        Assert.Equal(TokenType.None, functionCall.RightParenthesis.Type);
        Assert.Equal("", functionCall.RightParenthesis.Value);

        Assert.Equal(2, functionCall.Parameters.Count);
        Assert.IsType<NumberLiteralExpression>(functionCall.Parameters[0]);
        Assert.IsType<NumberLiteralExpression>(functionCall.Parameters[1]);

        Assert.Equal(1, functionCall.Commas.Count);
        Assert.Equal(TokenType.Comma, functionCall.Commas[0].Type);
        Assert.Equal(",", functionCall.Commas[0].Value);
    }
}