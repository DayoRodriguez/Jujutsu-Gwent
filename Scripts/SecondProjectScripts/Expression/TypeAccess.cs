using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeAccess : PropertyAccess
{
    public TypeAccess(IExpression card, Token accessToken) : base(card, accessToken) { }

    public override object Evaluate(Context context, List<Card> targets)
    {
        Card aux = (Card)card.Evaluate(context, targets);
        return aux.type;
    }

    public override void Set(Context context, List<Card> targets, object value)
    {   
        (card.Evaluate(context, targets) as Card).type = Tools.GetCardType((string)value);
    }
}
