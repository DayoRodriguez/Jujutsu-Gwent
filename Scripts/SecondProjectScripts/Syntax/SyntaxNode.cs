using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SyntaxNode 
{
    public abstract TokenType Type{get;}
    public SyntaxTree SyntaxTree{get;}

    //public SyntaxNode ? Parent =>SyntaxTree => GetParent(this);

    public abstract IEnumerable<SyntaxNode> GetChildren();

    private protected SyntaxNode(SyntaxTree syntaxTree)
    {
        SyntaxTree = syntaxTree;
    }
}
