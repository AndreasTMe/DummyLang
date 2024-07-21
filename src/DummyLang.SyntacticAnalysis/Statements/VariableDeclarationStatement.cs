﻿using DummyLang.LexicalAnalysis;
using DummyLang.SyntacticAnalysis.Abstractions;

namespace DummyLang.SyntacticAnalysis.Statements;

public sealed class VariableDeclarationStatement : IStatement
{
    internal const string IdentifierExpected     = "Identifier expected.";
    internal const string ColonExpected          = "Colon expected.";
    internal const string AssignmentExpected     = "Assignment expected.";
    internal const string ExpressionExpected     = "Expression expected.";
    internal const string TypeExpressionExpected = "Type expression expected.";
    internal const string SemicolonExpected      = "Semicolon expected at the end of a variable declaration statement.";

    public Token            DeclarationKeyword { get; }
    public Token            Identifier         { get; }
    public Token            TypeAssignment     { get; }
    public ITypeExpression? Type               { get; }
    public Token            ValueAssignment    { get; }
    public IExpression?     Value              { get; }
    public Token            Terminator         { get; }

    internal VariableDeclarationStatement(Token declarationKeyword,
                                          Token identifier,
                                          Token typeAssignment,
                                          ITypeExpression? type,
                                          Token valueAssignment,
                                          IExpression? value,
                                          Token terminator)
    {
        DeclarationKeyword = declarationKeyword;
        Identifier         = identifier;
        TypeAssignment     = typeAssignment;
        Type               = type;
        ValueAssignment    = valueAssignment;
        Value              = value;
        Terminator         = terminator;
    }

    public void Accept(ISyntaxNodeVisitor visitor) => visitor.Visit(this);
}