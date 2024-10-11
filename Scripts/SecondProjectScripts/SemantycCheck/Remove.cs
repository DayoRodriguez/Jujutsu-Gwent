using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Remove : ArgumentMethod
{
    public Remove(IExpression list, IExpression card, Token accessToken) : base(list,card,accessToken){}


    public override void Execute(Context context, List<Card> targets)
    {
        list.Evaluate(context, targets);
        (list as List).gameComponent.Remove((Card)card.Evaluate(context, targets));
    }
}
