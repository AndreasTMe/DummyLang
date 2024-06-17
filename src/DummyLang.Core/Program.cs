using DummyLang.SyntacticAnalysis;
using System;

string input;

do
{
    Console.Write("Enter the expression to parse: ");
    input = Console.ReadLine()?.Trim() ?? string.Empty;

    if (!string.IsNullOrEmpty(input))
    {
        var syntaxParser = new SyntaxParser();
        var tree = syntaxParser
            .Feed(input)
            .GenerateSyntax();

        tree.PrettyPrint();
        Console.WriteLine();
    }
} while (input != string.Empty);