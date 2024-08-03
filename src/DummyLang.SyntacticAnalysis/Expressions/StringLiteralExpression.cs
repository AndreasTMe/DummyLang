using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class StringLiteralExpression : PositionedNode, IExpression, ITypeExpression
{
    internal const string InvalidDoubleQuotes =
        "A string literal must start with a double quote (\") and end with a double quote (\").";
    internal const string EscapedLastDoubleQuote = "The last double quote of a string literal should not be escaped.";
    internal const string InvalidEscapedCharacters =
        "The string literal provided contains an invalid escaped character literal.";

    public TokenPosition Start => _start ??= StringToken.Position;
    public TokenPosition End   => _end ??= StringToken.Position;

    public Token StringToken { get; }

    internal StringLiteralExpression(Token stringToken) => StringToken = stringToken;

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}