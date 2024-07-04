using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class CharacterLiteralExpression : IExpression, ITypeExpression
{
    public Token CharacterToken { get; }

    internal CharacterLiteralExpression(Token characterToken) => CharacterToken = characterToken;
    
    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}