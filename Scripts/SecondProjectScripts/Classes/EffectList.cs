using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EffectList
{
    public List<string> effects;

    public EffectList()
    {
        effects = new List<string>();
    }

    public void setEffect(string effect)
    {
        effects.Add(effect);
    }
}
