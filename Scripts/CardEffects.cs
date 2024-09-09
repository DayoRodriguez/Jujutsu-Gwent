using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardEffects : MonoBehaviour
{
    public GameObject activingCard;
    public BoardManager board;
    public Transform selectedRow;
    public GameObject effectSelectedCard;
    public ChangePowerEffect changePower;
    public SpecialSummonEffect specialSummon;
    public ActiveCardEffect activeCard;
    public DestroyCardEffect destroyCard;
    public DrawCardEffect drawCard;
    public TakeControlEffect takeControl;
    public AddCardEffect addCard;

    void Start()
    {
        if (board == null)
        {
            board = FindObjectOfType<BoardManager>(); 
            if (board == null)
            {
                Debug.LogError("BoardManager no ha sido encontrado por ActiveEffectCard.");
            }
        }

        changePower = FindObjectOfType<ChangePowerEffect>();
        specialSummon = FindObjectOfType<SpecialSummonEffect>();
        activeCard = FindObjectOfType<ActiveCardEffect>();
        destroyCard = FindObjectOfType<DestroyCardEffect>();
        drawCard = FindObjectOfType<DrawCardEffect>();
        takeControl = FindObjectOfType<TakeControlEffect>();
        addCard = FindObjectOfType<AddCardEffect>();
    }

    public void Execute(GameObject activingCard)
    {
        SetActivingCard(activingCard);
        DisplayCard activingC = activingCard.GetComponent<DisplayCard>();

        switch(activingC.card.cardEffects)
        {
            
            case Card.CardEffect.ChangeAttack :
                Debug.Log("Entramos en el switch");
                changePower.Execute(activingCard);
                break;
            case Card.CardEffect.SpecialSummon :
                Debug.Log("Entramos en el switch");
                specialSummon.Execute(activingCard);
                break;
            case Card.CardEffect.Active :
                Debug.Log("Entramos en el switch");
                activeCard.Execute(activingCard);
                break;
            case Card.CardEffect.Destroy :
                Debug.Log("Entramos en el switch");
                destroyCard.Execute(activingCard);
                break;
            case Card.CardEffect.Draw :
                Debug.Log("Entramos en el switch");
                drawCard.Execute(activingCard);
                break;
            case Card.CardEffect.TakeControl :
                Debug.Log("Entramos en el switch");
                takeControl.Execute(activingCard);
                break;
            case Card.CardEffect.Add :
                Debug.Log("Entramos en el switch");
                addCard.Execute(activingCard);
                break;
            default : 
                break;                          
        }
    }

    public void SetActivingCard(GameObject card)
    {
        activingCard = card;
        Debug.Log("La carta ha sido asignada correctamente");
    }
}
