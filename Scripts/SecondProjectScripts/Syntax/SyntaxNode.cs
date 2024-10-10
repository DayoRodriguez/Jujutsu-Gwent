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

    public override string ToString()
    {
        using (var writer = new StringWriter())
        {
            WriteTo(writer);
            return writer.ToString();
        }
    }
}
