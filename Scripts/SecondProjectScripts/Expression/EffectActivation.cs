using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class EffectActivation : IASTNode
{
    public static readonly List<TokenType> synchroTypes= new List<TokenType>() {TokenType.Effect, TokenType.Selector, TokenType.PostAction, TokenType.ClosedBrace, TokenType.ClosedBracket};
    public Effect effect;
    public Selector selector;
    public EffectActivation postAction;

    public void Execute(Player triggerplayer)
    {   
        
        if(selector != null){
            switch (selector.source.literal)
            {
                case "board": selector.filtre.list = new BoardList(null,null); break;
                case "hand": selector.filtre.list = new HandList(null,new Literal(triggerplayer), null, null); break;
                case "otherHand": selector.filtre.list = new HandList(null,new Literal(triggerplayer.Other()), null, null); break;
                case "deck": selector.filtre.list = new DeckList(null,new Literal(triggerplayer), null, null); break;
                case "otherDeck": selector.filtre.list = new DeckList(null,new Literal(triggerplayer.Other()), null, null); break;
                case "graveyard": selector.filtre.list = new GraveyardList(null,new Literal(triggerplayer), null, null); break;
                case "otherGraveyard": selector.filtre.list = new GraveyardList(null,new Literal(triggerplayer.Other()), null, null); break;
                case "field": selector.filtre.list = new FieldList(null,new Literal(triggerplayer), null, null); break;
                case "otherField": selector.filtre.list = new FieldList(null,new Literal(triggerplayer.Other()), null, null); break;
            }
            // if (postAction.selector == null) postAction.selector = selector;
            // else if ((string)postAction.selector.source.literal == "parent") postAction.selector.filtre.list = selector.filtre;
            var temp = selector.Select(triggerplayer);
            if ((bool)selector.single && temp.Count > 0)
            {
                List<Card> singlecard = new List<Card>() { temp[0] };
                GlobalEffects.effects[effect.definition].action.targets = singlecard;
            }
            else GlobalEffects.effects[effect.definition].action.targets = temp;
        }
        else GlobalEffects.effects[effect.definition].action.targets=new List<Card>();
        effect.Execute(triggerplayer);
        // postAction.Execute(triggerplayer);
    }
}
