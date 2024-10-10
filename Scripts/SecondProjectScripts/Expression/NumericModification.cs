using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumericModification : Assignation
{
    public NumericModification(IExpression operand, IExpression assignation, Token operation) : base(operand, assignation ,operation){}

    public override void Execute(Context context, List<Card> targets)
    {
        object result = null;
        switch (operation.type)
        {
            case TokenType.AddEqual: result = (int)operand.Evaluate(context, targets) + (int)assignation.Evaluate(context, targets); break;
            case TokenType.SubEqual: result = (int)operand.Evaluate(context, targets) - (int)assignation.Evaluate(context, targets); break;
            case TokenType.DivEqual: result = (int)operand.Evaluate(context, targets) * (int)assignation.Evaluate(context, targets); break;
            case TokenType.MulEqual: result = (int)operand.Evaluate(context, targets) / (int)assignation.Evaluate(context, targets); break;
            case TokenType.ConcatEqual: result = (string)operand.Evaluate(context, targets) + (string)assignation.Evaluate(context, targets); break;
        }
        if (operand is PowerAccess) (operand as PowerAccess).Set(context, targets, result);
        else if (operand is Variable) context.Set((operand as Variable).name, result);
    }
}
