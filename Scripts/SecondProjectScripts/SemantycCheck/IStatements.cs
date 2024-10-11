using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatement : IASTNode
{
    public void Execute(Context context, List<Card> targets);
}
