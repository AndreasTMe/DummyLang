using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class UnexpectedTokenExpression : PositionedNode, IExpression, ITypeExpression
{
    internal const string UnexpectedToken = "Unexpected token.";

    public TokenPosition Start => _start ??= Token.Position;
    public TokenPosition End   => _end ??= Token.Position;

    public Token Token { get; }

    internal UnexpectedTokenExpression(Token token) => Token = token;

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}