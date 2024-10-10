using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Literal : Atom
{
    public Literal(object value)
    {
        this.value = value;
    }

    public object value;

    public override object Evaluate(Context context, List<Card> targets)
    {
        return value;
    }
}
