using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersActions : MonoBehaviour
{
    public GameManager gameManager;
    public BoardManager boardManager;

void Start()
{
    gameManager = FindObjectOfType<GameManager>();
    boardManager = FindObjectOfType<BoardManager>();
}
    public void ActivePlayerCard()
    {
        if(gameManager.actualState == GameManager.GameState.PlayerTurn)
        {
            if(boardManager.selectedCard != null && boardManager.selectedRow !=null)
            {
                    if(boardManager.selectedCard.transform.parent == boardManager.transformPlayerHand)
                    {
                        boardManager.HandleCardActivation(boardManager.selectedCard, boardManager.selectedRow);
                        boardManager.selectedCard = null;
                        boardManager.selectedRow = null;
                    }

                // boardManager.selectedCard = null;
                // boardManager.selectedRow = null;
            }
        }
    }

    public void ActiveOpponentCard()
    {
        if(gameManager.actualState == GameManager.GameState.OpponentTurn)
        {
            if(boardManager.opponentSelectedCard != null && boardManager.opponentSelectedRow !=null)
            {
                    if(boardManager.opponentSelectedCard.transform.parent == boardManager.opponentTransformPlayerHand)
                    {
                        boardManager.HandleCardActivation(boardManager.opponentSelectedCard, boardManager.opponentSelectedRow);
                        boardManager.opponentSelectedCard = null;
                        boardManager.opponentSelectedRow = null;   
                    }          
                // boardManager.opponentSelectedCard = null;
                // boardManager.opponentSelectedRow = null;
            }
        }
    }
    public void ActiveLiderEffect()
    {

    }
    public void PlayerPass()
    {
        if(gameManager.actualState == GameManager.GameState.PlayerTurn)
        {
            gameManager.PlayerPass();
        }
    }

    public void OpponentPass()
    {
        if(gameManager.actualState == GameManager.GameState.OpponentTurn)
        {
            gameManager.OpponentPass();
        }
    }
}
