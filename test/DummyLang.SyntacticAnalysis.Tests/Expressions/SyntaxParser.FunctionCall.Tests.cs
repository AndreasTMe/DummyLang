using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Expressions;
using DummyLang.SyntacticAnalysis.Parsers;
using DummyLang.SyntacticAnalysis.Utilities;
using Xunit;

namespace DummyLang.SyntacticAnalysis.Tests.Expressions;

public class FunctionCallSyntaxParserTests
{
    [Fact]
    public void ParseExpression_FunctionCallNoParams_ReadSuccessfully()
    {
        // Arrange
        const string source = "test()";

        // Act
        var tokens     = ParsingUtilities.ReadAllTokens(source);
        var index      = 0;
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(4, tokens.Length);
        Assert.Equal(3, index);
        Assert.IsType<PrimaryExpression>(expression);

        var primary = (PrimaryExpression)expression;
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
    public void ParseExpression_FunctionCallWithParams_ReadSuccessfully()
    {
        // Arrange
        const string source = "test(1, (2 + 3), b => DoSomething())";

        // Act
        var tokens     = ParsingUtilities.ReadAllTokens(source);
        var index      = 0;
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(17, tokens.Length);
        Assert.Equal(16, index);
        Assert.IsType<PrimaryExpression>(expression);

        var primary = (PrimaryExpression)expression;
        Assert.Equal(Token.None, primary.Token);
        Assert.IsType<FunctionCallExpression>(primary.Expression);

        var functionCall = (FunctionCallExpression)primary.Expression;
        Assert.Equal(TokenType.Identifier, functionCall.Identifier.Type);
        Assert.Equal("test", functionCall.Identifier.Value);

        Assert.Equal(TokenType.LeftParenthesis, functionCall.LeftParenthesis.Type);
        Assert.Equal("(", functionCall.LeftParenthesis.Value);

        Assert.Equal(TokenType.RightParenthesis, functionCall.RightParenthesis.Type);
        Assert.Equal(")", functionCall.RightParenthesis.Value);
        
        // TODO: Update the following 6 lines when lambdas are added
        Assert.Equal(5, functionCall.Parameters.Count); 
        Assert.IsType<NumberLiteralExpression>(functionCall.Parameters[0]);
        Assert.IsType<ParenthesisedExpression>(functionCall.Parameters[1]);
        Assert.IsType<IdentifierExpression>(functionCall.Parameters[2]);
        Assert.IsType<InvalidExpression>(functionCall.Parameters[3]);
        Assert.IsType<PrimaryExpression>(functionCall.Parameters[4]);

        Assert.Equal(2, functionCall.Commas.Count);
        Assert.Equal(TokenType.Comma, functionCall.Commas[0].Type);
        Assert.Equal(",", functionCall.Commas[0].Value);
        Assert.Equal(TokenType.Comma, functionCall.Commas[1].Type);
        Assert.Equal(",", functionCall.Commas[1].Value);
    }
    
    [Fact]
    public void ParseExpression_FunctionCall_NoClosingParenthesis()
    {
        // Arrange
        const string source = "test(";

        // Act
        var tokens     = ParsingUtilities.ReadAllTokens(source);
        var index      = 0;
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(3, tokens.Length);
        Assert.Equal(2, index);
        Assert.IsType<InvalidExpression>(expression);
        
        var invalid = (InvalidExpression)expression;
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
    public void ParseExpression_FunctionCall_NoClosingParenthesisWithParameter()
    {
        // Arrange
        const string source = "test(1";

        // Act
        var tokens     = ParsingUtilities.ReadAllTokens(source);
        var index      = 0;
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(4, tokens.Length);
        Assert.Equal(3, index);
        Assert.IsType<InvalidExpression>(expression);
        
        var invalid = (InvalidExpression)expression;
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
    public void ParseExpression_FunctionCall_ReservedKeywordFound()
    {
        // Arrange
        const string source = """
                              test(1, 2
                              var t = 1;
                              """;

        // Act
        var tokens     = ParsingUtilities.ReadAllTokens(source);
        var index      = 0;
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(11, tokens.Length);
        Assert.Equal(5, index);
        Assert.IsType<InvalidExpression>(expression);
        
        var invalid = (InvalidExpression)expression;
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