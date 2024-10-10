using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


public class Parameters 
{
    public static readonly List<TokenType> synchroTypes= new List<TokenType>() {TokenType.Identifier, TokenType.ClosedBrace};
    public Dictionary<string, object> parameters;
    public Parameters(Dictionary<string,object> parameters)
    {
        this.parameters=parameters;
    }
}
