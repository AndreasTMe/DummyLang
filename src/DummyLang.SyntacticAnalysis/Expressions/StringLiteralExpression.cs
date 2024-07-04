using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class StringLiteralExpression : IExpression, ITypeExpression
{
    public Token StringToken { get; }

    internal StringLiteralExpression(Token stringToken) => StringToken = stringToken;
    
    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}