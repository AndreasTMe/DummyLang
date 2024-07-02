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

        Assert.Equal(3, functionCall.Parameters.Count);
        Assert.IsType<NumberLiteralExpression>(functionCall.Parameters[0]);
        Assert.IsType<ParenthesisedExpression>(functionCall.Parameters[1]);
        // TODO: Update the following line when lambdas are added
        Assert.IsType<BinaryExpression>(functionCall.Parameters[2]);

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
        Assert.Equal(1, index);
        Assert.IsType<InvalidExpression>(expression);

        var invalid = (InvalidExpression)expression;
        Assert.Single(invalid.Tokens);
        Assert.Equal(TokenType.RightParenthesis, invalid.Tokens[0].Type);
        Assert.Null(invalid.Expression);
        Assert.NotNull(invalid.ExpressionType);
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
        Assert.Empty(invalid.Tokens);
        Assert.NotNull(invalid.Expression);
        Assert.IsType<FunctionCallExpression>(invalid.Expression);
        Assert.NotNull(invalid.ExpressionType);
        Assert.Equal(typeof(FunctionCallExpression), invalid.ExpressionType);

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
        Assert.Empty(invalid.Tokens);
        Assert.NotNull(invalid.Expression);
        Assert.IsType<FunctionCallExpression>(invalid.Expression);
        Assert.NotNull(invalid.ExpressionType);
        Assert.Equal(typeof(FunctionCallExpression), invalid.ExpressionType);

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