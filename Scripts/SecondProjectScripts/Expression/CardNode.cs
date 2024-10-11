using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardNode : IASTNode
{
    public static readonly List<TokenType> synchroTypes = new List<TokenType>() 
    {
        TokenType.Name, TokenType.Type , TokenType.Faction, TokenType.Power,
        TokenType.Range, TokenType.OnActivation, TokenType.ClosedBrace
    };

    public string name;
    public string faction;
    public Card.Types? type;
    public int? power;
    public List<string> position;
    public OnActivation activation;
    public Token keyword;
}
