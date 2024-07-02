using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Expressions.Abstractions;
using System.Collections.Generic;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class FunctionCallExpression : IExpression
{
    public Token Identifier       { get; }
    public Token LeftParenthesis  { get; }
    public Token RightParenthesis { get; }

    public IReadOnlyList<IExpression> Parameters { get; }
    public IReadOnlyList<Token>      Commas     { get; }

    internal FunctionCallExpression(Token identifier,
                                    Token leftParenthesis,
                                    Token rightParenthesis,
                                    List<IExpression> parameters,
                                    List<Token> commas)
    {
        Identifier       = identifier;
        LeftParenthesis  = leftParenthesis;
        RightParenthesis = rightParenthesis;
        Parameters       = parameters;
        Commas           = commas;
    }
}