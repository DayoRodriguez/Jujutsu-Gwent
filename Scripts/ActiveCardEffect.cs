using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveCardEffect : MonoBehaviour , ICardEffect
{
    public GameObject card;
    public BoardManager board;
    private CardEffects cardEffect;

    public void Execute(GameObject activingCard)
    {
        DisplayCard activingC = activingCard.GetComponent<DisplayCard>();

        cardEffect = FindObjectOfType<CardEffects>();

        card = cardEffect.activingCard;

        switch(activingC.card.effect)
        {
            case "ActiveIncrease" :
                ActiveIncrease(activingCard);
                break;
            case "AcivateClimate" :  
                AcivateClimate(activingCard);
                break;
            default :
                break;      
        }
    }

    public void Initialize(Card card)
    {

    }

    public void ShowMessagePanel(string sms)
    {
        Debug.Log(sms);
    }

    public bool CanActive()
    {
        //Revisar la Implementaciob de este metodo
        return true;
    }

    public void EndEffect(GameObject activingCard)
    {
        DisplayCard activingC = activingCard.GetComponent<DisplayCard>();

        if(!activingC.card.isUnit && (activingC.card.GetKind()[0] == "Increase"))
        {
            Transform grav = activingC.card.owner == Card.Owner.Player ? board.transformGraveyard : board.opponentTransformGraveyard;
            board.CleanRow(activingCard.transform.parent, grav);
        }
    }

    public void ActiveCard(Transform originRow, string typeCard)
    {
        DisplayCard[] cards = originRow.GetComponentsInChildren<DisplayCard>();
        List<DisplayCard> cardsToChoose = new List<DisplayCard>();
        
        if(cards.Length != 0)
        {
            foreach(DisplayCard c in cards)
            {   
                if(!c.card.isUnit && c.card.GetKind()[0].Equals(typeCard))
                {
                    //Añadieno las cartas entre las que se van a elegir la carta a activar
                    cardsToChoose.Add(c);
                }
            }

            if(cardsToChoose.Count != 0)
            {
                //implementar codigo para mostrar un panel con las cartas a elegir y seleccionarla
                //ademas de implementar la seleccionde la destiny Row
            }
            else
            {
                ShowMessagePanel("No es posible activar la carta");
            }
        }
        else
        {
            ShowMessagePanel("No es posible activar la carta");
        }
    }


    //Choso's effect
    public void ActiveIncrease(GameObject activingCard)
    {
        DisplayCard activingC = activingCard.GetComponent<DisplayCard>();

        if(activingC.card.owner == Card.Owner.Player)
        {
            ActiveCard(board.transformDeck, "Increase");
        }
        else 
        {
            ActiveCard(board.opponentTransformDeck, "Increase");            
        }
    }

    //Dagon's effect
    public void AcivateClimate(GameObject activingCard)
    {
        DisplayCard activingC = activingCard.GetComponent<DisplayCard>();

        if(activingC.card.owner == Card.Owner.Player)
        {
            ActiveCard(board.transformPlayerHand, "Climate");
        }
        else 
        {
            ActiveCard(board.opponentTransformPlayerHand, "Climate");
        }
    }

}
