using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


public class Effect : IASTNode
{
    public static readonly List<TokenType> synchroTypes= new List<TokenType>() {TokenType.Identifier, TokenType.Name, TokenType.ClosedBrace, TokenType.ClosedBracket};
    public string definition;
    public Parameters parameters;
    public Token keyword;

    public void Execute(Player triggerplayer)
    {
        Dictionary<string, object> copy = Tools.CopyDictionary(parameters.parameters);
        Context rootContext = new Context(triggerplayer, null, copy);
        GlobalEffects.effects[definition].action.context = new Context(triggerplayer, rootContext, new Dictionary<string, object>());
        GlobalEffects.effects[definition].Execute();
    }
}
