using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Onactivation : IASTNode
{
    public static readonly List<TokenType> synchroTypes= new List<TokenType>() {TokenType.OpenBrace, TokenType.ClosedBracket, TokenType.ValueSeparator};
    public Onactivation(List<EffectActivation> activations)
    {
        this.activations = activations;
    }

    public List<EffectActivation> activations;

    public void Execute(Player triggerplayer)
    {   
        foreach (EffectActivation activation in activations)
        {
            activation.Execute(triggerplayer);
        }
    }
}
