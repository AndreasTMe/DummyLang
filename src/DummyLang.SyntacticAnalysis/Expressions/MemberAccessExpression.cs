using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Utilities;

namespace DummyLang.SyntacticAnalysis.Expressions;

public sealed class MemberAccessExpression : Expression
{
    public Expression Identifier { get; }
    public Token      Access     { get; }
    public Expression Member     { get; }

    public MemberAccessExpression(Expression identifier, Token access, Expression member)
    {
        Identifier = identifier;
        Access     = access;
        Member     = member;
    }

    public override void PrettyPrint(int indent)
    {
        ConsoleUtilities.WriteLineFormatted(nameof(MemberAccessExpression), indent);

        Identifier.PrettyPrint(indent + 1);

        if (Access.Type != TokenType.None)
            ConsoleUtilities.WriteLineFormatted(Access.Type.ToString(), indent + 1);

        Member.PrettyPrint(indent + 1);
    }
}