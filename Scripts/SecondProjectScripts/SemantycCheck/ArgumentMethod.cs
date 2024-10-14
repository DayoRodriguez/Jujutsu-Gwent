using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class ArgumentMethod: Method
{
    public ArgumentMethod(IExpression list,IExpression card, Token accessToken) : base(list, accessToken)
    {
        this.card = card;
    }
    public IExpression card;
}