using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

public static class Tools 
{
    public static Card.Types GetCardType(string s)
    {
        switch (s)
        {
            case "Oro": return Card.Types.Golden;
            // case "Plata": return Card.Type.Silver;
            case "Clima": return Card.Types.Weather;
            case "Aumento": return Card.Types.Increase;
            case "Líder": return Card.Types.Leader;
            // case "Señuelo": return Card.Type.Decoy;
            default: return Card.Types.Silver;
        }
    }    

    public static string GetCardTypeString(Card.Types? s)
    {
        switch (s)
        {
            case Card.Types.Golden: return "Oro";
            case Card.Types.Silver: return "Plata";
            case Card.Types.Weather: return "Clima";
            case Card.Types.Increase: return "Aumento";
            case Card.Types.Leader: return "Lider";
            // case Card.Type.Decoy: return "Señuelo";
            case Card.Types.Dump: return "Despeje";
            default: return null;
        }
    }  

    public static Dictionary<TKey, TValue> CopyDictionary<TKey,TValue>(Dictionary<TKey, TValue> dict)
    {
        Dictionary<TKey,TValue> copy = new Dictionary<TKey,TValue>();
        foreach(var pair in dict)
        {
            copy.Add(pair.Key, pair.Value);
        }
        return copy;
    }

    public static ExpressionType GetValueType(object value)
    {
        if (value is int) return ExpressionType.Number;
        if (value is string) return ExpressionType.String;
        if (value is bool) return ExpressionType.Bool;
        if (value is Card) return ExpressionType.Card;
        if (value is List<Card>) return ExpressionType.List;
        return ExpressionType.Null;
    }

    public static List<Card.Position> GetCardPositions(List<string> positions)
    {
        List<Card.Position> result = new List<Card.Position>();
        foreach (string position in positions.OrderBy(p => p))
        {
            switch(position)
            {
                case "Melee": result.Add(Card.Position.Melee); break;
                case "Ranged": result.Add(Card.Position.Ranged); break;;
                case "Siege": result.Add(Card.Position.Siege); break;
                default: throw new ArgumentException("Invalid string position");
            }
        }
        return result;
    }

    public static string GetCardRow(List<Card.Position> rows){
                  
        bool melee = false;
        foreach(Card.Position pos in rows)
        {
            if(pos == Card.Position.Melee) melee = true;
        }
        bool ranged = false;
        foreach(Card.Position pos in rows)
        {
            if(pos == Card.Position.Ranged) ranged = true;
        }
        bool siege = false;
        foreach(Card.Position pos in rows)
        {
            if(pos == Card.Position.Siege) siege = true;
        }
        string row = "";
        if(melee && ranged && siege) 
        {
            row = "all";
        }
        else if(melee && ranged)
        {
            row = "close_range";
        }
        else if(melee && siege)
        {
            row = "close_siege";
        }
        else if(ranged && siege)
        {
            row = "range_siege";
        }
        else if(melee)
        {
            row = "close";
        }
        else if(ranged)
        {
            row = "range";
        }
        else if(siege)
        {
            row = "siege";
        }
        return row;
    }


}