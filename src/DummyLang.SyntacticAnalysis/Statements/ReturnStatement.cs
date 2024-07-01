using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Expressions;

namespace DummyLang.SyntacticAnalysis.Statements;

public sealed class ReturnStatement : Statement
{
    public Token        ReturnKeyword { get; }
    public Expression[] Expressions   { get; }
    public Token        Terminator    { get; }

    internal ReturnStatement(Token returnKeyword, Expression[] expressions, Token terminator)
    {
        ReturnKeyword = returnKeyword;
        Expressions   = expressions;
        Terminator    = terminator;
    }
}