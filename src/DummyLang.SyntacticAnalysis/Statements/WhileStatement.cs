using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Expressions;
using DummyLang.SyntacticAnalysis.Expressions.Abstractions;
using DummyLang.SyntacticAnalysis.Statements.Abstractions;

namespace DummyLang.SyntacticAnalysis.Statements;

public sealed class WhileStatement : Statement
{
    public Token             WhileKeyword     { get; }
    public Token             Label            { get; }
    public Token             LeftParenthesis  { get; }
    public IExpression        Condition        { get; }
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
}