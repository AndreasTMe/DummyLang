using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Expressions;
using DummyLang.SyntacticAnalysis.Expressions.Abstractions;
using DummyLang.SyntacticAnalysis.Statements.Abstractions;

namespace DummyLang.SyntacticAnalysis.Statements;

public sealed class ReturnStatement : Statement
{
    public Token        ReturnKeyword { get; }
    public IExpression[] Expressions   { get; }
    public Token        Terminator    { get; }

    internal ReturnStatement(Token returnKeyword, IExpression[] expressions, Token terminator)
    {
        ReturnKeyword = returnKeyword;
        Expressions   = expressions;
        Terminator    = terminator;
    }
}