using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;
using System.Collections.Generic;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class TypeFunctionExpression : PositionedNode, ITypeExpression
{
    internal const string LastParameterHasComma    = "Last parameter should not be followed by comma.";
    internal const string RightParenthesisExpected = "Right parenthesis token expected.";
    internal const string LambdaAssignExpected     = "Lambda assign token expected";
    internal const string LastArgumentHasComma     = "Last argument should not be followed by comma.";
    internal const string CommaExpected            = "Comma expected.";

    public TokenPosition Start =>
        _start ??= TokenPosition.GetMin(
            LeftParen.Position,
            InputTypes?[0].Start,
            RightParen.Position,
            LambdaAssign.Position,
            OutputTypes?[0].Start);

    public TokenPosition End =>
        _end ??= TokenPosition.GetMin(
            LeftParen.Position,
            InputTypes?[^1].End,
            RightParen.Position,
            LambdaAssign.Position,
            OutputTypes?[^1].End);

    public Token                                  LeftParen    { get; }
    public IReadOnlyList<TypeArgumentExpression>? InputTypes   { get; }
    public Token                                  RightParen   { get; }
    public Token                                  LambdaAssign { get; }
    public IReadOnlyList<TypeArgumentExpression>? OutputTypes  { get; }

    internal TypeFunctionExpression(Token leftParen,
                                    IReadOnlyList<TypeArgumentExpression>? inputTypes,
                                    Token rightParen,
                                    Token lambdaAssign,
                                    IReadOnlyList<TypeArgumentExpression>? outputTypes)
    {
        LeftParen    = leftParen;
        InputTypes   = inputTypes is { Count: > 0 } ? inputTypes : null;
        RightParen   = rightParen;
        LambdaAssign = lambdaAssign;
        OutputTypes  = outputTypes is { Count: > 0 } ? outputTypes : null;
    }

    public void Accept(ISyntaxNodeVisitor visitor)
    {
        visitor.Visit(this);
    }
}