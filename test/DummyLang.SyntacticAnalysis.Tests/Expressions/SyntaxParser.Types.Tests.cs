using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Expressions;
using DummyLang.SyntacticAnalysis.Parsers;
using DummyLang.SyntacticAnalysis.Utilities;
using Xunit;

namespace DummyLang.SyntacticAnalysis.Tests.Expressions;

public class TypeSyntaxParserTests
{
    [Fact]
    public void ParseExpression_Type_ReadSuccessfully()
    {
        // Arrange
        // Act
        var tokens     = ParsingUtilities.ReadAllTokens("SomeType");
        var index      = 0;
        var expression = ExpressionParser.ParseType(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(2, tokens.Length);
        Assert.Equal(1, index);
        Assert.IsType<TypeIdentifierExpression>(expression);
    }

    [Fact]
    public void ParseExpression_TypeUnion_ReadSuccessfully()
    {
        // Arrange
        // Act
        var tokens     = ParsingUtilities.ReadAllTokens("SomeType | SomeOtherType");
        var index      = 0;
        var expression = ExpressionParser.ParseType(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(4, tokens.Length);
        Assert.Equal(3, index);
        Assert.IsType<TypeBinaryExpression>(expression);
    }

    [Fact]
    public void ParseExpression_TypeIntersection_ReadSuccessfully()
    {
        // Arrange
        // Act
        var tokens     = ParsingUtilities.ReadAllTokens("SomeType & SomeOtherType");
        var index      = 0;
        var expression = ExpressionParser.ParseType(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(4, tokens.Length);
        Assert.Equal(3, index);
        Assert.IsType<TypeBinaryExpression>(expression);
    }

    [Fact]
    public void ParseExpression_TypeExclusion_ReadSuccessfully()
    {
        // Arrange
        // Act
        var tokens     = ParsingUtilities.ReadAllTokens("SomeType ^ SomeOtherType");
        var index      = 0;
        var expression = ExpressionParser.ParseType(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(4, tokens.Length);
        Assert.Equal(3, index);
        Assert.IsType<TypeBinaryExpression>(expression);
    }

    [Fact]
    public void ParseExpression_TypeGeneric_ReadSuccessfully()
    {
        // Arrange
        // Act
        var tokens     = ParsingUtilities.ReadAllTokens("SomeType<Type1, Type2>");
        var index      = 0;
        var expression = ExpressionParser.ParseType(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(7, tokens.Length);
        Assert.Equal(6, index);
        Assert.IsType<TypeGenericExpression>(expression);
        var typeGenericExpression = (TypeGenericExpression)expression;

        Assert.Equal(TokenType.Identifier, typeGenericExpression.Identifier.Type);
        Assert.Equal("SomeType", typeGenericExpression.Identifier.Value);
        Assert.Equal(TokenType.LessThan, typeGenericExpression.LessThan.Type);
        Assert.Equal("<", typeGenericExpression.LessThan.Value);
        Assert.Equal(TokenType.GreaterThan, typeGenericExpression.GreaterThan.Type);
        Assert.Equal(">", typeGenericExpression.GreaterThan.Value);
        Assert.NotNull(typeGenericExpression.TypeParameters);
        Assert.Equal(2, typeGenericExpression.TypeParameters.Count);
        Assert.IsType<TypeIdentifierExpression>(typeGenericExpression.TypeParameters[0]);
        Assert.IsType<TypeIdentifierExpression>(typeGenericExpression.TypeParameters[1]);
        Assert.Single(typeGenericExpression.Commas);
        Assert.Equal(TokenType.Comma, typeGenericExpression.Commas[0].Type);
        Assert.Equal(",", typeGenericExpression.Commas[0].Value);
    }

    [Fact]
    public void ParseExpression_TypeGenericWithParenthesis_ReadSuccessfully()
    {
        // Arrange
        // Act
        var tokens     = ParsingUtilities.ReadAllTokens("SomeType<(Type1 | Type2), Type3>");
        var index      = 0;
        var expression = ExpressionParser.ParseType(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(11, tokens.Length);
        Assert.Equal(10, index);
        Assert.IsType<TypeGenericExpression>(expression);
        var typeGenericExpression = (TypeGenericExpression)expression;

        Assert.Equal(TokenType.Identifier, typeGenericExpression.Identifier.Type);
        Assert.Equal("SomeType", typeGenericExpression.Identifier.Value);
        Assert.Equal(TokenType.LessThan, typeGenericExpression.LessThan.Type);
        Assert.Equal("<", typeGenericExpression.LessThan.Value);
        Assert.Equal(TokenType.GreaterThan, typeGenericExpression.GreaterThan.Type);
        Assert.Equal(">", typeGenericExpression.GreaterThan.Value);
        Assert.NotNull(typeGenericExpression.TypeParameters);
        Assert.Equal(2, typeGenericExpression.TypeParameters.Count);
        Assert.IsType<ParenthesisedExpression>(typeGenericExpression.TypeParameters[0]);
        Assert.IsType<TypeIdentifierExpression>(typeGenericExpression.TypeParameters[1]);
        Assert.Single(typeGenericExpression.Commas);
        Assert.Equal(TokenType.Comma, typeGenericExpression.Commas[0].Type);
        Assert.Equal(",", typeGenericExpression.Commas[0].Value);
    }

    [Fact]
    public void ParseExpression_TypeGeneric_NoClosingGeneric()
    {
        // Arrange
        const string source = "SomeType<";

        // Act
        var tokens     = ParsingUtilities.ReadAllTokens(source);
        var index      = 0;
        var expression = ExpressionParser.ParseType(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(3, tokens.Length);
        Assert.Equal(1, index);
        Assert.IsType<InvalidExpression>(expression);

        var invalid = (InvalidExpression)expression;
        Assert.Single(invalid.Tokens);
        Assert.Equal(TokenType.GreaterThan, invalid.Tokens[0].Type);
        Assert.Null(invalid.Expression);
        Assert.NotNull(invalid.ExpressionType);
    }

    [Fact]
    public void ParseExpression_TypeGeneric_NoClosingGenericWithParameter()
    {
        // Arrange
        const string source = "SomeType<1";

        // Act
        var tokens     = ParsingUtilities.ReadAllTokens(source);
        var index      = 0;
        var expression = ExpressionParser.ParseType(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(4, tokens.Length);
        Assert.Equal(3, index);
        Assert.IsType<InvalidExpression>(expression);

        var invalid = (InvalidExpression)expression;
        Assert.Empty(invalid.Tokens);
        Assert.NotNull(invalid.Expression);
        Assert.IsType<TypeGenericExpression>(invalid.Expression);
        Assert.NotNull(invalid.ExpressionType);
        Assert.Equal(typeof(TypeGenericExpression), invalid.ExpressionType);

        var typeGeneric = (TypeGenericExpression)invalid.Expression;
        Assert.Equal(TokenType.Identifier, typeGeneric.Identifier.Type);
        Assert.Equal("SomeType", typeGeneric.Identifier.Value);

        Assert.Equal(TokenType.LessThan, typeGeneric.LessThan.Type);
        Assert.Equal("<", typeGeneric.LessThan.Value);

        Assert.Equal(TokenType.None, typeGeneric.GreaterThan.Type);
        Assert.Equal("", typeGeneric.GreaterThan.Value);

        Assert.Single(typeGeneric.TypeParameters);
        Assert.IsType<NumberLiteralExpression>(typeGeneric.TypeParameters[0]);

        Assert.Empty(typeGeneric.Commas);
    }

    [Fact]
    public void ParseExpression_TypeGeneric_ReservedKeywordFound()
    {
        // Arrange
        const string source = """
                              SomeType<1, 2
                              var t = 1;
                              """;

        // Act
        var tokens     = ParsingUtilities.ReadAllTokens(source);
        var index      = 0;
        var expression = ExpressionParser.ParseType(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(11, tokens.Length);
        Assert.Equal(5, index);
        Assert.IsType<InvalidExpression>(expression);

        var invalid = (InvalidExpression)expression;
        Assert.Empty(invalid.Tokens);
        Assert.NotNull(invalid.Expression);
        Assert.IsType<TypeGenericExpression>(invalid.Expression);
        Assert.NotNull(invalid.ExpressionType);
        Assert.Equal(typeof(TypeGenericExpression), invalid.ExpressionType);

        var typeGeneric = (TypeGenericExpression)invalid.Expression;
        Assert.Equal(TokenType.Identifier, typeGeneric.Identifier.Type);
        Assert.Equal("SomeType", typeGeneric.Identifier.Value);

        Assert.Equal(TokenType.LessThan, typeGeneric.LessThan.Type);
        Assert.Equal("<", typeGeneric.LessThan.Value);

        Assert.Equal(TokenType.None, typeGeneric.GreaterThan.Type);
        Assert.Equal("", typeGeneric.GreaterThan.Value);

        Assert.Equal(2, typeGeneric.TypeParameters.Count);
        Assert.IsType<NumberLiteralExpression>(typeGeneric.TypeParameters[0]);
        Assert.IsType<NumberLiteralExpression>(typeGeneric.TypeParameters[1]);

        Assert.Single(typeGeneric.Commas);
        Assert.Equal(TokenType.Comma, typeGeneric.Commas[0].Type);
        Assert.Equal(",", typeGeneric.Commas[0].Value);
    }
}