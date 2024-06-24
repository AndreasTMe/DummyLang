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
        Assert.True(token.Position is { Line: 2, Column: 1 });
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
        Assert.True(token.Position is { Line: 1, Column: 1 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Plus, token.Type);
        Assert.Equal("+", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 2 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Minus, token.Type);
        Assert.Equal("-", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 3 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Star, token.Type);
        Assert.Equal("*", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 4 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Slash, token.Type);
        Assert.Equal("/", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 5 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.LessThan, token.Type);
        Assert.Equal("<", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 6 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.GreaterThan, token.Type);
        Assert.Equal(">", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 7 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.LeftParenthesis, token.Type);
        Assert.Equal("(", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 8 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.RightParenthesis, token.Type);
        Assert.Equal(")", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 9 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.LeftBrace, token.Type);
        Assert.Equal("{", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 10 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.RightBrace, token.Type);
        Assert.Equal("}", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 11 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.LeftBracket, token.Type);
        Assert.Equal("[", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 12 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.RightBracket, token.Type);
        Assert.Equal("]", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 13 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Eof, token.Type);
        Assert.Equal("", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 14 });
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
        Assert.True(token.Position is { Line: 1, Column: 1 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Identifier, token.Type);
        Assert.Equal("result", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 5 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Assign, token.Type);
        Assert.Equal("=", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 12 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Integer, token.Type);
        Assert.Equal("1", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 14 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.NotEqual, token.Type);
        Assert.Equal("!=", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 16 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Integer, token.Type);
        Assert.Equal("2", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 19 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Semicolon, token.Type);
        Assert.Equal(";", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 20 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Eof, token.Type);
        Assert.Equal("", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 21 });
    }

    [Fact]
    public void Next_Numbers_ShouldReturnCorrectTokens()
    {
        // Arrange
        const string numberInteger      = "12345";
        const string numberUnsigned     = "12345u";
        const string numberLong         = "12345l";
        const string numberUnsignedLong = "12345ul";
        const string numberBinary       = "0b010101";
        const string numberHex          = "0x0123456789ABCDEF";
        const string numberDouble1      = "123.45";
        const string numberDouble2      = "123.45d";
        const string numberFloat        = "123.45f";
        const string numberDecimal      = "123.45m";
        const string numberExponent     = "123.45e-123f";

        // Act
        var tokenizer = new Tokenizer();
        tokenizer.Use(numberInteger);
        var token = tokenizer.ReadNext();

        // Assert
        Assert.Equal(TokenType.Integer, token.Type);
        Assert.Equal(numberInteger, token.Value);
        Assert.True(token.Position is { Line: 1, Column: 1 });

        tokenizer.Use(numberUnsigned);
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Integer, token.Type);
        Assert.Equal(numberUnsigned, token.Value);
        Assert.True(token.Position is { Line: 1, Column: 1 });

        tokenizer.Use(numberLong);
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Integer, token.Type);
        Assert.Equal(numberLong, token.Value);
        Assert.True(token.Position is { Line: 1, Column: 1 });

        tokenizer.Use(numberUnsignedLong);
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Integer, token.Type);
        Assert.Equal(numberUnsignedLong, token.Value);
        Assert.True(token.Position is { Line: 1, Column: 1 });

        tokenizer.Use(numberBinary);
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Integer, token.Type);
        Assert.Equal(numberBinary, token.Value);
        Assert.True(token.Position is { Line: 1, Column: 1 });

        tokenizer.Use(numberHex);
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Integer, token.Type);
        Assert.Equal(numberHex, token.Value);
        Assert.True(token.Position is { Line: 1, Column: 1 });

        tokenizer.Use(numberDouble1);
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Real, token.Type);
        Assert.Equal(numberDouble1, token.Value);
        Assert.True(token.Position is { Line: 1, Column: 1 });

        tokenizer.Use(numberDouble2);
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Real, token.Type);
        Assert.Equal(numberDouble2, token.Value);
        Assert.True(token.Position is { Line: 1, Column: 1 });

        tokenizer.Use(numberFloat);
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Real, token.Type);
        Assert.Equal(numberFloat, token.Value);
        Assert.True(token.Position is { Line: 1, Column: 1 });

        tokenizer.Use(numberDecimal);
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Real, token.Type);
        Assert.Equal(numberDecimal, token.Value);
        Assert.True(token.Position is { Line: 1, Column: 1 });

        tokenizer.Use(numberExponent);
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Real, token.Type);
        Assert.Equal(numberExponent, token.Value);
        Assert.True(token.Position is { Line: 1, Column: 1 });
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
        Assert.True(token.Position is { Line: 1, Column: 1 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.LeftParenthesis, token.Type);
        Assert.Equal("(", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 6 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Character, token.Type);
        Assert.Equal("'\\''", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 7 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.RightParenthesis, token.Type);
        Assert.Equal(")", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 11 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Semicolon, token.Type);
        Assert.Equal(";", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 12 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Eof, token.Type);
        Assert.Equal("", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 13 });
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
        Assert.Equal("'a", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 1 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Identifier, token.Type);
        Assert.Equal("bc", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 3 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Identifier, token.Type);
        Assert.Equal("nextLine", token.Value);
        Assert.True(token.Position is { Line: 2, Column: 1 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Eof, token.Type);
        Assert.Equal("", token.Value);
        Assert.True(token.Position is { Line: 2, Column: 9 });
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
        Assert.True(token.Position is { Line: 1, Column: 1 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.LeftParenthesis, token.Type);
        Assert.Equal("(", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 6 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.String, token.Type);
        Assert.Equal("\"abc   abacs \\\" 142 + 21\"", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 7 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.RightParenthesis, token.Type);
        Assert.Equal(")", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 32 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Semicolon, token.Type);
        Assert.Equal(";", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 33 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Eof, token.Type);
        Assert.Equal("", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 34 });
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
        Assert.True(token.Position is { Line: 1, Column: 1 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Identifier, token.Type);
        Assert.Equal("nextLine", token.Value);
        Assert.True(token.Position is { Line: 2, Column: 1 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Eof, token.Type);
        Assert.Equal("", token.Value);
        Assert.True(token.Position is { Line: 2, Column: 9 });
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
        Assert.True(token.Position is { Line: 1, Column: 1 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Identifier, token.Type);
        Assert.Equal("five", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 5 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Assign, token.Type);
        Assert.Equal("=", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 10 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Integer, token.Type);
        Assert.Equal("5", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 12 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Semicolon, token.Type);
        Assert.Equal(";", token.Value);
        Assert.True(token.Position is { Line: 1, Column: 13 });

        // Line 2
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Var, token.Type);
        Assert.Equal("var", token.Value);
        Assert.True(token.Position is { Line: 2, Column: 1 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Identifier, token.Type);
        Assert.Equal("ten", token.Value);
        Assert.True(token.Position is { Line: 2, Column: 5 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Colon, token.Type);
        Assert.Equal(":", token.Value);
        Assert.True(token.Position is { Line: 2, Column: 8 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Identifier, token.Type);
        Assert.Equal("Number", token.Value);
        Assert.True(token.Position is { Line: 2, Column: 10 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Assign, token.Type);
        Assert.Equal("=", token.Value);
        Assert.True(token.Position is { Line: 2, Column: 17 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Integer, token.Type);
        Assert.Equal("10", token.Value);
        Assert.True(token.Position is { Line: 2, Column: 19 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Semicolon, token.Type);
        Assert.Equal(";", token.Value);
        Assert.True(token.Position is { Line: 2, Column: 21 });

        // Line 3
        // Line 4
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Const, token.Type);
        Assert.Equal("const", token.Value);
        Assert.True(token.Position is { Line: 4, Column: 1 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Identifier, token.Type);
        Assert.Equal("add", token.Value);
        Assert.True(token.Position is { Line: 4, Column: 7 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Assign, token.Type);
        Assert.Equal("=", token.Value);
        Assert.True(token.Position is { Line: 4, Column: 11 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Fun, token.Type);
        Assert.Equal("fun", token.Value);
        Assert.True(token.Position is { Line: 4, Column: 13 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.LeftParenthesis, token.Type);
        Assert.Equal("(", token.Value);
        Assert.True(token.Position is { Line: 4, Column: 16 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Identifier, token.Type);
        Assert.Equal("a", token.Value);
        Assert.True(token.Position is { Line: 4, Column: 17 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Colon, token.Type);
        Assert.Equal(":", token.Value);
        Assert.True(token.Position is { Line: 4, Column: 18 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Identifier, token.Type);
        Assert.Equal("Number", token.Value);
        Assert.True(token.Position is { Line: 4, Column: 20 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Comma, token.Type);
        Assert.Equal(",", token.Value);
        Assert.True(token.Position is { Line: 4, Column: 26 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Identifier, token.Type);
        Assert.Equal("b", token.Value);
        Assert.True(token.Position is { Line: 4, Column: 28 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Colon, token.Type);
        Assert.Equal(":", token.Value);
        Assert.True(token.Position is { Line: 4, Column: 29 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Identifier, token.Type);
        Assert.Equal("Number", token.Value);
        Assert.True(token.Position is { Line: 4, Column: 31 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.RightParenthesis, token.Type);
        Assert.Equal(")", token.Value);
        Assert.True(token.Position is { Line: 4, Column: 37 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Colon, token.Type);
        Assert.Equal(":", token.Value);
        Assert.True(token.Position is { Line: 4, Column: 38 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Identifier, token.Type);
        Assert.Equal("Number", token.Value);
        Assert.True(token.Position is { Line: 4, Column: 40 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.LeftBrace, token.Type);
        Assert.Equal("{", token.Value);
        Assert.True(token.Position is { Line: 4, Column: 47 });

        // Line 5
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Return, token.Type);
        Assert.Equal("return", token.Value);
        Assert.True(token.Position is { Line: 5, Column: 5 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Identifier, token.Type);
        Assert.Equal("a", token.Value);
        Assert.True(token.Position is { Line: 5, Column: 12 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Plus, token.Type);
        Assert.Equal("+", token.Value);
        Assert.True(token.Position is { Line: 5, Column: 14 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Identifier, token.Type);
        Assert.Equal("b", token.Value);
        Assert.True(token.Position is { Line: 5, Column: 16 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Semicolon, token.Type);
        Assert.Equal(";", token.Value);
        Assert.True(token.Position is { Line: 5, Column: 17 });

        // Line 6
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.RightBrace, token.Type);
        Assert.Equal("}", token.Value);
        Assert.True(token.Position is { Line: 6, Column: 1 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Semicolon, token.Type);
        Assert.Equal(";", token.Value);
        Assert.True(token.Position is { Line: 6, Column: 2 });

        // Line 7
        // Line 8
        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Var, token.Type);
        Assert.Equal("var", token.Value);
        Assert.True(token.Position is { Line: 8, Column: 1 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Identifier, token.Type);
        Assert.Equal("result", token.Value);
        Assert.True(token.Position is { Line: 8, Column: 5 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Assign, token.Type);
        Assert.Equal("=", token.Value);
        Assert.True(token.Position is { Line: 8, Column: 12 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Identifier, token.Type);
        Assert.Equal("add", token.Value);
        Assert.True(token.Position is { Line: 8, Column: 14 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.LeftParenthesis, token.Type);
        Assert.Equal("(", token.Value);
        Assert.True(token.Position is { Line: 8, Column: 17 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Identifier, token.Type);
        Assert.Equal("five", token.Value);
        Assert.True(token.Position is { Line: 8, Column: 18 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Comma, token.Type);
        Assert.Equal(",", token.Value);
        Assert.True(token.Position is { Line: 8, Column: 22 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Identifier, token.Type);
        Assert.Equal("ten", token.Value);
        Assert.True(token.Position is { Line: 8, Column: 24 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.RightParenthesis, token.Type);
        Assert.Equal(")", token.Value);
        Assert.True(token.Position is { Line: 8, Column: 27 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Semicolon, token.Type);
        Assert.Equal(";", token.Value);
        Assert.True(token.Position is { Line: 8, Column: 28 });

        token = tokenizer.ReadNext();
        Assert.Equal(TokenType.Eof, token.Type);
        Assert.Equal("", token.Value);
        Assert.True(token.Position is { Line: 8, Column: 29 });
    }
}