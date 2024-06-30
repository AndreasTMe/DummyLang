using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Utilities;

namespace DummyLang.SyntacticAnalysis.Extensions;

internal static class TokenExtensions
{
    public static bool IsAdditiveOperator(this Token token) =>
        token.Type == TokenType.Plus || token.Type == TokenType.Minus;

    public static OperatorPrecedence GetOperatorPrecedence(this Token token)
    {
        switch (token.Type)
        {
            case TokenType.Assign:
            case TokenType.LambdaAssign:
            case TokenType.PlusAssign:
            case TokenType.MinusAssign:
            case TokenType.StarAssign:
            case TokenType.SlashAssign:
            case TokenType.PercentAssign:
            case TokenType.LeftBitShiftAssign:
            case TokenType.RightBitShiftAssign:
            case TokenType.AmpersandAssign:
            case TokenType.PipeAssign:
            case TokenType.CaretAssign:
            case TokenType.DoubleQuestionMarkAssign:
                return OperatorPrecedence.Assignment;
            case TokenType.DoubleQuestionMark:
                return OperatorPrecedence.NullCoalescing;
            case TokenType.DoublePipe:
                return OperatorPrecedence.ConditionalOr;
            case TokenType.DoubleAmpersand:
                return OperatorPrecedence.ConditionalAnd;
            case TokenType.Pipe:
                return OperatorPrecedence.BitwiseOr;
            case TokenType.Caret:
                return OperatorPrecedence.BitwiseXOr;
            case TokenType.Ampersand:
                return OperatorPrecedence.BitwiseAnd;
            case TokenType.Equal:
            case TokenType.NotEqual:
                return OperatorPrecedence.Equality;
            case TokenType.LessThan:
            case TokenType.LessThanOrEqual:
            case TokenType.GreaterThan:
            case TokenType.GreaterThanOrEqual:
                return OperatorPrecedence.Relational;
            case TokenType.LeftBitShift:
            case TokenType.RightBitShift:
                return OperatorPrecedence.Bitshift;
            case TokenType.Plus:
            case TokenType.Minus:
                return OperatorPrecedence.Additive;
            case TokenType.Star:
            case TokenType.Slash:
            case TokenType.Percent:
                return OperatorPrecedence.Multiplicative;
            case TokenType.DoubleDot:
                return OperatorPrecedence.Range;
            default:
                return OperatorPrecedence.None;
        }
    }
}