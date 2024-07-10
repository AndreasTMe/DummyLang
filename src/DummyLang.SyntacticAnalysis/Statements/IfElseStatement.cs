using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;
using System.Collections.Generic;

namespace DummyLang.SyntacticAnalysis.Statements;

public sealed class IfElseStatement : IStatement
{
    public IfBlock            If      { get; }
    public List<ElseIfBlock>? ElseIfs { get; }
    public ElseBlock?         Else    { get; }

    internal IfElseStatement(IfBlock ifBlock, List<ElseIfBlock>? elseIfBlocks, ElseBlock? elseBlock)
    {
        If      = ifBlock;
        ElseIfs = elseIfBlocks is { Count: > 0 } ? elseIfBlocks : null;
        Else    = elseBlock;
    }

    public sealed class IfBlock
    {
        public Token              IfKeyword        { get; }
        public Token              LeftParenthesis  { get; }
        public IExpression?       Condition        { get; }
        public Token              RightParenthesis { get; }
        public CompoundStatement? Block            { get; }

        internal IfBlock(Token ifKeyword,
                         Token leftParenthesis,
                         IExpression? condition,
                         Token rightParenthesis,
                         CompoundStatement? block)
        {
            IfKeyword        = ifKeyword;
            LeftParenthesis  = leftParenthesis;
            Condition        = condition;
            RightParenthesis = rightParenthesis;
            Block            = block;
        }
    }

    public sealed class ElseIfBlock
    {
        public Token    ElseKeyword { get; }
        public IfBlock? IfBlock     { get; }

        internal ElseIfBlock(Token elseKeyword, IfBlock? ifBlock)
        {
            ElseKeyword = elseKeyword;
            IfBlock     = ifBlock;
        }
    }

    public sealed class ElseBlock
    {
        public Token              ElseKeyword { get; }
        public CompoundStatement? Block       { get; }

        internal ElseBlock(Token elseKeyword, CompoundStatement? block)
        {
            ElseKeyword = elseKeyword;
            Block       = block;
        }
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}