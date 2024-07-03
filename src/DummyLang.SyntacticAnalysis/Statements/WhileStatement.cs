using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;
using DummyLang.SyntacticAnalysis.Expressions.Abstractions;
using DummyLang.SyntacticAnalysis.Statements.Abstractions;

namespace DummyLang.SyntacticAnalysis.Statements;

public sealed class WhileStatement : IStatement
{
    public Token             WhileKeyword     { get; }
    public Token             Label            { get; }
    public Token             LeftParenthesis  { get; }
    public IExpression       Condition        { get; }
    public Token             RightParenthesis { get; }
    public CompoundStatement Block            { get; }

    internal WhileStatement(Token whileKeyword,
                            Token label,
                            Token leftParenthesis,
                            IExpression condition,
                            Token rightParenthesis,
                            CompoundStatement block)
    {
        WhileKeyword     = whileKeyword;
        Label            = label;
        LeftParenthesis  = leftParenthesis;
        Condition        = condition;
        RightParenthesis = rightParenthesis;
        Block            = block;
    }

    public void Accept(ISyntaxNodeVisitor visitor)
    {
        visitor.Visit(this);

        Condition.Accept(visitor);
        Block.Accept(visitor);
    }
}