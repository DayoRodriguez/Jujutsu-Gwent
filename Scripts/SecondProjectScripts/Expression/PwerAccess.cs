using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerAccess : PropertyAccess
{
    public PowerAccess(IExpression card, Token accessToken) : base(card, accessToken) { }

    public override object Evaluate(Context context, List<Card> targets)
    {
        Card aux = (Card)card.Evaluate(context, targets);
        if (aux is FieldCard)
        {
            return (card.Evaluate(context, targets) as FieldCard).powers[3];
        }
        else throw new InvalidOperationException("Card doesn't contain power field");
    }

    public override void Set(Context context, List<Card> targets, object value)
    {
        (card.Evaluate(context, targets) as FieldCard).powers[1] = (int)value;
    }
}
