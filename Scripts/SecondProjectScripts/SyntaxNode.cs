using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class SyntaxNode 
{
    public abstract TokenType Type{get;}

    public abstract IEnumerable<SyntaxNode> GetChildren();
}
