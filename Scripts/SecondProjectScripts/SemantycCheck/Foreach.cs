using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Foreach : Block
{
    public Foreach(List<IStatement> statements, IExpression collection, Token variable, Token keyword) : base(statements, keyword)
    {
        this.collection = collection;
        this.variable = variable;
    }

    public Token variable;
    public IExpression collection;

    public override void Execute(Context context, List<Card> targets)
    {
        this.context = new Context(context.triggerplayer, context, new Dictionary<string, object>());

        foreach (Card card in (List<Card>)collection)
        {
            this.context.Set(variable, card);
            foreach (IStatement statement in statements)
            {
                statement.Execute(this.context, targets);
            }
        }
    }
}
