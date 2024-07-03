using DummyLang.SyntacticAnalysis.Abstractions;
using System.Collections.Generic;

namespace DummyLang.SyntacticAnalysis;

public sealed class SyntaxTree
{
    private readonly List<ISyntaxNode> _nodes = [];

    public IReadOnlyList<ISyntaxNode> Nodes => _nodes;

    internal void Insert(in ISyntaxNode syntaxNode)
    {
        _nodes.Add(syntaxNode);
    }

    public void Clear()
    {
        _nodes.Clear();
    }
}