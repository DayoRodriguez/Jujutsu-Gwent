using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

[Serializable]
public class EffectDefinition : IASTNode
{
    public static readonly List<TokenType> moduleTypes= new List<TokenType>() {
        TokenType.Name, TokenType.Params,
        TokenType.Action, TokenType.ClosedBrace
    };
    public string name;
    public ParametersDefinition parameterDefs;
    public Action action;
    public Token keyword;

    public EffectDefinition() {}
    public void Execute()
    {
        action.Execute(action.context, action.targets);
    }
}

public enum ExpressionType
{
    Number, Bool, String, Card, List, RangeList, Player, Context, Targets, Null,
}