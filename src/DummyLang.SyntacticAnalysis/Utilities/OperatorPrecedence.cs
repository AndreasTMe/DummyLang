namespace DummyLang.SyntacticAnalysis.Utilities;

public enum OperatorPrecedence : byte
{
    None = 0,
    Comma,
    Assignment,
    Ternary,
    NullCoalescing,
    LogicalOr,
    LogicalAnd,
    BitwiseOr,
    BitwiseXOr,
    BitwiseAnd,
    Equality,
    Relational,
    Bitshift,
    Additive,
    Multiplicative,
    Range,
    Unary,
    Primary
}