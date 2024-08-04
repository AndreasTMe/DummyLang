using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;
using System.Collections.Generic;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class TypeGenericExpression : IExpression, ITypeExpression
{
    internal const string RightGenericBracketMissing = "Right generic bracket token expected.";
    internal const string LastArgumentHasComma       = "Last argument should not be followed by comma.";
    internal const string CommaExpected              = "Comma expected.";

    public Token                                  Identifier    { get; }
    public Token                                  LessThan      { get; }
    public IReadOnlyList<TypeArgumentExpression>? TypeArguments { get; }
    public Token                                  GreaterThan   { get; }

    public TokenPositions Positions { get; }

    internal TypeGenericExpression(Token identifier,
                                   Token lessThan,
                                   Token greaterThan,
                                   List<TypeArgumentExpression>? typeArguments = null)
    {
        Identifier    = identifier;
        LessThan      = lessThan;
        TypeArguments = typeArguments is { Count: > 0 } ? typeArguments : null;
        GreaterThan   = greaterThan;

        Positions = new TokenPositions(
            identifier.Position,
            lessThan.Position,
            typeArguments?[0].Positions[0],
            greaterThan.Position);
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}