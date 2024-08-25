using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardEffects : MonoBehaviour
{
    public GameObject activingCard;
    public BoardManager board;
    public Transform selectedRow;
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

    public void Excute(GameObject activingCard)
    {
        SetActivingCard(activingCard);
        DisplayCard activingC = activingCard.GetComponent<DisplayCard>();

        switch(activingC.card.cardEffects)
        {
            case Card.CardEffect.ChangeAttack :
                changePower.Execute(activingCard);
                break;
            case Card.CardEffect.SpecialSummon :
                specialSummon.Execute(activingCard);
                break;
            case Card.CardEffect.Active :
                activeCard.Execute(activingCard);
                break;
            case Card.CardEffect.Destroy :
                destroyCard.Execute(activingCard);
                break;
            case Card.CardEffect.Draw :
                drawCard.Execute(activingCard);
                break;
            case Card.CardEffect.TakeControl :
                takeControl.Execute(activingCard);
                break;
            case Card.CardEffect.Add :
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

    public bool NeedsRowSelection()
    {
        DisplayCard activingC = activingCard.GetComponent<DisplayCard>();

        // Definir si el tipo de efecto requiere seleccionar una fila
        switch (activingC.card.cardEffects)
        {
            case Card.CardEffect.ChangeAttack:
            case Card.CardEffect.Destroy:
            case Card.CardEffect.SpecialSummon:
                return true;  // Estos efectos requieren seleccionar una fila
            default:
                return false; // Otros efectos no requieren selección de fila
        }
    }

    // Nuevo método: verifica si el efecto necesita seleccionar una carta
    public bool NeedsCardSelection()
    {
        DisplayCard activingC = activingCard.GetComponent<DisplayCard>();

        // Definir si el tipo de efecto requiere seleccionar una carta
        switch (activingC.card.cardEffects)
        {
            case Card.CardEffect.TakeControl:
            case Card.CardEffect.Destroy:
            case Card.CardEffect.Add:
                return true;  // Estos efectos requieren seleccionar una carta
            default:
                return false; // Otros efectos no requieren selección de carta
        }
    }
}
