using Xunit;

namespace DummyLang.SyntacticAnalysis.Tests.Statements;

public class VariableStatementSyntaxParserTests
{
    [Fact]
    public void GenerateSyntax_Statements_ConstDeclaration()
    {
        // TODO: This passes but is in no way a good test
        // Arrange
        const string source = "const foo: i32 = 1 + 2;";

        // Act
        var parser = new SyntaxParser();
        var syntaxTree = parser.Feed(source)
                               .GenerateSyntax();

        // Assert
        Assert.NotNull(syntaxTree);
    }
}