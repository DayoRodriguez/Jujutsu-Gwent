using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardList : List
{
    public BoardList(IExpression context, Token accessToken) : base(accessToken){
        this.context=context;
    }
    public IExpression context;
    public override object Evaluate(Context context, List<Card> targets)
    {
        return GlobalContext.Board.cards;
    }
}
