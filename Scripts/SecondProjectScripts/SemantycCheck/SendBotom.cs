using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SendBottom : ArgumentMethod
{
    public SendBottom(IExpression list, IExpression card, Token accessToken) : base(list,card,accessToken){}


    public override void Execute(Context context, List<Card> targets)
    {
        list.Evaluate(context, targets);
        (list as List).gameComponent.SendBottom((Card)card.Evaluate(context, targets));
    }
}
