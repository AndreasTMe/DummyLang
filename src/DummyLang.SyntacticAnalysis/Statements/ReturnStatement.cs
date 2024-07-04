using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;

namespace DummyLang.SyntacticAnalysis.Statements;

public sealed class ReturnStatement : IStatement
{
    public Token         ReturnKeyword { get; }
    public IExpression[] Expressions   { get; }
    public Token         Terminator    { get; }

    internal ReturnStatement(Token returnKeyword, IExpression[] expressions, Token terminator)
    {
        ReturnKeyword = returnKeyword;
        Expressions   = expressions;
        Terminator    = terminator;
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}