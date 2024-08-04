using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;
using System.Collections.Generic;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class FunctionCallExpression : IExpression
{
    internal const string RightParenthesisExpected = "Right parenthesis token expected.";
    internal const string LastArgumentHasComma     = "Last argument should not be followed by comma.";
    internal const string CommaExpected            = "Comma expected.";

    public Token                              Identifier       { get; }
    public Token                              LeftParenthesis  { get; }
    public IReadOnlyList<ArgumentExpression>? Arguments        { get; }
    public Token                              RightParenthesis { get; }

    public TokenPositions Positions { get; }

    internal FunctionCallExpression(Token identifier,
                                    Token leftParenthesis,
                                    Token rightParenthesis,
                                    List<ArgumentExpression>? arguments = null)
    {
        Identifier       = identifier;
        LeftParenthesis  = leftParenthesis;
        Arguments        = arguments is { Count: > 0 } ? arguments : null;
        RightParenthesis = rightParenthesis;

        Positions = new TokenPositions(
            identifier.Position,
            leftParenthesis.Position,
            arguments?[0].Positions[0],
            rightParenthesis.Position);
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}