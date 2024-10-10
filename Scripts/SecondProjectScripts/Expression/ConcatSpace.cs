using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcatSpace : BinaryOperatorSyntax
{
    public ConcatSpace(IExpression left, IExpression right, Token token) : base(left, right, token) { }

    public override object Evaluate(Context context, List<Card> targets)
    {
        return (string)left.Evaluate(context, targets) + " " + (string)right.Evaluate(context, targets);
    }
}
