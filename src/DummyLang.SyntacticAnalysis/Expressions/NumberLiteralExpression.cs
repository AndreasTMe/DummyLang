using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;
using DummyLang.SyntacticAnalysis.Utilities;
using System;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class NumberLiteralExpression : PositionedNode, IExpression, ITypeExpression
{
    public TokenPosition Start => _start ??= NumberToken.Position;
    public TokenPosition End   => _end ??= NumberToken.Position;

    public Token      NumberToken { get; }
    public NumberType Type        { get; }

    internal NumberLiteralExpression(Token numberToken)
    {
        NumberToken = numberToken;

        var number = numberToken.Value;
        // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
        switch (numberToken.Type)
        {
            case TokenType.Integer:
            {
                Type = NumberType.Integer;

                if (number.StartsWith("0x"))
                    Type = NumberType.Hexadecimal;
                else if (number.StartsWith("0b"))
                    Type = NumberType.Binary;
                else if (number.EndsWith("ul", StringComparison.OrdinalIgnoreCase))
                    Type = NumberType.UnsignedLong;
                else if (number.EndsWith("u", StringComparison.OrdinalIgnoreCase))
                    Type = NumberType.UnsignedInteger;
                else if (number.EndsWith("l", StringComparison.OrdinalIgnoreCase))
                    Type = NumberType.Long;
                break;
            }
            case TokenType.Real:
            {
                Type = NumberType.Double;

                if (number.EndsWith("f", StringComparison.OrdinalIgnoreCase))
                    Type = NumberType.Float;
                else if (number.EndsWith("d", StringComparison.OrdinalIgnoreCase))
                    Type = NumberType.Double;
                else if (number.EndsWith("m", StringComparison.OrdinalIgnoreCase))
                    Type = NumberType.Decimal;
                else if (number.Contains('e', StringComparison.OrdinalIgnoreCase))
                    Type = NumberType.WithExponent;
                break;
            }
            default:
                Type = NumberType.None;
                break;
        }
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}