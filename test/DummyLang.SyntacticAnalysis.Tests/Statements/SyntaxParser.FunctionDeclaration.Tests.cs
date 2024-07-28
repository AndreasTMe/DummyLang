using DummyLang.SyntacticAnalysis.Parsers;
using DummyLang.SyntacticAnalysis.Utilities;
using DummyLang.SyntacticAnalysis.Visitors;
using Xunit;

namespace DummyLang.SyntacticAnalysis.Tests.Statements;

public class FunctionDeclarationSyntaxParserTests
{
    [Fact]
    public void ParseStatement_FunctionDeclarationNoParams_ShouldBeReadCorrectly()
    {
        // Arrange
        const string source = """
                              func test: () => void = () => {
                              }
                              """;
        var tokens    = ParsingUtilities.ReadAllTokens(source);
        var index     = 0;
        var validator = new SyntaxNodeValidationVisitor();

        // Act
        var statement = StatementParser.Parse(ref index, in tokens);

        // Assert
        // TODO: Implement tests

        Assert.False(validator.HasErrors);
        statement.Accept(validator);
        Assert.False(validator.HasErrors);
    }
}