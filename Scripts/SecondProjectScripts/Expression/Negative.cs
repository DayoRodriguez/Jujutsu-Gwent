using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Negative : UnaryOperatorSyntax
{
    public Negative(IExpression right, Token operation) : base(right, operation) { }

    public override object Evaluate(Context context, List<Card> targets)
    {
        return -(int)right.Evaluate(context, targets);
    }
}
