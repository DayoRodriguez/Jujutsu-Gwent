using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class CardLogic : MonoBehaviour, IPointerClickHandler
{   
    public Card card;
    private BoardManager boardManager;

    private Image cardImageOnBoard;

    void Start()
    {
        card = GetComponent<DisplayCard>().card;
        boardManager = FindObjectOfType<BoardManager>();

        cardImageOnBoard = boardManager.cardImageOnBoard;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(boardManager != null)
        {
            DisplayCard displayCard = GetComponent<DisplayCard>();

            if (displayCard.card.owner == Card.Owner.Player)
            {
                boardManager.selectedCard = this.gameObject;
                Debug.Log("Player card selected: " + this.gameObject.name);
                Debug.Log("La carta tiene un owner "+ displayCard.card.owner.ToString());
            }
            else if (displayCard.card.owner == Card.Owner.Opponent)
            {
                boardManager.opponentSelectedCard = this.gameObject;
                Debug.Log("Opponent card selected: " + this.gameObject.name);
                Debug.Log("La carta tiene un owner "+ displayCard.card.owner.ToString());
            }
            if(!card.HasBeenMulligan)
            {
                card.IsSelected = !card.IsSelected;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        cardImageOnBoard.GetComponent<UnityEngine.UI.Image>().sprite = GetComponent<DisplayCard>().card.spriteImage;
        cardImageOnBoard.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cardImageOnBoard.gameObject.SetActive(false);
    }
}
