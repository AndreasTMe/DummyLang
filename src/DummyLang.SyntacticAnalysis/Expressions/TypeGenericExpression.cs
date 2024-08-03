using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;
using System.Collections.Generic;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class TypeGenericExpression : PositionedNode, IExpression, ITypeExpression
{
    internal const string RightGenericBracketMissing = "Right generic bracket token expected.";
    internal const string LastArgumentHasComma       = "Last argument should not be followed by comma.";
    internal const string CommaExpected              = "Comma expected.";

    public TokenPosition Start =>
        _start ??= TokenPosition.GetMin(
            Identifier.Position,
            LessThan.Position,
            GreaterThan.Position,
            TypeArguments?[0].Start);

    public TokenPosition End =>
        _end ??= TokenPosition.GetMin(
            Identifier.Position,
            LessThan.Position,
            GreaterThan.Position,
            TypeArguments?[^1].End);

    public Token                                  Identifier    { get; }
    public Token                                  LessThan      { get; }
    public Token                                  GreaterThan   { get; }
    public IReadOnlyList<TypeArgumentExpression>? TypeArguments { get; }

    internal TypeGenericExpression(Token identifier,
                                   Token lessThan,
                                   Token greaterThan,
                                   List<TypeArgumentExpression>? typeArguments = null)
    {
        Identifier    = identifier;
        LessThan      = lessThan;
        GreaterThan   = greaterThan;
        TypeArguments = typeArguments is { Count: > 0 } ? typeArguments : null;
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}