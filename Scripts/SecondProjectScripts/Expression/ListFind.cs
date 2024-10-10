using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListFind : List
{
    public ListFind() : base(null){ }

    public ListFind(IExpression list, IExpression predicate, Token parameter, Token accessToken, Token argumentToken) : base(accessToken)
    {
        this.list = list;
        this.predicate = predicate;
        this.parameter = parameter;
        this.argumentToken = argumentToken;
    }

    public IExpression list;
    public IExpression predicate;
    public Token parameter;
    public Token argumentToken;

    public override object Evaluate(Context context, List<Card> targets)
    {
       //Guardar el valor de la variable si existe en el contexto
        object card = 0;
        List<Card> result = new List<Card>();
        bool usedvariable = false;
        Debug.Log(context.variables);
        if (context.variables.ContainsKey(parameter.lexeme))
        {   
            Debug.Log("Entra");
            card = context.variables[parameter.lexeme];
            usedvariable = true;
        }
        
        // Evalúa el predicado de cada carta de la lista
        foreach (Card listcard in (List<Card>)list.Evaluate(context, targets))
        {
            context.Set(parameter, listcard);
            if ((bool)predicate.Evaluate(context, targets)) result.Add(listcard);
        }

        // Restaurar el valor de la variable original si se utilizó
        if (usedvariable) context.Set(parameter, card);
        else context.variables.Remove(parameter.lexeme);

        return result;
    }
}
