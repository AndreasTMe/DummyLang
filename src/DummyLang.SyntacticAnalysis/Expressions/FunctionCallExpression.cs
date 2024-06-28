using DummyLang.LexicalAnalysis;
using System.Collections.Generic;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class FunctionCallExpression : Expression
{
    public Token Identifier       { get; }
    public Token LeftParenthesis  { get; }
    public Token RightParenthesis { get; }

    public IReadOnlyList<Expression> Parameters { get; }
    public IReadOnlyList<Token>      Commas     { get; }

    public FunctionCallExpression(Token identifier,
                                  Token leftParenthesis,
                                  Token rightParenthesis,
                                  List<Expression> parameters,
                                  List<Token> commas)
    {
        Identifier       = identifier;
        LeftParenthesis  = leftParenthesis;
        RightParenthesis = rightParenthesis;
        Parameters       = parameters;
        Commas           = commas;
    }
}