using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Expressions;
using System.Collections.Generic;

namespace DummyLang.SyntacticAnalysis.Statements;

public sealed class IfElseStatement : Statement
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
        public Token             IfKeyword        { get; }
        public Token             LeftParenthesis  { get; }
        public Expression        Condition        { get; }
        public Token             RightParenthesis { get; }
        public CompoundStatement Block            { get; }

        internal IfBlock(Token ifKeyword,
                         Token leftParenthesis,
                         Expression condition,
                         Token rightParenthesis,
                         CompoundStatement block)
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
        public Token             ElseKeyword      { get; }
        public Token             IfKeyword        { get; }
        public Token             LeftParenthesis  { get; }
        public Expression        Condition        { get; }
        public Token             RightParenthesis { get; }
        public CompoundStatement Block            { get; }

        internal ElseIfBlock(Token elseKeyword, IfBlock ifBlock)
        {
            ElseKeyword      = elseKeyword;
            IfKeyword        = ifBlock.IfKeyword;
            LeftParenthesis  = ifBlock.LeftParenthesis;
            Condition        = ifBlock.Condition;
            RightParenthesis = ifBlock.RightParenthesis;
            Block            = ifBlock.Block;
        }
    }

    public sealed class ElseBlock
    {
        public Token             ElseKeyword { get; }
        public CompoundStatement Block       { get; }

        internal ElseBlock(Token elseKeyword, CompoundStatement block)
        {
            ElseKeyword = elseKeyword;
            Block       = block;
        }
    }
}