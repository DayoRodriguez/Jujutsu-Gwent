using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Method : IStatement
{
    public Method(IExpression list, Token accessToken)
    {
        this.list = list;
        this.accessToken = accessToken;
    }

    public Token accessToken;
    public IExpression list;
    public abstract void Execute(Context context, List<Card> targets);
}
