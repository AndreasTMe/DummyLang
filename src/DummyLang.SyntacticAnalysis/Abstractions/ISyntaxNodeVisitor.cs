using DummyLang.SyntacticAnalysis.Expressions;
using DummyLang.SyntacticAnalysis.Statements;

namespace DummyLang.SyntacticAnalysis.Abstractions;

public interface ISyntaxNodeVisitor
{
    /*************
     * Expressions
     *************/

    void Visit(ArgumentExpression expression);

    void Visit(BinaryExpression expression);

    void Visit(CharacterLiteralExpression expression);

    void Visit(FunctionCallExpression expression);

    void Visit(IdentifierExpression expression);

    void Visit(IndexArgumentExpression expression);

    void Visit(IndexerExpression expression);

    void Visit(MemberAccessExpression expression);

    void Visit(NumberLiteralExpression expression);

    void Visit(ParameterExpression expression);

    void Visit(ParenthesisedExpression expression);

    void Visit(PrimaryExpression expression);

    void Visit(RangeExpression expression);

    void Visit(StringLiteralExpression expression);

    void Visit(TypeArgumentExpression expression);

    void Visit(TypeBinaryExpression expression);

    void Visit(TypeFunctionExpression expression);

    void Visit(TypeGenericExpression expression);

    void Visit(TypeIdentifierExpression expression);

    void Visit(TypeParameterExpression expression);

    void Visit(UnaryExpression expression);

    void Visit(UnexpectedTokenExpression expression);

    /*************
     * Statements
     *************/

    void Visit(BreakStatement statement);

    void Visit(CompoundStatement statement);

    void Visit(ContinueStatement statement);

    void Visit(ExpressionStatement statement);

    void Visit(FunctionDeclarationStatement statement);

    void Visit(IfElseStatement statement);

    void Visit(NoOpStatement statement);

    void Visit(ReturnStatement statement);

    void Visit(VariableDeclarationStatement statement);

    void Visit(WhileStatement statement);
}