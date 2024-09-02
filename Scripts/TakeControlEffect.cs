using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeControlEffect : MonoBehaviour , ICardEffect
{
    public GameObject card;
    public BoardManager board;
    private CardEffects cardEffect;
    
    public void Execute(GameObject activingCard)
    {
        Initialize();
        DisplayCard activingC = activingCard.GetComponent<DisplayCard>();
        //cardEffect = FindObjectOfType<CardEffects>();

        //card = cardEffect.activingCard;

        switch(activingC.card.effect)
        {
            case "TakeControl" :
                Debug.Log("Activando el efecto de la carta " + activingC.card.name);
                TakeControl(activingCard);
                break;
            default :
                break;    
        }
    }

    public void Initialize()
    {
        board = FindObjectOfType<BoardManager>();
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

    //Doble Alma's effect
    public void TakeControl(GameObject activingCard)
    {
        DisplayCard activingC = activingCard.GetComponent<DisplayCard>();

        if(activingC.card.owner == Card.Owner.Player && 
        (ThereIsCard(board.opponentTransformSeigeRow) || ThereIsCard(board.opponentTransformRangedRow) || ThereIsCard(board.opponentTransformMeleeRow)))
        {
            board.SelectionCardIfNeeded
            (
                card => 
                {
                   DisplayCard cardToControl = card.GetComponent<DisplayCard>();

                   board.SelectionRowIfNeeded
                   (
                        row =>
                        {
                            Transform selectionRow = row;
                            cardToControl.transform.SetParent(selectionRow);

                            cardToControl.transform.localScale = Vector3.one;
                            cardToControl.transform.localPosition = Vector3.zero;
                            cardToControl.transform.localRotation = Quaternion.identity;
                        }
                   );        
                }
            );
                       
                        
        }
    }

    private bool ThereIsCard(Transform Row)
    {
        DisplayCard[] cardsInRow = Row.GetComponentsInChildren<DisplayCard>();
        if(cardsInRow.Length != 0) return true;
        else return false;
    }

}
