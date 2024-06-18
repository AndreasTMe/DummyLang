using DummyLang.SyntacticAnalysis.Utilities;
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
    
    public IReadOnlyList<Node> Nodes => _nodes;

    public void Add(in Node node)
    {
        _nodes.Add(node);
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