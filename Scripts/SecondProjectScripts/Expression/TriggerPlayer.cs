using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPlayer : Atom
{
    public override object Evaluate(Context context, List<Card> targets)
    {
        return context.triggerplayer;
    }
}
