using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Expressions;
using DummyLang.SyntacticAnalysis.Parsers;
using DummyLang.SyntacticAnalysis.Utilities;
using DummyLang.SyntacticAnalysis.Visitors;
using Xunit;

namespace DummyLang.SyntacticAnalysis.Tests.Expressions;

public class TypeSyntaxParserTests
{
    [Fact]
    public void ParseExpression_Type_ReadSuccessfully()
    {
        // Arrange
        var validator = new SyntaxNodeValidationVisitor();
        var tokens    = ParsingUtilities.ReadAllTokens("SomeType");
        var index     = 0;

        // Act
        var expression = ExpressionParser.ParseType(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(2, tokens.Length);
        Assert.Equal(1, index);
        Assert.IsType<TypeIdentifierExpression>(expression);

        Assert.False(validator.HasErrors);
        expression.Accept(validator);
        Assert.False(validator.HasErrors);
    }

    [Fact]
    public void ParseExpression_TypeUnion_ReadSuccessfully()
    {
        // Arrange
        var validator = new SyntaxNodeValidationVisitor();
        var tokens    = ParsingUtilities.ReadAllTokens("SomeType | SomeOtherType");
        var index     = 0;

        // Act
        var expression = ExpressionParser.ParseType(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(4, tokens.Length);
        Assert.Equal(3, index);
        Assert.IsType<TypeBinaryExpression>(expression);

        Assert.False(validator.HasErrors);
        expression.Accept(validator);
        Assert.False(validator.HasErrors);
    }

    [Fact]
    public void ParseExpression_TypeIntersection_ReadSuccessfully()
    {
        // Arrange
        var validator = new SyntaxNodeValidationVisitor();
        var tokens    = ParsingUtilities.ReadAllTokens("SomeType & SomeOtherType");
        var index     = 0;

        // Act
        var expression = ExpressionParser.ParseType(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(4, tokens.Length);
        Assert.Equal(3, index);
        Assert.IsType<TypeBinaryExpression>(expression);

        Assert.False(validator.HasErrors);
        expression.Accept(validator);
        Assert.False(validator.HasErrors);
    }

    [Fact]
    public void ParseExpression_TypeExclusion_ReadSuccessfully()
    {
        // Arrange
        var validator = new SyntaxNodeValidationVisitor();
        var tokens    = ParsingUtilities.ReadAllTokens("SomeType ^ SomeOtherType");
        var index     = 0;

        // Act
        var expression = ExpressionParser.ParseType(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(4, tokens.Length);
        Assert.Equal(3, index);
        Assert.IsType<TypeBinaryExpression>(expression);

        Assert.False(validator.HasErrors);
        expression.Accept(validator);
        Assert.False(validator.HasErrors);
    }

    [Fact]
    public void ParseExpression_TypeGeneric_ReadSuccessfully()
    {
        // Arrange
        var validator = new SyntaxNodeValidationVisitor();
        var tokens    = ParsingUtilities.ReadAllTokens("SomeType<Type1, Type2>");
        var index     = 0;

        // Act
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
        Assert.NotNull(typeGenericExpression.TypeArguments);
        Assert.Equal(2, typeGenericExpression.TypeArguments.Count);
        Assert.IsType<TypeArgumentExpression>(typeGenericExpression.TypeArguments[0]);
        Assert.IsType<TypeArgumentExpression>(typeGenericExpression.TypeArguments[1]);

        Assert.False(validator.HasErrors);
        expression.Accept(validator);
        Assert.False(validator.HasErrors);
    }

    [Fact]
    public void ParseExpression_TypeGenericWithParenthesis_ReadSuccessfully()
    {
        // Arrange
        var validator = new SyntaxNodeValidationVisitor();
        var tokens    = ParsingUtilities.ReadAllTokens("SomeType<(Type1 | Type2), Type3>");
        var index     = 0;

        // Act
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
        Assert.NotNull(typeGenericExpression.TypeArguments);
        Assert.Equal(2, typeGenericExpression.TypeArguments.Count);
        Assert.IsType<TypeArgumentExpression>(typeGenericExpression.TypeArguments[0]);
        Assert.IsType<TypeArgumentExpression>(typeGenericExpression.TypeArguments[1]);

        Assert.False(validator.HasErrors);
        expression.Accept(validator);
        Assert.False(validator.HasErrors);
    }

    [Fact]
    public void ParseExpression_TypeGeneric_NoClosingGeneric()
    {
        // Arrange
        var validator = new SyntaxNodeValidationVisitor();
        var tokens    = ParsingUtilities.ReadAllTokens("SomeType<");
        var index     = 0;

        // Act
        var expression = ExpressionParser.ParseType(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(3, tokens.Length);
        Assert.Equal(2, index);
        Assert.IsType<TypeGenericExpression>(expression);

        var typeGenericExpression = (TypeGenericExpression)expression;

        Assert.Equal(TokenType.Identifier, typeGenericExpression.Identifier.Type);
        Assert.Equal("SomeType", typeGenericExpression.Identifier.Value);
        Assert.Equal(TokenType.LessThan, typeGenericExpression.LessThan.Type);
        Assert.Equal("<", typeGenericExpression.LessThan.Value);
        Assert.Equal(TokenType.None, typeGenericExpression.GreaterThan.Type);
        Assert.Equal("", typeGenericExpression.GreaterThan.Value);
        Assert.Null(typeGenericExpression.TypeArguments);

        Assert.False(validator.HasErrors);
        expression.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(
            validator.Diagnostics,
            d => d.Message.EndsWith(TypeGenericExpression.RightGenericBracketMissing));
    }

    [Fact]
    public void ParseExpression_TypeGeneric_NoClosingGenericWithParameter()
    {
        // Arrange
        var validator = new SyntaxNodeValidationVisitor();
        var tokens    = ParsingUtilities.ReadAllTokens("SomeType<1");
        var index     = 0;

        // Act
        var expression = ExpressionParser.ParseType(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(4, tokens.Length);
        Assert.Equal(3, index);
        Assert.IsType<TypeGenericExpression>(expression);

        var typeGenericExpression = (TypeGenericExpression)expression;

        Assert.Equal(TokenType.Identifier, typeGenericExpression.Identifier.Type);
        Assert.Equal("SomeType", typeGenericExpression.Identifier.Value);
        Assert.Equal(TokenType.LessThan, typeGenericExpression.LessThan.Type);
        Assert.Equal("<", typeGenericExpression.LessThan.Value);
        Assert.Equal(TokenType.None, typeGenericExpression.GreaterThan.Type);
        Assert.Equal("", typeGenericExpression.GreaterThan.Value);
        Assert.NotNull(typeGenericExpression.TypeArguments);
        Assert.Equal(1, typeGenericExpression.TypeArguments.Count);
        Assert.IsType<TypeArgumentExpression>(typeGenericExpression.TypeArguments[0]);

        Assert.False(validator.HasErrors);
        expression.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(
            validator.Diagnostics,
            d => d.Message.EndsWith(TypeGenericExpression.RightGenericBracketMissing));
    }

    [Fact]
    public void ParseExpression_TypeGeneric_ReservedKeywordFound()
    {
        // Arrange
        var validator = new SyntaxNodeValidationVisitor();
        var tokens = ParsingUtilities.ReadAllTokens(
            """
            SomeType<1, 2
            var t = 1;
            """);
        var index = 0;

        // Act
        var expression = ExpressionParser.ParseType(ref index, in tokens);

        // Assert
        Assert.NotNull(expression);
        Assert.Equal(11, tokens.Length);
        Assert.Equal(5, index);
        Assert.IsType<TypeGenericExpression>(expression);

        var typeGenericExpression = (TypeGenericExpression)expression;

        Assert.Equal(TokenType.Identifier, typeGenericExpression.Identifier.Type);
        Assert.Equal("SomeType", typeGenericExpression.Identifier.Value);
        Assert.Equal(TokenType.LessThan, typeGenericExpression.LessThan.Type);
        Assert.Equal("<", typeGenericExpression.LessThan.Value);
        Assert.Equal(TokenType.None, typeGenericExpression.GreaterThan.Type);
        Assert.Equal("", typeGenericExpression.GreaterThan.Value);
        Assert.NotNull(typeGenericExpression.TypeArguments);
        Assert.Equal(2, typeGenericExpression.TypeArguments.Count);
        Assert.IsType<TypeArgumentExpression>(typeGenericExpression.TypeArguments[0]);
        Assert.IsType<TypeArgumentExpression>(typeGenericExpression.TypeArguments[1]);

        Assert.False(validator.HasErrors);
        expression.Accept(validator);
        Assert.True(validator.HasErrors);
        Assert.Equal(1, validator.ErrorCount);
        Assert.Contains(
            validator.Diagnostics,
            d => d.Message.EndsWith(TypeGenericExpression.RightGenericBracketMissing));
    }
}