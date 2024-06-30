using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Expressions;

namespace DummyLang.SyntacticAnalysis.Statements;

public sealed class ReturnStatement : Statement
{
    public Token        Return      { get; }
    public Expression[] Expressions { get; }
    public Token        Terminator  { get; }

    internal ReturnStatement(Token @return, Expression[] expressions, Token terminator)
    {
        Return      = @return;
        Expressions = expressions;
        Terminator  = terminator;
    }
}