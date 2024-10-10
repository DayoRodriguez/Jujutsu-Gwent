using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Negation : UnaryOperatorSyntax
{
    public Negation(IExpression right, Token operation) : base(right, operation) { }

    public override object Evaluate(Context context, List<Card> targets)
    {
        return !(bool)right.Evaluate(context, targets);
    }
}
