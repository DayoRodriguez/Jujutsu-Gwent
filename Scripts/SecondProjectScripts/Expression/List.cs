using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class List : Atom 
{
    public List(Token accesToken)
    {
        this.accessToken = accesToken;
    }
    
    public Token accessToken;
    public GameComponent gameComponent;
}
