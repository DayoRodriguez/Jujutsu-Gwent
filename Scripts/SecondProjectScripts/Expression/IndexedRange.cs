using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndexedRange: IExpression
{
    public IExpression range;
    public IExpression index;   
    public Token indexedToken;
    
    public IndexedRange(IExpression range, IExpression index, Token indexedToken){
        this.range = range;
        this.index = index;
        this.indexedToken = indexedToken;
    }

    public object Evaluate(Context context, List<Card> targets){
        return (range.Evaluate(context,targets) as List<Card.Position>)[(int)index.Evaluate(context,targets)];
    }
}
