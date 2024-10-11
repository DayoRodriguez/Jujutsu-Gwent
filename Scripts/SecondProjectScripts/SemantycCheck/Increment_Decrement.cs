using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Increment_Decrement : Assignation, IExpression
{
    public Increment_Decrement(IExpression operand, Token operation) : base(operand, null, operation){}
    
    public object Evaluate(Context context, List<Card> targets)
    {
        int result = (int)operand.Evaluate(context, targets);
        Execute(context, targets);
        return result;
    }

    public override void Execute(Context context, List<Card> targets)
    {
        int result = 0;
        if (operation.type == TokenType.Increment) result = (int)operand.Evaluate(context, targets) + 1;
        else result = (int)operand.Evaluate(context, targets) - 1;
        if (operand is PowerAccess) (operand as PowerAccess).Set(context, targets, result);
        else if (operand is Variable) context.Set((operand as Variable).name, result);
    }
}
