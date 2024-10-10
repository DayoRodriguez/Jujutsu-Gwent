using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Different : BinaryOperatorSyntax
{
    public Different(IExpression left, IExpression right, Token token) : base(left, right, token) { }

    public override object Evaluate(Context context, List<Card> targets)
    {
        return left.Evaluate(context, targets) != right.Evaluate(context, targets);
    }
}