using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;

namespace DummyLang.SyntacticAnalysis.Statements;

public sealed class WhileStatement : IStatement
{
    internal const string LeftParenthesisExpected =
        "Left parenthesis token expected after 'while' keyword or its label";
    internal const string ExpressionExpected       = "Expression expected.";
    internal const string RightParenthesisExpected = "Right parenthesis token expected after the 'while' condition";
    internal const string WhileBlockExpected       = "'while' block expected.";

    public Token              WhileKeyword     { get; }
    public Token              Label            { get; }
    public Token              LeftParenthesis  { get; }
    public IExpression?       Condition        { get; }
    public Token              RightParenthesis { get; }
    public CompoundStatement? Block            { get; }

    internal WhileStatement(Token whileKeyword,
                            Token label,
                            Token leftParenthesis,
                            IExpression? condition,
                            Token rightParenthesis,
                            CompoundStatement? block)
    {
        WhileKeyword     = whileKeyword;
        Label            = label;
        LeftParenthesis  = leftParenthesis;
        Condition        = condition;
        RightParenthesis = rightParenthesis;
        Block            = block;
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}