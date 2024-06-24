using System;
using System.Text;

namespace DummyLang.Diagnostics;

public readonly struct DiagnosticInfo : IEquatable<DiagnosticInfo>
{
    private static readonly StringBuilder Sb = new();

    public DiagnosticType Type    { get; }
    public string         Message { get; }
    public string         Path    { get; }
    public int            Line    { get; }
    public int            Column  { get; }

    public DiagnosticInfo(DiagnosticType type, string message, string path, int line, int column)
    {
        Type    = type;
        Message = message;
        Path    = path;
        Line    = line;
        Column  = column;
    }

    public void Log()
    {
        Sb.Clear();
        Sb.AppendLine(!string.IsNullOrWhiteSpace(Message) ? Message : "Unknown Error");

        if (!string.IsNullOrWhiteSpace(Path))
        {
            Sb.Append("\tat ").Append(Path);
            Sb.Append(". (line: ")
              .Append(Line)
              .Append(", column: ")
              .Append(Column)
              .Append(')');
        }

        Console.WriteLine(Sb.ToString());
    }

    public bool Equals(DiagnosticInfo other) =>
        Type == other.Type
        && Message == other.Message
        && Path == other.Path
        && Line == other.Line
        && Column == other.Column
        && GetHashCode() == other.GetHashCode();

    public override bool Equals(object? obj) => obj is DiagnosticInfo other && Equals(other);

    public override int GetHashCode() =>
        HashCode.Combine(typeof(DiagnosticInfo), (int)Type, Message, Path, Line, Column);

    public static bool operator ==(DiagnosticInfo left, DiagnosticInfo right) => left.Equals(right);

    public static bool operator !=(DiagnosticInfo left, DiagnosticInfo right) => !(left == right);
}