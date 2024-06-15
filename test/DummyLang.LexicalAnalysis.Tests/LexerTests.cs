using Xunit;

namespace DummyLang.LexicalAnalysis.Tests;

public class LexerTests
{
    [Fact]
    public void Next_Nothing_ShouldReturnCorrectTokens()
    {
        // Arrange
        const string source = "        \t    \n";

        // Act
        var lexer = new Lexer(source);

        // Assert
        Assert.Equal(TokenType.Eof, lexer.ReadNext().Type);
    }
    
    [Fact]
    public void Next_SimpleInput_ShouldReturnCorrectTokens()
    {
        // Arrange
        const string source = "=+-*/<>(){}[]";

        // Act
        var lexer = new Lexer(source);

        // Assert
        Assert.Equal(TokenType.Assign, lexer.ReadNext().Type);
        Assert.Equal(TokenType.Plus, lexer.ReadNext().Type);
        Assert.Equal(TokenType.Minus, lexer.ReadNext().Type);
        Assert.Equal(TokenType.Star, lexer.ReadNext().Type);
        Assert.Equal(TokenType.Slash, lexer.ReadNext().Type);
        Assert.Equal(TokenType.LessThan, lexer.ReadNext().Type);
        Assert.Equal(TokenType.GreaterThan, lexer.ReadNext().Type);

        Assert.Equal(TokenType.LeftParen, lexer.ReadNext().Type);
        Assert.Equal(TokenType.RightParen, lexer.ReadNext().Type);
        Assert.Equal(TokenType.LeftBrace, lexer.ReadNext().Type);
        Assert.Equal(TokenType.RightBrace, lexer.ReadNext().Type);
        Assert.Equal(TokenType.LeftBracket, lexer.ReadNext().Type);
        Assert.Equal(TokenType.RightBracket, lexer.ReadNext().Type);

        Assert.Equal(TokenType.Eof, lexer.ReadNext().Type);
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
        var lexer = new Lexer(source);

        // Assert
        Assert.Equal(TokenType.Var, lexer.ReadNext().Type);
        Assert.Equal(TokenType.Identifier, lexer.ReadNext().Type);
        Assert.Equal(TokenType.Assign, lexer.ReadNext().Type);
        Assert.Equal(TokenType.Number, lexer.ReadNext().Type);
        Assert.Equal(TokenType.Semicolon, lexer.ReadNext().Type);
        
        Assert.Equal(TokenType.Var, lexer.ReadNext().Type);
        Assert.Equal(TokenType.Identifier, lexer.ReadNext().Type);
        Assert.Equal(TokenType.Colon, lexer.ReadNext().Type);
        Assert.Equal(TokenType.Identifier, lexer.ReadNext().Type);
        Assert.Equal(TokenType.Assign, lexer.ReadNext().Type);
        Assert.Equal(TokenType.Number, lexer.ReadNext().Type);
        Assert.Equal(TokenType.Semicolon, lexer.ReadNext().Type);
        
        Assert.Equal(TokenType.Const, lexer.ReadNext().Type);
        Assert.Equal(TokenType.Identifier, lexer.ReadNext().Type);
        Assert.Equal(TokenType.Assign, lexer.ReadNext().Type);
        Assert.Equal(TokenType.Fun, lexer.ReadNext().Type);
        Assert.Equal(TokenType.LeftParen, lexer.ReadNext().Type);
        Assert.Equal(TokenType.Identifier, lexer.ReadNext().Type);
        Assert.Equal(TokenType.Colon, lexer.ReadNext().Type);
        Assert.Equal(TokenType.Identifier, lexer.ReadNext().Type);
        Assert.Equal(TokenType.Comma, lexer.ReadNext().Type);
        Assert.Equal(TokenType.Identifier, lexer.ReadNext().Type);
        Assert.Equal(TokenType.Colon, lexer.ReadNext().Type);
        Assert.Equal(TokenType.Identifier, lexer.ReadNext().Type);
        Assert.Equal(TokenType.RightParen, lexer.ReadNext().Type);
        Assert.Equal(TokenType.Colon, lexer.ReadNext().Type);
        Assert.Equal(TokenType.Identifier, lexer.ReadNext().Type);
        Assert.Equal(TokenType.LeftBrace, lexer.ReadNext().Type);
        Assert.Equal(TokenType.Return, lexer.ReadNext().Type);
        Assert.Equal(TokenType.Identifier, lexer.ReadNext().Type);
        Assert.Equal(TokenType.Plus, lexer.ReadNext().Type);
        Assert.Equal(TokenType.Identifier, lexer.ReadNext().Type);
        Assert.Equal(TokenType.Semicolon, lexer.ReadNext().Type);
        Assert.Equal(TokenType.RightBrace, lexer.ReadNext().Type);
        Assert.Equal(TokenType.Semicolon, lexer.ReadNext().Type);
        
        Assert.Equal(TokenType.Var, lexer.ReadNext().Type);
        Assert.Equal(TokenType.Identifier, lexer.ReadNext().Type);
        Assert.Equal(TokenType.Assign, lexer.ReadNext().Type);
        Assert.Equal(TokenType.Identifier, lexer.ReadNext().Type);
        Assert.Equal(TokenType.LeftParen, lexer.ReadNext().Type);
        Assert.Equal(TokenType.Identifier, lexer.ReadNext().Type);
        Assert.Equal(TokenType.Comma, lexer.ReadNext().Type);
        Assert.Equal(TokenType.Identifier, lexer.ReadNext().Type);
        Assert.Equal(TokenType.RightParen, lexer.ReadNext().Type);
        Assert.Equal(TokenType.Semicolon, lexer.ReadNext().Type);
        
        Assert.Equal(TokenType.Eof, lexer.ReadNext().Type);
    }
    
    [Fact]
    public void Next_FloatingPointNumbers_ShouldReturnCorrectTokens()
    {
        // Arrange
        const string validSource = "const correct = 0.1234;";
        const string invalidSource = "const wrong = 0.123.45.678;";

        // Act
        var validLexer = new Lexer(validSource);
        var invalidLexer = new Lexer(invalidSource);

        // Assert
        Assert.Equal(TokenType.Const, validLexer.ReadNext().Type);
        Assert.Equal(TokenType.Identifier, validLexer.ReadNext().Type);
        Assert.Equal(TokenType.Assign, validLexer.ReadNext().Type);
        Assert.Equal(TokenType.Number, validLexer.ReadNext().Type);
        Assert.Equal(TokenType.Semicolon, validLexer.ReadNext().Type);

        Assert.Equal(TokenType.Eof, validLexer.ReadNext().Type);
        
        Assert.Equal(TokenType.Const, invalidLexer.ReadNext().Type);
        Assert.Equal(TokenType.Identifier, invalidLexer.ReadNext().Type);
        Assert.Equal(TokenType.Assign, invalidLexer.ReadNext().Type);
        Assert.Equal(TokenType.Invalid, invalidLexer.ReadNext().Type);
    }
}