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

        Assert.Null(functionCall.Arguments);
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

        Assert.NotNull(functionCall.Arguments);
        Assert.Equal(3, functionCall.Arguments.Count);

        Assert.NotNull(functionCall.Arguments[0].Argument);
        Assert.IsType<NumberLiteralExpression>(functionCall.Arguments[0].Argument);
        Assert.Equal(TokenType.Comma, functionCall.Arguments[0].Comma.Type);
        Assert.Equal(",", functionCall.Arguments[0].Comma.Value);

        Assert.NotNull(functionCall.Arguments[1].Argument);
        Assert.IsType<ParenthesisedExpression>(functionCall.Arguments[1].Argument);
        Assert.Equal(TokenType.Comma, functionCall.Arguments[1].Comma.Type);
        Assert.Equal(",", functionCall.Arguments[1].Comma.Value);

        // TODO: Update the following line when lambdas are added
        Assert.NotNull(functionCall.Arguments[2].Argument);
        Assert.IsType<BinaryExpression>(functionCall.Arguments[2].Argument);
        Assert.Equal(TokenType.None, functionCall.Arguments[2].Comma.Type);
        Assert.Equal("", functionCall.Arguments[2].Comma.Value);
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

        Assert.NotNull(functionCall.Arguments);
        Assert.Equal(1, functionCall.Arguments.Count);
        Assert.IsType<ArgumentExpression>(functionCall.Arguments[0]);
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

        Assert.NotNull(functionCall.Arguments);
        Assert.Equal(2, functionCall.Arguments.Count);
        Assert.IsType<ArgumentExpression>(functionCall.Arguments[0]);
        Assert.IsType<ArgumentExpression>(functionCall.Arguments[1]);
    }
}