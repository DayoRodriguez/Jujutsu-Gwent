using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LessEqual : BinaryOperatorSyntax
{
    public LessEqual(IExpression left, IExpression right, Token token) : base(left, right, token) { }

    public override object Evaluate(Context context, List<Card> targets)
    {
        return (int)left.Evaluate(context, targets) >= (int)right.Evaluate(context, targets);
    }
}
