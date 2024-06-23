using DummyLang.Diagnostics;
using System;
using System.Collections.Generic;

namespace DummyLang.SyntacticAnalysis;

public sealed class SyntaxTree
{
    public abstract class Node
    {
        public abstract void PrettyPrint(int indent);
    }

    private readonly List<Node> _nodes = [];
    private readonly HashSet<DiagnosticInfo> _diagnostics = [];

    public IReadOnlyList<Node> Nodes => _nodes;
    public IReadOnlySet<DiagnosticInfo> Diagnostics => _diagnostics;

    internal void Insert(in Node node)
    {
        _nodes.Add(node);
    }

    internal void CaptureDiagnostics(in IReadOnlySet<DiagnosticInfo> diagnostics)
    {
        _diagnostics.UnionWith(diagnostics);
    }

    public void Clear()
    {
        _nodes.Clear();
        _diagnostics.Clear();
    }

    public void PrettyPrint()
    {
        Console.WriteLine(nameof(SyntaxTree));

        foreach (var node in _nodes)
        {
            node.PrettyPrint(1);
        }
    }
}