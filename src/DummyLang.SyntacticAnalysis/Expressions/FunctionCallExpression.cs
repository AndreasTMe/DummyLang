using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;
using System.Collections.Generic;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class FunctionCallExpression : IExpression
{
    public Token                              Identifier       { get; }
    public Token                              LeftParenthesis  { get; }
    public Token                              RightParenthesis { get; }
    public IReadOnlyList<ArgumentExpression>? Arguments        { get; }

    internal FunctionCallExpression(Token identifier,
                                    Token leftParenthesis,
                                    Token rightParenthesis,
                                    List<ArgumentExpression>? arguments = null)
    {
        Identifier       = identifier;
        LeftParenthesis  = leftParenthesis;
        RightParenthesis = rightParenthesis;
        Arguments        = arguments;
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}