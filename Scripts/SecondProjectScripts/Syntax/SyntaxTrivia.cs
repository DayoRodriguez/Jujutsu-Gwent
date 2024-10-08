using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SyntaxTrivia
{
    internal SyntaxTrivia(SyntaxTree syntaxTree, TokenType type, int position, string text)
    {
        SyntaxTree = syntaxTree;
        Type = type;
        Position = position;
        Text = text;
    }

    public SyntaxTree SyntaxTree { get; }
    public TokenType Type { get; }
    public int Position { get; }
    public TextSpan Span => new TextSpan(Position, Text?.Length ?? 0);
    public string Text { get; }
}