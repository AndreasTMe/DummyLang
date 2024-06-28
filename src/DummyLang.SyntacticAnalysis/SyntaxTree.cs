using System.Collections.Generic;

namespace DummyLang.SyntacticAnalysis;

public sealed class SyntaxTree
{
    public abstract class Node;

    private readonly List<Node> _nodes = [];

    public IReadOnlyList<Node> Nodes => _nodes;

    internal void Insert(in Node node)
    {
        _nodes.Add(node);
    }

    public void Clear()
    {
        _nodes.Clear();
    }
}