using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class And : BinaryOperatorSyntax
{
    public And(IExpression left, IExpression right, Token token) : base(left, right, token) { }

    public override object Evaluate(Context context, List<Card> targets)
    {
        return (bool)left.Evaluate(context, targets) && (bool)right.Evaluate(context, targets);
    }
}
