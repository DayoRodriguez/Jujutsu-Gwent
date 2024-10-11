using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shuffle : Method
{
    public Shuffle(IExpression list, Token accessToken) : base(list,accessToken) {}

    public override void Execute(Context context, List<Card> targets)
    {
        list.Evaluate(context, targets);
        (list as List).gameComponent.Shuffle();
    }
}