using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BinaryOperatorSyntax : IExpression
{
    public IExpression left;
    public IExpression right;
    public Token operation;

    protected BinaryOperatorSyntax(IExpression left, IExpression right, Token operation)
    {
        this.left = left;
        this.right = right;
        this.operation = operation;
    }
    public abstract object Evaluate(Context context, List<Card> targets);
}
