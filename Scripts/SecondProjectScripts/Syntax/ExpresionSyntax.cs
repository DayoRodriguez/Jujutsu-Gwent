using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ExpresionSyntax : SyntaxNode
{
    private protected ExpresionSyntax(SyntaxTree syntaxTree)
    : base(syntaxTree)
    {
        
    }   
}
