using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICardAtom : IExpression
{
    public void Set(Context context, List<Card> targets, Card card);
}
