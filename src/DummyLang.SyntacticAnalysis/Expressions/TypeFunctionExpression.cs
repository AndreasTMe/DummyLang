using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;
using System.Collections.Generic;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class TypeFunctionExpression : ITypeExpression
{
    internal const string LastParameterHasComma    = "Last parameter should not be followed by comma.";
    internal const string RightParenthesisExpected = "Right parenthesis token expected.";
    internal const string LambdaAssignExpected     = "Lambda assign token expected";
    internal const string LastArgumentHasComma     = "Last argument should not be followed by comma.";
    internal const string CommaExpected            = "Comma expected.";

    public Token                                   LeftParen    { get; }
    public IReadOnlyList<TypeParameterExpression>? InputTypes   { get; }
    public Token                                   RightParen   { get; }
    public Token                                   LambdaAssign { get; }
    public IReadOnlyList<TypeArgumentExpression>?  OutputTypes  { get; }

    internal TypeFunctionExpression(Token leftParen,
                                    IReadOnlyList<TypeParameterExpression>? inputTypes,
                                    Token rightParen,
                                    Token lambdaAssign,
                                    IReadOnlyList<TypeArgumentExpression>? outputTypes)
    {
        LeftParen    = leftParen;
        InputTypes   = inputTypes;
        RightParen   = rightParen;
        LambdaAssign = lambdaAssign;
        OutputTypes  = outputTypes;
    }

    public void Accept(ISyntaxNodeVisitor visitor)
    {
        visitor.Visit(this);
    }
}