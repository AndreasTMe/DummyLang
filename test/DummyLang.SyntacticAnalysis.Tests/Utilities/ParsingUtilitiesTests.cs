using DummyLang.LexicalAnalysis;
using DummyLang.LexicalAnalysis.Extensions;
using DummyLang.SyntacticAnalysis.Utilities;
using System.Collections.Generic;
using Xunit;

namespace DummyLang.SyntacticAnalysis.Tests.Utilities;

public class ParsingUtilitiesTests
{
    [Fact]
    public void TryGetBalancedBrackets_FunctionCall_StopAt1stClosingParenthesis()
    {
        // Arrange
        const string source = "test(1,2);";

        // Act
        var tokens = GetTokens(source);

        // Assert
        Assert.True(ParsingUtilities.TryGetBalancedBrackets(tokens, 1, out var endIndex));
        Assert.Equal(5, endIndex);
        Assert.Equal(TokenType.RightParenthesis, tokens[endIndex].Type);
    }

    [Fact]
    public void TryGetBalancedBrackets_ComplexFunctionCall_StopAt3rdClosingParenthesis()
    {
        // Arrange
        const string source = "test((1+2), a => DoSomething());";

        // Act
        var tokens = GetTokens(source);

        // Assert
        Assert.True(ParsingUtilities.TryGetBalancedBrackets(tokens, 1, out var endIndex));
        Assert.Equal(13, endIndex);
        Assert.Equal(TokenType.RightParenthesis, tokens[endIndex].Type);
    }
    
    [Fact]
    public void TryGetBalancedBrackets_InvalidFunctionCall_StopAtSemicolon()
    {
        // Arrange
        const string source = """
                              test((1+2);
                              var t = 0;
                              """;

        // Act
        var tokens = GetTokens(source);

        // Assert
        Assert.False(ParsingUtilities.TryGetBalancedBrackets(tokens, 1, out var endIndex));
        Assert.Equal(7, endIndex);
        Assert.Equal(TokenType.Semicolon, tokens[endIndex].Type);
    }
    
    [Fact]
    public void TryGetBalancedBrackets_InvalidFunctionCall_StopAtNewLine()
    {
        // Arrange
        const string source = """
                              test(
                              var t = 0;
                              """;

        // Act
        var tokens = GetTokens(source);

        // Assert
        Assert.False(ParsingUtilities.TryGetBalancedBrackets(tokens, 1, out var endIndex));
        Assert.Equal(2, endIndex);
        Assert.Equal(TokenType.Var, tokens[endIndex].Type);
    }
    
    [Fact]
    public void TryGetBalancedBrackets_InvalidFunctionCall_StopAtClosingParenthesis()
    {
        // Arrange
        const string source = "test((1+2), a => {);";

        // Act
        var tokens = GetTokens(source);

        // Assert
        Assert.False(ParsingUtilities.TryGetBalancedBrackets(tokens, 1, out var endIndex));
        Assert.Equal(11, endIndex);
        Assert.Equal(TokenType.RightParenthesis, tokens[endIndex].Type);
    }

    private static Token[] GetTokens(string source)
    {
        var tokenizer = new Tokenizer();
        tokenizer.Use(source);

        var tokens = new List<Token>();
        var token  = tokenizer.ReadNext();

        while (!token.IsEndOfFile())
        {
            tokens.Add(token);
            token = tokenizer.ReadNext();
        }

        tokens.Add(token);

        return tokens.ToArray();
    }
}