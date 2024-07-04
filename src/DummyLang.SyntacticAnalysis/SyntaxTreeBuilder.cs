using DummyLang.LexicalAnalysis;
using DummyLang.LexicalAnalysis.Extensions;
using DummyLang.SyntacticAnalysis.Parsers;
using DummyLang.SyntacticAnalysis.Utilities;
using System;

namespace DummyLang.SyntacticAnalysis;

public class SyntaxTreeBuilder
{
    private readonly SyntaxTree _syntaxTree = new();

    private string  _sourcePath = string.Empty;
    private Token[] _tokens     = [];
    private int     _index;

    public SyntaxTreeBuilder Feed(string sourcePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sourcePath);

        _sourcePath = sourcePath;
        _tokens     = ParsingUtilities.ReadAllTokens(sourcePath);
        _index      = 0;

        return this;
    }

    public SyntaxTree Build()
    {
        _syntaxTree.Clear();

        while (_index < _tokens.Length)
        {
            var lastIndex = _index;

            if (_tokens[_index].IsEndOfFile() || _tokens[_index].IsNone())
                break;

            var node = StatementParser.Parse(ref _index, in _tokens);
            _syntaxTree.Insert(node);

            if (lastIndex == _index)
                _index++;
        }

        _index = 0;

        return _syntaxTree;
    }
}