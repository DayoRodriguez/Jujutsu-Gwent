using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IExpression : IASTNode
{
    public object Evaluate(Context context, List<Card> targets);
}
