using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public static class GlobalContext
{
    public static BoardManager gameMaster;
    public static GameComponent Board{get => gameMaster.board;}   
 
    public static GameComponent Hand(Player player)
    {
        return player.hand;
    }
    public static GameComponent Deck(Player player)
    {
        return player.deck;
    }
    public static GameComponent Field(Player player)
    {
        return player.field;
    }
    public static GameComponent Graveyard(Player player)
    {
        return player.graveyard;
    }
}
