using DummyLang.LexicalAnalysis;
using DummyLang.LexicalAnalysis.Extensions;
using DummyLang.SyntacticAnalysis.Expressions;
using DummyLang.SyntacticAnalysis.Parsers;
using DummyLang.SyntacticAnalysis.Statements;
using DummyLang.SyntacticAnalysis.Utilities;
using System;
using System.ComponentModel;

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

            if (_tokens[_index].IsEndOfFile() || _tokens[_index].IsInvalid())
                break;

            var node = ParseStatement();
            _syntaxTree.Insert(node);

            if (lastIndex == _index)
                MoveToNext();
        }

        _index = 0;

        return _syntaxTree;
    }

    private void MoveToNext() => _index++;

    private Token GetAndMoveToNext() => _index < _tokens.Length ? _tokens[_index++] : Token.None;

    private Token LookAt(int index) => index >= 0 && index < _tokens.Length ? _tokens[index] : Token.None;

    private Statement ParseStatement()
    {
        var tokenType = _tokens[_index].Type;
        switch (tokenType)
        {
            case TokenType.Var:
            case TokenType.Const:
                return ParseVariableDeclaration();
            default:
                throw new InvalidEnumArgumentException(nameof(tokenType));
        }
    }

    private VariableDeclarationStatement ParseVariableDeclaration()
    {
        // This is always the first token
        var declarationKeyword = GetAndMoveToNext();

        // 1. Find semicolon
        // 2. If found, get index and search backwards for:
        //      a) "="
        //      b) ":"
        //      c) and the rest
        // 3. If not found, search should stop at:
        //      a) next reserved keyword
        //      b) next identifier after new line, if last token is specific operator (TODO: Explore more)

        Token lookAhead;
        var   lastIndex = _index;
        var   colonIdx  = -1;
        var   assignIdx = -1;
        while ((lookAhead = LookAt(lastIndex)).Type != TokenType.Eof)
        {
            if (lookAhead.Type == TokenType.Semicolon || lookAhead.IsReservedKeyword())
                break;

            if (colonIdx < 0 && lookAhead.Type == TokenType.Colon)
                colonIdx = lastIndex;
            else if (assignIdx < 0 && lookAhead.Type == TokenType.Assign)
                assignIdx = lastIndex;

            lastIndex++;
        }

        var identifier = _index == colonIdx - 1
            ? new IdentifierExpression(_tokens[colonIdx - 1])
            : null;

        var typeAssignmentOperator = colonIdx >= 0 && colonIdx < _tokens.Length ? _tokens[colonIdx] : Token.None;
        var typeValue = colonIdx + 1 == assignIdx - 1
            ? new IdentifierExpression(_tokens[assignIdx - 1])
            : colonIdx + 1 == lastIndex - 1
                ? new IdentifierExpression(_tokens[lastIndex - 1])
                : null;

        _index = assignIdx + 1;
        var valueAssignmentOperator = assignIdx >= 0 && assignIdx < _tokens.Length ? _tokens[assignIdx] : Token.None;
        var valueAssignment = _tokens[_index].Type != TokenType.Semicolon
            ? ExpressionParser.Parse(ref _index, _tokens[..lastIndex])
            : null;

        var terminator = lookAhead.Type == TokenType.Semicolon ? lookAhead : Token.None;

        _index = lastIndex + 1;

        return new VariableDeclarationStatement(
            declarationKeyword,
            identifier,
            typeAssignmentOperator,
            typeValue,
            valueAssignmentOperator,
            valueAssignment,
            terminator);
    }
}