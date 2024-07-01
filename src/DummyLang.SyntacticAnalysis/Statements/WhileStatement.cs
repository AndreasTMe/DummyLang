using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Expressions;

namespace DummyLang.SyntacticAnalysis.Statements;

public sealed class WhileStatement : Statement
{
    public Token             WhileKeyword     { get; }
    public Token             Label            { get; }
    public Token             LeftParenthesis  { get; }
    public Expression        Condition        { get; }
    public Token             RightParenthesis { get; }
    public CompoundStatement Block            { get; }

    internal WhileStatement(Token whileKeyword,
                            Token label,
                            Token leftParenthesis,
                            Expression condition,
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