using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnaryOperatorSyntax : Atom
{
    public Token operation;
    public IExpression right;
    public UnaryOperatorSyntax(IExpression right, Token operation)
    {
        this.right = right;
        this.operation = operation;
    }
}
