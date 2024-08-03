using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;
using System.Collections.Generic;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class FunctionCallExpression : PositionedNode, IExpression
{
    internal const string RightParenthesisExpected = "Right parenthesis token expected.";
    internal const string LastArgumentHasComma     = "Last argument should not be followed by comma.";
    internal const string CommaExpected            = "Comma expected.";

    public TokenPosition Start =>
        _start ??= TokenPosition.GetMin(
            Identifier.Position,
            LeftParenthesis.Position,
            RightParenthesis.Position,
            Arguments?[0].Start);

    public TokenPosition End =>
        _end ??= TokenPosition.GetMax(
            Identifier.Position,
            LeftParenthesis.Position,
            RightParenthesis.Position,
            Arguments?[^1].End);

    public Token                              Identifier       { get; }
    public Token                              LeftParenthesis  { get; }
    public Token                              RightParenthesis { get; }
    public IReadOnlyList<ArgumentExpression>? Arguments        { get; }

    internal FunctionCallExpression(Token identifier,
                                    Token leftParenthesis,
                                    Token rightParenthesis,
                                    List<ArgumentExpression>? arguments = null)
    {
        Identifier       = identifier;
        LeftParenthesis  = leftParenthesis;
        RightParenthesis = rightParenthesis;
        Arguments        = arguments is { Count: > 0 } ? arguments : null;
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}