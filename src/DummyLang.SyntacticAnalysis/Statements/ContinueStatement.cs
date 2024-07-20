using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;

namespace DummyLang.SyntacticAnalysis.Statements;

public sealed class ContinueStatement : IStatement
{
    internal const string InvalidToken      = "Invalid token after 'break' keyword.";
    internal const string SemicolonExpected = "Semicolon expected at the end of a 'break' statement.";
    
    public         Token  ContinueKeyword { get; }
    public         Token  Label           { get; }
    public         Token  Terminator      { get; }

    internal ContinueStatement(Token continueKeyword, Token label, Token terminator)
    {
        ContinueKeyword = continueKeyword;
        Label           = label;
        Terminator      = terminator;
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}