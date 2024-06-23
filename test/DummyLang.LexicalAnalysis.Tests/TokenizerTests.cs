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
        var token = tokenizer.ReadNext();

        // Assert
        Assert.Equal(TokenType.Eof, token.Type);
        Assert.Equal("", token.Value);
        Assert.True(token.Position is { Line: 2, Column: 1, Index: 0, Width: 14 });
        
        Assert.Equal(token.Position.Index + token.Position.Width, source.Length);
    }

    [Fact]
    public void Next_SimpleInput_ShouldReturnCorrectTokens()
    {
        // Arrange
        const string source = "=+-*/<>(){}[]";

        // Act
        var tokenizer = new Tokenizer();
        tokenizer.Use(source);
        var token = tokenizer.ReadNext();

        // Assert
        Assert.Equal(TokenType.Assign, token.Type);
        Assert.Equal("=", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 1, Index: 0, Width: 1 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Plus, token.Type);
        Assert.Equal("+", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 2, Index: 1, Width: 1 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Minus, token.Type);
        Assert.Equal("-", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 3, Index: 2, Width: 1 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Star, token.Type);
        Assert.Equal("*", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 4, Index: 3, Width: 1 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Slash, token.Type);
        Assert.Equal("/", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 5, Index: 4, Width: 1 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.LessThan, token.Type);
        Assert.Equal("<", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 6, Index: 5, Width: 1 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.GreaterThan, token.Type);
        Assert.Equal(">", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 7, Index: 6, Width: 1 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.LeftParen, token.Type);
        Assert.Equal("(", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 8, Index: 7, Width: 1 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.RightParen, token.Type);
        Assert.Equal(")", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 9, Index: 8, Width: 1 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.LeftBrace, token.Type);
        Assert.Equal("{", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 10, Index: 9, Width: 1 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.RightBrace, token.Type);
        Assert.Equal("}", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 11, Index: 10, Width: 1 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.LeftBracket, token.Type);
        Assert.Equal("[", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 12, Index: 11, Width: 1 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.RightBracket, token.Type);
        Assert.Equal("]", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 13, Index: 12, Width: 1 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Eof, token.Type);
        Assert.Equal("", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 14, Index: 13, Width: 0 });
        
        Assert.Equal(token.Position.Index + token.Position.Width, source.Length);
    }
    
    [Fact]
    public void Next_MultiCharacterTokenTypes_ShouldReturnCorrectTokens()
    {
        // Arrange
        const string source = "var result = 1 != 2;";

        // Act
        var tokenizer = new Tokenizer();
        tokenizer.Use(source);
        var token = tokenizer.ReadNext();

        // Assert
        Assert.Equal(TokenType.Var, token.Type);
        Assert.Equal("var", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 1, Index: 0, Width: 3 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Identifier, token.Type);
        Assert.Equal("result", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 5, Index: 4, Width: 6 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Assign, token.Type);
        Assert.Equal("=", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 12, Index: 11, Width: 1 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Integer, token.Type);
        Assert.Equal("1", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 14, Index: 13, Width: 1 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.NotEqual, token.Type);
        Assert.Equal("!=", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 16, Index: 15, Width: 2 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Integer, token.Type);
        Assert.Equal("2", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 19, Index: 18, Width: 1 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Semicolon, token.Type);
        Assert.Equal(";", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 20, Index: 19, Width: 1 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Eof, token.Type);
        Assert.Equal("", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 21, Index: 20, Width: 0 });
        
        Assert.Equal(token.Position.Index + token.Position.Width, source.Length);
    }

    [Fact]
    public void Next_Numbers_ShouldReturnCorrectTokens()
    {
        // Arrange
        const string numberInteger = "12345";
        const string numberUnsigned = "12345u";
        const string numberLong = "12345l";
        const string numberUnsignedLong = "12345ul";
        const string numberBinary = "0b010101";
        const string numberHex = "0x0123456789ABCDEF";
        const string numberDouble1 = "123.45";
        const string numberDouble2 = "123.45d";
        const string numberFloat = "123.45f";
        const string numberDecimal = "123.45m";
        const string numberExponent = "123.45e-123f";

        // Act
        var tokenizer = new Tokenizer();
        tokenizer.Use(numberInteger);
        var token = tokenizer.ReadNext();

        // Assert
        Assert.Equal(TokenType.Integer, token.Type);
        Assert.Equal(numberInteger, token.Value);
        Assert.True(token.Position is { Line: 1, Column: 1, Index: 0, Width: 5 });
        
        tokenizer.Use(numberUnsigned);
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Integer, token.Type);
        Assert.Equal(numberUnsigned, token.Value);
        Assert.True(token.Position is { Line: 1, Column: 1, Index: 0, Width: 6 });
        
        tokenizer.Use(numberLong);
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Integer, token.Type);
        Assert.Equal(numberLong, token.Value);
        Assert.True(token.Position is { Line: 1, Column: 1, Index: 0, Width: 6 });
        
        tokenizer.Use(numberUnsignedLong);
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Integer, token.Type);
        Assert.Equal(numberUnsignedLong, token.Value);
        Assert.True(token.Position is { Line: 1, Column: 1, Index: 0, Width: 7 });
        
        tokenizer.Use(numberBinary);
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Integer, token.Type);
        Assert.Equal(numberBinary, token.Value);
        Assert.True(token.Position is { Line: 1, Column: 1, Index: 0, Width: 8 });
        
        tokenizer.Use(numberHex);
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Integer, token.Type);
        Assert.Equal(numberHex, token.Value);
        Assert.True(token.Position is { Line: 1, Column: 1, Index: 0, Width: 18 });
        
        tokenizer.Use(numberDouble1);
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Real, token.Type);
        Assert.Equal(numberDouble1, token.Value);
        Assert.True(token.Position is { Line: 1, Column: 1, Index: 0, Width: 6 });
        
        tokenizer.Use(numberDouble2);
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Real, token.Type);
        Assert.Equal(numberDouble2, token.Value);
        Assert.True(token.Position is { Line: 1, Column: 1, Index: 0, Width: 7 });
        
        tokenizer.Use(numberFloat);
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Real, token.Type);
        Assert.Equal(numberFloat, token.Value);
        Assert.True(token.Position is { Line: 1, Column: 1, Index: 0, Width: 7 });
        
        tokenizer.Use(numberDecimal);
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Real, token.Type);
        Assert.Equal(numberDecimal, token.Value);
        Assert.True(token.Position is { Line: 1, Column: 1, Index: 0, Width: 7 });
        
        tokenizer.Use(numberExponent);
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Real, token.Type);
        Assert.Equal(numberExponent, token.Value);
        Assert.True(token.Position is { Line: 1, Column: 1, Index: 0, Width: 12 });
    }
    
    [Fact]
    public void Next_CharacterInput_ShouldReturnCorrectTokens()
    {
        // Arrange
        const string source = """
                              Print('\'');
                              """;

        // Act
        var tokenizer = new Tokenizer();
        tokenizer.Use(source);
        var token = tokenizer.ReadNext();

        // Assert
        Assert.Equal(TokenType.Identifier, token.Type);
        Assert.Equal("Print", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 1, Index: 0, Width: 5 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.LeftParen, token.Type);
        Assert.Equal("(", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 6, Index: 5, Width: 1 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Character, token.Type);
        Assert.Equal("'\\''", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 7, Index: 6, Width: 4 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.RightParen, token.Type);
        Assert.Equal(")", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 11, Index: 10, Width: 1 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Semicolon, token.Type);
        Assert.Equal(";", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 12, Index: 11, Width: 1 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Eof, token.Type);
        Assert.Equal("", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 13, Index: 12, Width: 0 });
    }
    
    [Fact]
    public void Next_InvalidCharacterInput_ShouldReturnCorrectTokens()
    {
        // Arrange
        const string source = """
                              'abc
                              nextLine
                              """;

        // Act
        var tokenizer = new Tokenizer();
        tokenizer.Use(source);
        var token = tokenizer.ReadNext();

        // Assert
        Assert.Equal(TokenType.Character, token.Type);
        Assert.Equal("'ab", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 1, Index: 0, Width: 3 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Identifier, token.Type);
        Assert.Equal("c", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 4, Index: 3, Width: 1 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Identifier, token.Type);
        Assert.Equal("nextLine", token.Value);
        Assert.True(token.Position is { Line: 2, Column: 1, Index: 6, Width: 8 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Eof, token.Type);
        Assert.Equal("", token.Value);
        Assert.True(token.Position is { Line: 2, Column: 9, Index: 14, Width: 0 });
    }
    
    [Fact]
    public void Next_StringInput_ShouldReturnCorrectTokens()
    {
        // Arrange
        const string source = """
                              Print("abc   abacs \" 142 + 21");
                              """;

        // Act
        var tokenizer = new Tokenizer();
        tokenizer.Use(source);
        var token = tokenizer.ReadNext();

        // Assert
        Assert.Equal(TokenType.Identifier, token.Type);
        Assert.Equal("Print", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 1, Index: 0, Width: 5 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.LeftParen, token.Type);
        Assert.Equal("(", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 6, Index: 5, Width: 1 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.String, token.Type);
        Assert.Equal("\"abc   abacs \\\" 142 + 21\"", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 7, Index: 6, Width: 25 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.RightParen, token.Type);
        Assert.Equal(")", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 32, Index: 31, Width: 1 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Semicolon, token.Type);
        Assert.Equal(";", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 33, Index: 32, Width: 1 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Eof, token.Type);
        Assert.Equal("", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 34, Index: 33, Width: 0 });
    }
    
    [Fact]
    public void Next_InvalidStringInput_ShouldReturnCorrectTokens()
    {
        // Arrange
        const string source = """
                              "abc   abacs \" 142 + ab
                              nextLine
                              """;

        // Act
        var tokenizer = new Tokenizer();
        tokenizer.Use(source);
        var token = tokenizer.ReadNext();

        // Assert
        Assert.Equal(TokenType.String, token.Type);
        Assert.Equal("\"abc   abacs \\\" 142 + ab", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 1, Index: 0, Width: 24 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Identifier, token.Type);
        Assert.Equal("nextLine", token.Value);
        Assert.True(token.Position is { Line: 2, Column: 1, Index: 26, Width: 8 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Eof, token.Type);
        Assert.Equal("", token.Value);
        Assert.True(token.Position is { Line: 2, Column: 9, Index: 34, Width: 0 });
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
        var token = tokenizer.ReadNext();

        // Assert
        // Line 1
        Assert.Equal(TokenType.Var, token.Type);
        Assert.Equal("var", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 1, Index: 0, Width: 3 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Identifier, token.Type);
        Assert.Equal("five", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 5, Index: 4, Width: 4 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Assign, token.Type);
        Assert.Equal("=", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 10, Index: 9, Width: 1 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Integer, token.Type);
        Assert.Equal("5", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 12, Index: 11, Width: 1 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Semicolon, token.Type);
        Assert.Equal(";", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 13, Index: 12, Width: 1 });
        
        // Line 2
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Var, token.Type);
        Assert.Equal("var", token.Value);
        Assert.True(token.Position is { Line: 2, Column: 1, Index: 15, Width: 3 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Identifier, token.Type);
        Assert.Equal("ten", token.Value);
        Assert.True(token.Position is { Line: 2, Column: 5, Index: 19, Width: 3 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Colon, token.Type);
        Assert.Equal(":", token.Value);
        Assert.True(token.Position is { Line: 2, Column: 8, Index: 22, Width: 1 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Identifier, token.Type);
        Assert.Equal("Number", token.Value);
        Assert.True(token.Position is { Line: 2, Column: 10, Index: 24, Width: 6 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Assign, token.Type);
        Assert.Equal("=", token.Value);
        Assert.True(token.Position is { Line: 2, Column: 17, Index: 31, Width: 1 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Integer, token.Type);
        Assert.Equal("10", token.Value);
        Assert.True(token.Position is { Line: 2, Column: 19, Index: 33, Width: 2 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Semicolon, token.Type);
        Assert.Equal(";", token.Value);
        Assert.True(token.Position is { Line: 2, Column: 21, Index: 35, Width: 1 });

        // Line 3
        // Line 4
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Const, token.Type);
        Assert.Equal("const", token.Value);
        Assert.True(token.Position is { Line: 4, Column: 1, Index: 40, Width: 5 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Identifier, token.Type);
        Assert.Equal("add", token.Value);
        Assert.True(token.Position is { Line: 4, Column: 7, Index: 46, Width: 3 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Assign, token.Type);
        Assert.Equal("=", token.Value);
        Assert.True(token.Position is { Line: 4, Column: 11, Index: 50, Width: 1 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Fun, token.Type);
        Assert.Equal("fun", token.Value);
        Assert.True(token.Position is { Line: 4, Column: 13, Index: 52, Width: 3 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.LeftParen, token.Type);
        Assert.Equal("(", token.Value);
        Assert.True(token.Position is { Line: 4, Column: 16, Index: 55, Width: 1 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Identifier, token.Type);
        Assert.Equal("a", token.Value);
        Assert.True(token.Position is { Line: 4, Column: 17, Index: 56, Width: 1 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Colon, token.Type);
        Assert.Equal(":", token.Value);
        Assert.True(token.Position is { Line: 4, Column: 18, Index: 57, Width: 1 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Identifier, token.Type);
        Assert.Equal("Number", token.Value);
        Assert.True(token.Position is { Line: 4, Column: 20, Index: 59, Width: 6 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Comma, token.Type);
        Assert.Equal(",", token.Value);
        Assert.True(token.Position is { Line: 4, Column: 26, Index: 65, Width: 1 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Identifier, token.Type);
        Assert.Equal("b", token.Value);
        Assert.True(token.Position is { Line: 4, Column: 28, Index: 67, Width: 1 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Colon, token.Type);
        Assert.Equal(":", token.Value);
        Assert.True(token.Position is { Line: 4, Column: 29, Index: 68, Width: 1 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Identifier, token.Type);
        Assert.Equal("Number", token.Value);
        Assert.True(token.Position is { Line: 4, Column: 31, Index: 70, Width: 6 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.RightParen, token.Type);
        Assert.Equal(")", token.Value);
        Assert.True(token.Position is { Line: 4, Column: 37, Index: 76, Width: 1 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Colon, token.Type);
        Assert.Equal(":", token.Value);
        Assert.True(token.Position is { Line: 4, Column: 38, Index: 77, Width: 1 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Identifier, token.Type);
        Assert.Equal("Number", token.Value);
        Assert.True(token.Position is { Line: 4, Column: 40, Index: 79, Width: 6 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.LeftBrace, token.Type);
        Assert.Equal("{", token.Value);
        Assert.True(token.Position is { Line: 4, Column: 47, Index: 86, Width: 1 });
        
        // Line 5
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Return, token.Type);
        Assert.Equal("return", token.Value);
        Assert.True(token.Position is { Line: 5, Column: 5, Index: 93, Width: 6 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Identifier, token.Type);
        Assert.Equal("a", token.Value);
        Assert.True(token.Position is { Line: 5, Column: 12, Index: 100, Width: 1 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Plus, token.Type);
        Assert.Equal("+", token.Value);
        Assert.True(token.Position is { Line: 5, Column: 14, Index: 102, Width: 1 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Identifier, token.Type);
        Assert.Equal("b", token.Value);
        Assert.True(token.Position is { Line: 5, Column: 16, Index: 104, Width: 1 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Semicolon, token.Type);
        Assert.Equal(";", token.Value);
        Assert.True(token.Position is { Line: 5, Column: 17, Index: 105, Width: 1 });
        
        // Line 6
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.RightBrace, token.Type);
        Assert.Equal("}", token.Value);
        Assert.True(token.Position is { Line: 6, Column: 1, Index: 108, Width: 1 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Semicolon, token.Type);
        Assert.Equal(";", token.Value);
        Assert.True(token.Position is { Line: 6, Column: 2, Index: 109, Width: 1 });
        
        // Line 7
        // Line 8
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Var, token.Type);
        Assert.Equal("var", token.Value);
        Assert.True(token.Position is { Line: 8, Column: 1, Index: 114, Width: 3 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Identifier, token.Type);
        Assert.Equal("result", token.Value);
        Assert.True(token.Position is { Line: 8, Column: 5, Index: 118, Width: 6 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Assign, token.Type);
        Assert.Equal("=", token.Value);
        Assert.True(token.Position is { Line: 8, Column: 12, Index: 125, Width: 1 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Identifier, token.Type);
        Assert.Equal("add", token.Value);
        Assert.True(token.Position is { Line: 8, Column: 14, Index: 127, Width: 3 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.LeftParen, token.Type);
        Assert.Equal("(", token.Value);
        Assert.True(token.Position is { Line: 8, Column: 17, Index: 130, Width: 1 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Identifier, token.Type);
        Assert.Equal("five", token.Value);
        Assert.True(token.Position is { Line: 8, Column: 18, Index: 131, Width: 4 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Comma, token.Type);
        Assert.Equal(",", token.Value);
        Assert.True(token.Position is { Line: 8, Column: 22, Index: 135, Width: 1 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Identifier, token.Type);
        Assert.Equal("ten", token.Value);
        Assert.True(token.Position is { Line: 8, Column: 24, Index: 137, Width: 3 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.RightParen, token.Type);
        Assert.Equal(")", token.Value);
        Assert.True(token.Position is { Line: 8, Column: 27, Index: 140, Width: 1 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Semicolon, token.Type);
        Assert.Equal(";", token.Value);
        Assert.True(token.Position is { Line: 8, Column: 28, Index: 141, Width: 1 });
        
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Eof, token.Type);
        Assert.Equal("", token.Value);
        Assert.True(token.Position is { Line: 8, Column: 29, Index: 142, Width: 0 });
        
        Assert.Equal(token.Position.Index + token.Position.Width, source.Length);
    }
}