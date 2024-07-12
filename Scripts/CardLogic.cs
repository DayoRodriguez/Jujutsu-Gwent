using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardLogic : MonoBehaviour
{   
    public Card card;
    private BoardManager boardManager;

    void Start()
    {
        card = GetComponent<DisplayCard>().card;
        boardManager = FindObjectOfType<BoardManager>();
    }

    void OnMouseDown()
    {
        if(boardManager != null)
        {
            boardManager.selectedCard = this.gameObject;
        }
    }

    /*public void PlayCard()
    {
        if(boardManager != null)
        {
            if(card != null && boardManager.selectedRow != null)
            {
                string[] types = card.GetKind();

                if(types[0] == "M" && boardManager.selectedRow == boardManager.tranformMeleeRow)
                {
                    boardManager.ActiveCard(this.gameObject, boardManager.selectedRow);      
                }
                if(types[1] == "R" && boardManager.selectedRow == boardManager.tranformRangedRow)
                {
                    boardManager.ActiveCard(this.gameObject, boardManager.selectedRow);      
                }
                if(types[2] == "S" && boardManager.selectedRow == boardManager.tranformSeigeRow)
                {
                    boardManager.ActiveCard(this.gameObject, boardManager.selectedRow);      
                }
            }
        }
    }*/
}
