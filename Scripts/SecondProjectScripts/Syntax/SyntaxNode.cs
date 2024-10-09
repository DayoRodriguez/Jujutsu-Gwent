using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SyntaxNode 
{
    private protected SyntaxNode(SyntaxTree syntaxTree)
    {
        SyntaxTree = syntaxTree;
    }

    public SyntaxTree SyntaxTree { get; }

    public SyntaxNode? Parent => SyntaxTree.GetParent(this);

    public abstract TokenType Type { get; }

    public virtual TextSpan Span
    {
        get
        {
            var first = GetChildren().First().Span;
            var last = GetChildren().Last().Span;
            return TextSpan.FromBounds(first.Start, last.End);
        }
    }

    public virtual TextSpan FullSpan
    {
        get
        {
            var first = GetChildren().First().FullSpan;
            var last = GetChildren().Last().FullSpan;
            return TextSpan.FromBounds(first.Start, last.End);
        }
    }

    public TextLocation Location => new TextLocation(SyntaxTree.Text, Span);

    public IEnumerable<SyntaxNode> AncestorsAndSelf()
    {
        var node = this;
        while (node != null)
        {
            yield return node;
            node = node.Parent;
        }
    }

    public IEnumerable<SyntaxNode> Ancestors()
    {
        return AncestorsAndSelf().Skip(1);
    }

    public abstract IEnumerable<SyntaxNode> GetChildren();

    public Token GetLastToken()
    {
        if (this is Token token)
            return token;

        // A syntax node should always contain at least 1 token.
        return GetChildren().Last().GetLastToken();
    }

    public void WriteTo(TextWriter writer)
    {
        PrettyPrint(writer, this);
    }

    private static void PrettyPrint(TextWriter writer, SyntaxNode node, string indent = "", bool isLast = true)
    {
        var isToConsole = writer == Console.Out;
        var token = node as Token;

        if (token != null)
        {
            foreach (var trivia in token.LeadingTrivia)
            {
                if (isToConsole)
                    Degub.ForegroundColor = ConsoleColor.DarkGray;

                writer.Write(indent);
                writer.Write("├──");

                if (isToConsole)
                    Console.ForegroundColor = ConsoleColor.DarkGreen;

                writer.WriteLine($"L: {trivia.Kind}");
            }
        }

        var hasTrailingTrivia = token != null && token.TrailingTrivia.Any();
        var tokenMarker = !hasTrailingTrivia && isLast ? "└──" : "├──";

        if (isToConsole)
            Console.ForegroundColor = ConsoleColor.DarkGray;

        writer.Write(indent);
        writer.Write(tokenMarker);

        if (isToConsole)
            Console.ForegroundColor = node is Token ? ConsoleColor.Blue : ConsoleColor.Cyan;

        writer.Write(node.Kind);

        if (token != null && token.Value != null)
        {
            writer.Write(" ");
            writer.Write(token.Value);
        }

        if (isToConsole)
            Console.ResetColor();

        writer.WriteLine();

        if (token != null)
        {
            foreach (var trivia in token.TrailingTrivia)
            {
                var isLastTrailingTrivia = trivia == token.TrailingTrivia.Last();
                var triviaMarker = isLast && isLastTrailingTrivia ? "└──" : "├──";

                if (isToConsole)
                    Console.ForegroundColor = ConsoleColor.DarkGray;

                writer.Write(indent);
                writer.Write(triviaMarker);

                if (isToConsole)
                    Console.ForegroundColor = ConsoleColor.DarkGreen;

                writer.WriteLine($"T: {trivia.Kind}");
            }
        }

        indent += isLast ? "   " : "│  ";

        var lastChild = node.GetChildren().LastOrDefault();

        foreach (var child in node.GetChildren())
            PrettyPrint(writer, child, indent, child == lastChild);
    }

    public override string ToString()
    {
        using (var writer = new StringWriter())
        {
            WriteTo(writer);
            return writer.ToString();
        }
    }
}
