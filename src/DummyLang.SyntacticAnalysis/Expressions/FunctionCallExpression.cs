using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Utilities;
using System;
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

    public override void PrettyPrint(int indent)
    {
        ConsoleUtilities.WriteLineFormatted($"{nameof(FunctionCallExpression)}({Identifier.Value})", indent);

        ConsoleUtilities.WriteLineFormatted(Identifier.Type.ToString(), indent + 1);
        ConsoleUtilities.WriteLineFormatted(LeftParenthesis.Type.ToString(), indent + 1);

        for (var i = 0; i < Math.Max(Parameters.Count, Commas.Count); i++)
        {
            if (i < Parameters.Count)
                Parameters[i].PrettyPrint(indent + 1);

            if (i < Commas.Count)
                ConsoleUtilities.WriteLineFormatted(Commas[i].Type.ToString(), indent + 1);
        }

        ConsoleUtilities.WriteLineFormatted(RightParenthesis.Type.ToString(), indent + 1);
    }
}