using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RangeAccess : PropertyAccess
{
    public static readonly List<TokenType> synchroTypes = new List<TokenType>() {TokenType.ValueSeparator, TokenType.ClosedBracket, TokenType.ClosedBrace};
    public RangeAccess(IExpression card, Token accessToken) : base(card, accessToken) { }

    public override object Evaluate(Context context, List<Card> targets)
    {
        Card aux = (Card)card.Evaluate(context, targets);

        List<Card.Position> positions = aux.positions;

        List<string> result =new List<string>();
        foreach (Card.Position position in positions){
            string add="";
            switch(position){
                case Card.Position.Melee: add="Melee"; break;
                case Card.Position.Ranged: add="Ranged"; break;
                case Card.Position.Siege: add="Siege"; break;
                //TODO: Imprimr error en consola
                default: throw new ArgumentException("Invalid  position");
            }
            result.Add(add);
        }
        return result;
    }

    public override void Set(Context context, List<Card> targets, object value)
    {
        (card.Evaluate(context, targets) as Card).positions = (List<Card.Position>)value;
    }
}
