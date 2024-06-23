using DummyLang.Diagnostics;
using System.Collections.Generic;

namespace DummyLang.SyntacticAnalysis.Diagnostics;

internal sealed class SyntaxDiagnosticsHandler : IDiagnosticsHandler
{
    private readonly HashSet<DiagnosticInfo> _diagnostics = [];

    public IReadOnlySet<DiagnosticInfo> Diagnostics => _diagnostics;

    public void Capture(string message, string path, int line, int column)
    {
        _diagnostics.Add(new DiagnosticInfo(
            DiagnosticType.Syntax,
            message,
            path,
            line,
            column));
    }

    public void Clear() => _diagnostics.Clear();
}