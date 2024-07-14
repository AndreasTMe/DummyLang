using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Expressions;
using DummyLang.SyntacticAnalysis.Parsers;
using DummyLang.SyntacticAnalysis.Utilities;
using DummyLang.SyntacticAnalysis.Visitors;
using Xunit;

namespace DummyLang.SyntacticAnalysis.Tests.Expressions;

public class FunctionCallSyntaxParserTests
{
    [Fact]
    public void ParseExpression_FunctionCallNoParams_ReadSuccessfully()
    {
        // Arrange
        const string source    = "test()";
        var          validator = new SyntaxNodeValidationVisitor();
        var          tokens    = ParsingUtilities.ReadAllTokens(source);
        var          index     = 0;

        // Act
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

        Assert.False(validator.HasErrors);
        expression.Accept(validator);
        Assert.False(validator.HasErrors);
    }

    [Fact]
    public void ParseExpression_FunctionCallWithParams_ReadSuccessfully()
    {
        // Arrange
        const string source    = "test(1, (2 + 3), b => DoSomething())";
        var          validator = new SyntaxNodeValidationVisitor();
        var          tokens    = ParsingUtilities.ReadAllTokens(source);
        var          index     = 0;

        // Act
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

        Assert.False(validator.HasErrors);
        expression.Accept(validator);
        Assert.False(validator.HasErrors);
    }

    [Fact]
    public void ParseExpression_FunctionCall_NoClosingParenthesis()
    {
        // Arrange
        const string source    = "test(";
        var          validator = new SyntaxNodeValidationVisitor();
        var          tokens    = ParsingUtilities.ReadAllTokens(source);
        var          index     = 0;

        // Act
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(3, tokens.Length);
        Assert.Equal(2, index);
        Assert.IsType<PrimaryExpression>(expression);

        var primary = (PrimaryExpression)expression;
        Assert.Equal(Token.None, primary.Token);
        Assert.IsType<FunctionCallExpression>(primary.Expression);

        var functionCall = (FunctionCallExpression)primary.Expression;
        Assert.Equal(TokenType.Identifier, functionCall.Identifier.Type);
        Assert.Equal("test", functionCall.Identifier.Value);

        Assert.Equal(TokenType.LeftParenthesis, functionCall.LeftParenthesis.Type);
        Assert.Equal("(", functionCall.LeftParenthesis.Value);

        Assert.Equal(TokenType.None, functionCall.RightParenthesis.Type);
        Assert.Equal("", functionCall.RightParenthesis.Value);

        Assert.Null(functionCall.Arguments);

        Assert.False(validator.HasErrors);
        expression.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(
            validator.Diagnostics,
            d => d.Message.EndsWith(FunctionCallExpression.RightParenthesisExpected));
    }

    [Fact]
    public void ParseExpression_FunctionCall_NoClosingParenthesisWithParameter()
    {
        // Arrange
        const string source    = "test(1";
        var          validator = new SyntaxNodeValidationVisitor();
        var          tokens    = ParsingUtilities.ReadAllTokens(source);
        var          index     = 0;

        // Act
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

        Assert.Equal(TokenType.None, functionCall.RightParenthesis.Type);
        Assert.Equal("", functionCall.RightParenthesis.Value);

        Assert.NotNull(functionCall.Arguments);
        Assert.Equal(1, functionCall.Arguments.Count);
        Assert.IsType<ArgumentExpression>(functionCall.Arguments[0]);

        Assert.False(validator.HasErrors);
        expression.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(
            validator.Diagnostics,
            d => d.Message.EndsWith(FunctionCallExpression.RightParenthesisExpected));
    }

    [Fact]
    public void ParseExpression_FunctionCall_ReservedKeywordFound()
    {
        // Arrange
        const string source = """
                              test(1, 2
                              var t = 1;
                              """;
        var validator = new SyntaxNodeValidationVisitor();
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;

        // Act
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(11, tokens.Length);
        Assert.Equal(5, index);
        Assert.IsType<PrimaryExpression>(expression);

        var primary = (PrimaryExpression)expression;
        Assert.Equal(Token.None, primary.Token);
        Assert.IsType<FunctionCallExpression>(primary.Expression);

        var functionCall = (FunctionCallExpression)primary.Expression;
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

        Assert.False(validator.HasErrors);
        expression.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(
            validator.Diagnostics,
            d => d.Message.EndsWith(FunctionCallExpression.RightParenthesisExpected));
    }

    [Fact]
    public void ParseExpression_FunctionCall_LastArgumentHasComma()
    {
        // Arrange
        const string source    = "test(1,)";
        var          validator = new SyntaxNodeValidationVisitor();
        var          tokens    = ParsingUtilities.ReadAllTokens(source);
        var          index     = 0;

        // Act
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(6, tokens.Length);
        Assert.Equal(5, index);
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
        Assert.Equal(1, functionCall.Arguments.Count);
        Assert.IsType<ArgumentExpression>(functionCall.Arguments[0]);

        Assert.False(validator.HasErrors);
        expression.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(
            validator.Diagnostics,
            d => d.Message.EndsWith(FunctionCallExpression.LastArgumentHasComma));
    }

    [Fact]
    public void ParseExpression_FunctionCall_CommaExpected()
    {
        // Arrange
        const string source    = "test(1 2)";
        var          validator = new SyntaxNodeValidationVisitor();
        var          tokens    = ParsingUtilities.ReadAllTokens(source);
        var          index     = 0;

        // Act
        var expression = ExpressionParser.Parse(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(6, tokens.Length);
        Assert.Equal(5, index);
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
        Assert.Equal(2, functionCall.Arguments.Count);
        Assert.IsType<ArgumentExpression>(functionCall.Arguments[0]);
        Assert.IsType<ArgumentExpression>(functionCall.Arguments[1]);

        Assert.False(validator.HasErrors);
        expression.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(
            validator.Diagnostics,
            d => d.Message.EndsWith(FunctionCallExpression.CommaExpected));
    }
}