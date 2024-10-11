using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Assignation : IStatement
{
    public Assignation(IExpression operand, IExpression assignation, Token operation)
    {
        this.operand = operand;
        this.assignation = assignation;
        this.operation = operation;
    }

    public IExpression operand;
    public IExpression assignation;
    public Token operation;

    public virtual void Execute(Context context, List<Card> targets)
    {
        if (operand is ICardAtom) (operand as ICardAtom).Set(context, targets, assignation.Evaluate(context, targets) as Card);
        else if (operand is PropertyAccess) (operand as PropertyAccess).Set(context, targets, assignation.Evaluate(context,targets));
        else if (operand is Variable) context.Set((operand as Variable).name, assignation.Evaluate(context, targets));
    }
}