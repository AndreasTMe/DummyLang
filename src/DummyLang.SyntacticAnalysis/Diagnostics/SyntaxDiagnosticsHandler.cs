using DummyLang.Diagnostics;
using System.Collections.Generic;
using System.Threading;

namespace DummyLang.SyntacticAnalysis.Diagnostics;

internal sealed class SyntaxDiagnosticsHandler : IDiagnosticsHandler
{
    private readonly ReaderWriterLockSlim _lock = new(LockRecursionPolicy.NoRecursion);
    private readonly HashSet<DiagnosticInfo> _diagnostics = [];

    public IReadOnlySet<DiagnosticInfo> Diagnostics
    {
        get
        {
            _lock.EnterReadLock();
            try
            {
                return _diagnostics;
            }
            finally
            {
                if (_lock.IsReadLockHeld)
                    _lock.ExitReadLock();
            }
        }
    }

    public void Capture(string message, string path, int line, int column)
    {
        _lock.EnterWriteLock();
        try
        {
            _diagnostics.Add(new DiagnosticInfo(
                DiagnosticType.Syntax,
                message,
                path,
                line,
                column));
        }
        finally
        {
            if (_lock.IsWriteLockHeld)
                _lock.ExitWriteLock();
        }
    }

    public void Clear()
    {
        _lock.EnterWriteLock();
        try
        {
            _diagnostics.Clear();
        }
        finally
        {
            if (_lock.IsWriteLockHeld) _lock.ExitWriteLock();
        }
    }
}