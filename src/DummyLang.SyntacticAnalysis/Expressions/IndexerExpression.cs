using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Utilities;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class IndexerExpression : Expression
{
    public Token       Identifier   { get; }
    public Token       LeftBracket  { get; }
    public Expression? Index        { get; }
    public Token       RightBracket { get; }

    public IndexerExpression(Token identifier, Token leftBracket, Token rightBracket, Expression? index)
    {
        Identifier   = identifier;
        LeftBracket  = leftBracket;
        Index        = index;
        RightBracket = rightBracket;
    }

    public override void PrettyPrint(int indent)
    {
        ConsoleUtilities.WriteLineFormatted(nameof(IndexerExpression), indent);

        ConsoleUtilities.WriteLineFormatted($"{Identifier.Type}({Identifier.Value})", indent + 1);

        if (LeftBracket.Type != TokenType.None)
            ConsoleUtilities.WriteLineFormatted(LeftBracket.Type.ToString(), indent + 1);

        Index?.PrettyPrint(indent + 1);

        if (RightBracket.Type != TokenType.None)
            ConsoleUtilities.WriteLineFormatted(RightBracket.Type.ToString(), indent + 1);
    }
}