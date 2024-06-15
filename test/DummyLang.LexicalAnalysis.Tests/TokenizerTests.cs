using Xunit;

namespace DummyLang.LexicalAnalysis.Tests;

public class TokenizerTests
{
    [Fact]
    public void Next_Nothing_ShouldReturnCorrectTokens()
    {
        // Arrange
        const string source = "        \t    \n";

        // Act
        var tokenizer = new Tokenizer();
        tokenizer.Use(source);

        // Assert
        Assert.True(tokenizer.ReadNext() is { Type: TokenType.Eof, Value: "", Position: { Start: 0, End: 14 } });
    }

    [Fact]
    public void Next_SimpleInput_ShouldReturnCorrectTokens()
    {
        // Arrange
        const string source = "=+-*/<>(){}[]";

        // Act
        var tokenizer = new Tokenizer();
        tokenizer.Use(source);

        // Assert
        Assert.True(tokenizer.ReadNext() is { Type: TokenType.Assign, Position: { Start: 0, End: 0 } });
        Assert.True(tokenizer.ReadNext() is { Type: TokenType.Plus, Position: { Start: 1, End: 1 } });
        Assert.True(tokenizer.ReadNext() is { Type: TokenType.Minus, Position: { Start: 2, End: 2 } });
        Assert.True(tokenizer.ReadNext() is { Type: TokenType.Star, Position: { Start: 3, End: 3 } });
        Assert.True(tokenizer.ReadNext() is { Type: TokenType.Slash, Position: { Start: 4, End: 4 } });
        Assert.True(tokenizer.ReadNext() is { Type: TokenType.LessThan, Position: { Start: 5, End: 5 } });
        Assert.True(tokenizer.ReadNext() is { Type: TokenType.GreaterThan, Position: { Start: 6, End: 6 } });

        Assert.True(tokenizer.ReadNext() is { Type: TokenType.LeftParen, Position: { Start: 7, End: 7 } });
        Assert.True(tokenizer.ReadNext() is { Type: TokenType.RightParen, Position: { Start: 8, End: 8 } });
        Assert.True(tokenizer.ReadNext() is { Type: TokenType.LeftBrace, Position: { Start: 9, End: 9 } });
        Assert.True(tokenizer.ReadNext() is { Type: TokenType.RightBrace, Position: { Start: 10, End: 10 } });
        Assert.True(tokenizer.ReadNext() is { Type: TokenType.LeftBracket, Position: { Start: 11, End: 11 } });
        Assert.True(tokenizer.ReadNext() is { Type: TokenType.RightBracket, Position: { Start: 12, End: 12 } });

        Assert.True(tokenizer.ReadNext() is { Type: TokenType.Eof, Position: { Start: 13, End: 13 } });
    }

    [Fact]
    public void Next_ComplexInput_ShouldReturnCorrectTokens()
    {
        // Arrange
        const string source = """
                              var five = 5;
                              var ten: Number = 10;

                              const add = fun(a: Number, b: Number): Number {
                                  return a + b;
                              };

                              var result = add(five, ten);
                              """;

        // Act
        var tokenizer = new Tokenizer();
        tokenizer.Use(source);

        // Assert
        Assert.Equal(TokenType.Var, tokenizer.ReadNext().Type);
        Assert.Equal(TokenType.Identifier, tokenizer.ReadNext().Type);
        Assert.Equal(TokenType.Assign, tokenizer.ReadNext().Type);
        Assert.Equal(TokenType.Number, tokenizer.ReadNext().Type);
        Assert.Equal(TokenType.Semicolon, tokenizer.ReadNext().Type);

        Assert.Equal(TokenType.Var, tokenizer.ReadNext().Type);
        Assert.Equal(TokenType.Identifier, tokenizer.ReadNext().Type);
        Assert.Equal(TokenType.Colon, tokenizer.ReadNext().Type);
        Assert.Equal(TokenType.Identifier, tokenizer.ReadNext().Type);
        Assert.Equal(TokenType.Assign, tokenizer.ReadNext().Type);
        Assert.Equal(TokenType.Number, tokenizer.ReadNext().Type);
        Assert.Equal(TokenType.Semicolon, tokenizer.ReadNext().Type);

        Assert.Equal(TokenType.Const, tokenizer.ReadNext().Type);
        Assert.Equal(TokenType.Identifier, tokenizer.ReadNext().Type);
        Assert.Equal(TokenType.Assign, tokenizer.ReadNext().Type);
        Assert.Equal(TokenType.Fun, tokenizer.ReadNext().Type);
        Assert.Equal(TokenType.LeftParen, tokenizer.ReadNext().Type);
        Assert.Equal(TokenType.Identifier, tokenizer.ReadNext().Type);
        Assert.Equal(TokenType.Colon, tokenizer.ReadNext().Type);
        Assert.Equal(TokenType.Identifier, tokenizer.ReadNext().Type);
        Assert.Equal(TokenType.Comma, tokenizer.ReadNext().Type);
        Assert.Equal(TokenType.Identifier, tokenizer.ReadNext().Type);
        Assert.Equal(TokenType.Colon, tokenizer.ReadNext().Type);
        Assert.Equal(TokenType.Identifier, tokenizer.ReadNext().Type);
        Assert.Equal(TokenType.RightParen, tokenizer.ReadNext().Type);
        Assert.Equal(TokenType.Colon, tokenizer.ReadNext().Type);
        Assert.Equal(TokenType.Identifier, tokenizer.ReadNext().Type);
        Assert.Equal(TokenType.LeftBrace, tokenizer.ReadNext().Type);
        Assert.Equal(TokenType.Return, tokenizer.ReadNext().Type);
        Assert.Equal(TokenType.Identifier, tokenizer.ReadNext().Type);
        Assert.Equal(TokenType.Plus, tokenizer.ReadNext().Type);
        Assert.Equal(TokenType.Identifier, tokenizer.ReadNext().Type);
        Assert.Equal(TokenType.Semicolon, tokenizer.ReadNext().Type);
        Assert.Equal(TokenType.RightBrace, tokenizer.ReadNext().Type);
        Assert.Equal(TokenType.Semicolon, tokenizer.ReadNext().Type);

        Assert.Equal(TokenType.Var, tokenizer.ReadNext().Type);
        Assert.Equal(TokenType.Identifier, tokenizer.ReadNext().Type);
        Assert.Equal(TokenType.Assign, tokenizer.ReadNext().Type);
        Assert.Equal(TokenType.Identifier, tokenizer.ReadNext().Type);
        Assert.Equal(TokenType.LeftParen, tokenizer.ReadNext().Type);
        Assert.Equal(TokenType.Identifier, tokenizer.ReadNext().Type);
        Assert.Equal(TokenType.Comma, tokenizer.ReadNext().Type);
        Assert.Equal(TokenType.Identifier, tokenizer.ReadNext().Type);
        Assert.Equal(TokenType.RightParen, tokenizer.ReadNext().Type);
        Assert.Equal(TokenType.Semicolon, tokenizer.ReadNext().Type);

        Assert.Equal(TokenType.Eof, tokenizer.ReadNext().Type);
    }

    [Fact]
    public void Next_FloatingPointNumbers_ShouldReturnCorrectTokens()
    {
        // Arrange
        const string source = "const correct = 0.1234;";

        // Act
        var tokenizer = new Tokenizer();
        tokenizer.Use(source);

        // Assert
        Assert.True(tokenizer.ReadNext() is { Type: TokenType.Const, Position: { Start: 0, End: 4 } });
        Assert.True(tokenizer.ReadNext() is { Type: TokenType.Identifier, Position: { Start: 6, End: 12 } });
        Assert.True(tokenizer.ReadNext() is { Type: TokenType.Assign, Position: { Start: 14, End: 14 } });
        Assert.True(tokenizer.ReadNext() is { Type: TokenType.Number, Position: { Start: 16, End: 16 } });
        Assert.True(tokenizer.ReadNext() is { Type: TokenType.Dot, Position: { Start: 17, End: 17 } });
        Assert.True(tokenizer.ReadNext() is { Type: TokenType.Number, Position: { Start: 18, End: 21 } });
        Assert.True(tokenizer.ReadNext() is { Type: TokenType.Semicolon, Position: { Start: 22, End: 22 } });

        Assert.True(tokenizer.ReadNext() is { Type: TokenType.Eof, Position: { Start: 23, End: 23 } });
    }
}