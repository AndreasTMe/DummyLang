using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class IdentifierExpression : PositionedNode, IExpression
{
    public TokenPosition Start => _start ??= Token.Position;
    public TokenPosition End   => _end ??= Token.Position;

    public Token Token { get; }

    internal IdentifierExpression(Token token) => Token = token;

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}