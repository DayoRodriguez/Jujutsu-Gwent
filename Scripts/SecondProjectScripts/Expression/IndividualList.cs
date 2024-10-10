using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IndividualList : List
{
    //Este campo no se utiliza en el método de evaluación, es solo para la comprobación semántica.
    //Es por eso que en los casos en los que no se necesita una comprobación semántica, tendrá un valor nulo.
    public IExpression context;
    public Token playertoken;
    public IExpression player;
    
    public IndividualList(IExpression context, IExpression player, Token accessToken, Token playertoken) : base(accessToken)
    {
        this.context=context;
        this.player = player;
        this.playertoken = playertoken;
    }
}
