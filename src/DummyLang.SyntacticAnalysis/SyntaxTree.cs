using DummyLang.SyntacticAnalysis.Abstractions;
using DummyLang.SyntacticAnalysis.Visitors;
using System.Collections.Generic;

namespace DummyLang.SyntacticAnalysis;

public sealed class SyntaxTree
{
    private readonly List<ISyntaxNode> _syntaxNodes = [];

    public IReadOnlyList<INode> Nodes => _syntaxNodes;

    internal void Insert(in ISyntaxNode syntaxNode)
    {
        _syntaxNodes.Add(syntaxNode);
    }

    public void ValidateSyntax()
    {
        var validationVisitor = new SyntaxNodeValidationVisitor();

        foreach (var syntaxNode in _syntaxNodes)
            syntaxNode.Accept(validationVisitor);

        if (!validationVisitor.HasErrors)
            return;

        foreach (var diagnosticInfo in validationVisitor.Diagnostics)
            diagnosticInfo.Log();
    }

    public void Clear()
    {
        _syntaxNodes.Clear();
    }
}