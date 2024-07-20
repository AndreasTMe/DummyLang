using DummyLang.SyntacticAnalysis;
using System;

string input;

do
{
    Console.Write("Enter the expression to parse: ");
    input = Console.ReadLine()?.Trim() ?? string.Empty;

    if (!string.IsNullOrEmpty(input))
    {
        var syntaxParser = new SyntaxTreeBuilder();
        var tree = syntaxParser.Feed(input)
                               .Build();
        
        tree.ValidateSyntax();
    }
} while (input != string.Empty);