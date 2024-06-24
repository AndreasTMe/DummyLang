using System.Collections.Generic;

namespace DummyLang.Diagnostics;

public interface IDiagnosticsHandler
{
    IReadOnlySet<DiagnosticInfo> Diagnostics { get; }

    void Capture(string message, string path, int line, int column);

    void Clear();
}