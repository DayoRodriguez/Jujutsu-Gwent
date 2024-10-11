using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class While : Block
{
    public While(List<IStatement> statements, IExpression predicate, Token keyword) : base(statements, keyword)
    {
        this.predicate = predicate;
    }

    public IExpression predicate;

    public override void Execute(Context context, List<Card> targets)
    {
        this.context = new Context(context.triggerplayer, context, new Dictionary<string, object>());
        while ((bool)predicate.Evaluate(context, targets))
        {
            foreach (IStatement statement in statements)
            {
                statement.Execute(this.context, targets);
            }
        }
    }
}
