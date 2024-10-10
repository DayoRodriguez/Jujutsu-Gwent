using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveyardList : IndividualList
{
    public GraveyardList(IExpression context,IExpression player, Token accessToken, Token playertoken) : base(context, player, accessToken, playertoken) { }

    public override object Evaluate(Context context, List<Card> targets)
    {
        Player targetPlayer = (Player)player.Evaluate(context, targets);
        gameComponent = GlobalContext.Graveyard(targetPlayer);
        return gameComponent.cards;    
    }
}
