using DummyLang.LexicalAnalysis;

namespace DummyLang.SyntacticAnalysis.Abstractions;

public abstract class PositionedNode
{
    // ReSharper disable once InconsistentNaming
    protected TokenPosition? _start;
    // ReSharper disable once InconsistentNaming
    protected TokenPosition? _end;
}