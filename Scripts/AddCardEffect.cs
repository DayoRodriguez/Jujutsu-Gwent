using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddCardEffect : MonoBehaviour , ICardEffect
{
    public GameObject cardSelectionPanel;  // Panel que contendrá las cartas
    public GameObject cardButtonPrefab;    // Prefab de un botón para cada carta
    public Transform cardListContainer;

    public GameObject card;
    public BoardManager board;
    public PlayerDeck decks;
    public GameObject ShowCardPanel;
    private CardEffects cardEffect;

    public void Execute(GameObject activingCard)
    {   
        Initialize();
        DisplayCard activingC = activingCard.GetComponent<DisplayCard>();
        //cardEffect = FindObjectOfType<CardEffects>();

        //card = cardEffect.activingCard;

        switch(activingC.card.effect)
        {
            case "AddSpecialCard" :
                Debug.Log("Activando el efecto de la carta " + activingC.card.name);
                if(activingC.card.owner == Card.Owner.Player)
                {
                    AddSpecialCard(board.transformDeck, board.transformPlayerHand);
                }
                else
                {
                    AddSpecialCard(board.opponentTransformDeck, board.opponentTransformPlayerHand);
                }
                break;
            case "AddSpecialCardDeck" :
                Debug.Log("Activando el efecto de la carta " + activingC.card.name);
                if(activingC.card.owner == Card.Owner.Player)
                {
                    AddSpecialCardDeck(board.transformDeck, board.transformPlayerHand);
                }
                else
                {
                    AddSpecialCardDeck(board.opponentTransformDeck, board.opponentTransformPlayerHand);
                }
                break;
            default :
                break;    
        }
    }

    public void Initialize()
    {
        //cardSelectionPanel = GameObject.Find("cardSelectionPanel");
        //cardListContainer = GameObject.Find("cardListContainer").GetComponent<Transform>();
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

    //Dominio's Expansion's effect
    public void AddSpecialCard(Transform origen, Transform hand)
    {
        DisplayCard selectedCard = SelectionCard(origen);
        if(!selectedCard.card.isUnit)
        {
            MovedCardToDestination(origen, hand, selectedCard.gameObject);
        }
    }

    //Maki Zennin's effect
    public void AddSpecialCardDeck(Transform origen, Transform hand)
    {
        DisplayCard selectedCard = SelectionCard(origen);
        if(!selectedCard.card.isUnit)
        {
            MovedCardToDestination(origen, hand, selectedCard.gameObject);
        }
    }

//-------Metodos basicos para utilizar en los efectos -----------------------------------------------------------------
    private void MovedCardToDestination(Transform origin, Transform destiantion, GameObject cardToMoved)
    {
        DisplayCard cardTM = cardToMoved.GetComponent<DisplayCard>();

        if(CardExistInOrigin(origin, cardTM))
        {
            if(destiantion.childCount < 10)
            {
                MovedCard(cardTM, destiantion);
            }
        }
        else 
        {
            Transform grav = destiantion == board.transformPlayerHand ? board.transformGraveyard : board.opponentTransformGraveyard;
            MovedCard(cardTM, grav);
        }
    }

    private void MovedCard(DisplayCard cardToMove, Transform destination)
    {
        if(cardToMove.gameObject.GetComponent<CardLogic>() == null)
        {
            cardToMove.gameObject.AddComponent<CardLogic>();
        }

        cardToMove.card.isActivated = true;
        cardToMove.SetUp(cardToMove.card);

        cardToMove.transform.SetParent(destination);
        cardToMove.transform.localScale = Vector3.one;
        cardToMove.transform.localPosition = Vector3.zero;
        cardToMove.transform.localRotation = Quaternion.identity;
    }

    private DisplayCard SelectionCard(Transform orig)
    {
        DisplayCard[] cardsInOrig = orig.GetComponentsInChildren<DisplayCard>();
        ShowCardToSelect(cardsInOrig);
        return null;   
    }

    private void ShowCardToSelect(DisplayCard[] cardsToShow)
    {
        cardSelectionPanel.SetActive(true);

        foreach(Transform child in cardListContainer)
        {
            Destroy(child);
        }

        foreach(DisplayCard c in cardsToShow)
        {
            GameObject cardButton = Instantiate(cardButtonPrefab, cardListContainer);

            cardButton.GetComponent<Text>().text = c.card.name;

            cardButton.GetComponent<Button>().onClick.AddListener(() => OnCardSelected(c));
        }
    }

    private void OnCardSelected(DisplayCard selectedC)
    {
        Transform ownerHand = selectedC.card.owner == Card.Owner.Player ? board.transformPlayerHand : board.opponentTransformPlayerHand;

        AddCardToHand(selectedC, ownerHand);

        cardSelectionPanel.SetActive(false);
    }

    private void AddCardToHand(DisplayCard selectedCard, Transform hand)
    {
        if(hand.childCount >= 10)
        {
            if(hand == board.transformPlayerHand)
            {
                selectedCard.transform.SetParent(board.transformGraveyard);
            }
            else
            {
                selectedCard.transform.SetParent(board.opponentTransformGraveyard);
            }
        }
        else 
        {
            selectedCard.transform.SetParent(hand);
        }

        selectedCard.transform.localPosition = Vector3.zero;
        selectedCard.transform.localScale = Vector3.one;
        selectedCard.transform.localRotation = Quaternion.identity;
        selectedCard.card.isActivated = true;
        selectedCard.SetUp(selectedCard.card);
    }


    private bool CardExistInOrigin(Transform origin, DisplayCard cardToLook)
    {
        DisplayCard[] cardsInOrigin = origin.GetComponents<DisplayCard>();
        if(cardsInOrigin.Length != 0)
        {
            foreach(DisplayCard c in cardsInOrigin)
            {
                if(c.card.name.Equals(cardToLook.card.name))
                {
                    return true;
                }
            }
            return false;
        }
        else return false;
    }
}
