using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pop : Method, ICardAtom
{
    public Pop(IExpression list, Token accessToken) : base(list, accessToken) {}

    public object Evaluate(Context context, List<Card> targets)
    {
        List<Card> evaluation = list.Evaluate(context, targets) as List<Card>;
        if (evaluation.Count == 0) throw new Exception("Cannot Apply Pop method to empty list");
        Card result = evaluation[evaluation.Count - 1];
        Execute(context, targets);
        return result;
    }

    public override void Execute(Context context, List<Card> targets)
    {
        list.Evaluate(context, targets);
        (list as List).gameComponent.Pop();
    }

    public void Set(Context context, List<Card> targets, Card card) {}
}
