using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player
{
    public int power { get; set; }
    public int closePower { get; set; }
    public int rangePower { get; set; }
    public int siegePower { get; set; }
    public int cardsHand { get; set; }
    public int cardsDeck { get; set; }
    public int gems { get; set; }

    public Card.Owner owner;

    public GameComponent hand;
    public GameComponent deck;
    public GameComponent field;
    public GameComponent graveyard;
    public BoardManager board;


    public Player(Card.Owner p)
    {
        power = 0;
        closePower = 0;
        rangePower = 0;
        siegePower = 0;
        cardsHand = p == Card.Owner.Player? board.transformPlayerHand.GetComponentsInChildren<DisplayCard>().Length : board.opponentTransformPlayerHand.GetComponentsInChildren<DisplayCard>().Length;
        cardsDeck = p == Card.Owner.Player? board.transformDeck.GetComponentsInChildren<DisplayCard>().Length : board.opponentTransformDeck.GetComponentsInChildren<DisplayCard>().Length;;
        gems = 2;
        owner = p;
    }

    public Player Other()
    {
        if (this == GlobalContext.gameMaster.player1) return GlobalContext.gameMaster.player2;
        return GlobalContext.gameMaster.player1;
    }
}
