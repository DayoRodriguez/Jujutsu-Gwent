using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MemberSyntax : SyntaxNode
{
    private protected MemberSyntax(SyntaxTree syntaxTree)
    : base(syntaxTree)
    {
    }
}
