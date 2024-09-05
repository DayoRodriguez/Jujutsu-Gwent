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
    public GameObject cardPrefabs;

    public BoardManager board;
    private CardEffects cardEffect;

    public void Execute(GameObject activingCard)
    {   
        Initialize();
        DisplayCard activingC = activingCard.GetComponent<DisplayCard>();

        switch(activingC.card.effect)
        {
            case "AddSpecialCard" :
                Debug.Log("Activando el efecto de la carta " + activingC.card.name);
                Transform deckOrigen = activingC.card.owner == Card.Owner.Player ? board.transformDeck : board.opponentTransformDeck;
                Transform gravOrigen = activingC.card.owner == Card.Owner.Player ? board.transformGraveyard : board.opponentTransformGraveyard;
                Transform[] origenes = {deckOrigen, gravOrigen};
                AddSpecialCard(activingC, origenes);
                EndEffect(activingCard);
                break;
            case "AddSpecialCardDeck" :
                Debug.Log("Activando el efecto de la carta " + activingC.card.name);
                Transform origen = activingC.card.owner == Card.Owner.Player ? board.transformDeck : board.opponentTransformDeck;
                AddSpecialCardDeck(activingC, origen);
                EndEffect(activingCard);
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

    //Dominio's Expansion's effect
    public void AddSpecialCard(DisplayCard activingC, Transform[] origenes)
    {
        List<DisplayCard> selectedCards = new List<DisplayCard>();
        foreach(Transform t in origenes)
        {
            foreach(DisplayCard c in GetCards(t, false)) selectedCards.Add(c);
        }
        if(selectedCards.Count != 0)
        {
            foreach(DisplayCard c in selectedCards)
            {
                c.card.owner = activingC.card.owner;
            }
            ShowCardToAdd(selectedCards);
        }
        else Debug.Log("NO SE PUEDE ACTIVAR EL EFFECTO");
    }

    //Maki Zennin's effect
    public void AddSpecialCardDeck(DisplayCard activingC, Transform origen)
    {
        List<DisplayCard> selectedCards = GetCards(origen, false);
        if(selectedCards.Count != 0)
        {
            foreach(DisplayCard c in selectedCards)
            {
                c.card.owner = activingC.card.owner;
            }
            ShowCardToAdd(selectedCards);
        } 
        else Debug.Log("NO SE PUEDE ACTIVAR EL EFFECTO");
    }

//-------Metodos basicos para utilizar en los efectos -----------------------------------------------------------------
    private List<DisplayCard> GetCards(Transform orig, bool b)
    {
        List<DisplayCard> cardsToAdd = new List<DisplayCard>();
        DisplayCard[] cards = orig.GetComponentsInChildren<DisplayCard>();

        foreach(DisplayCard c in cards)
        {
            if(b && c.card.isUnit) cardsToAdd.Add(c);
            else if(!c.card.isUnit) cardsToAdd.Add(c);
        }
        
        return cardsToAdd;
    }

    private void ShowCardToAdd(List<DisplayCard> cardsToAdd)
    {
        foreach(Transform child in cardListContainer)
        {
            Destroy(child.gameObject);
        }

        cardSelectionPanel.SetActive(true);

        foreach(DisplayCard c in cardsToAdd)
        {
            GameObject cardB = Instantiate(cardButtonPrefab, cardListContainer);
            cardB.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>(c.card.name);
            cardB.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => OnCardSelected(c));
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
        selectedCard.gameObject.transform.localPosition = Vector3.zero;
        selectedCard.gameObject.transform.localScale = Vector3.one;
        selectedCard.gameObject.transform.localRotation = Quaternion.identity;

        if(hand.childCount >= 10)
        {
            if(hand == board.transformPlayerHand)
            {
                selectedCard.gameObject.transform.SetParent(board.transformGraveyard);
            }
            else
            {
                selectedCard.gameObject.transform.SetParent(board.opponentTransformGraveyard);
            }
        }
        else 
        {
            selectedCard.gameObject.transform.SetParent(hand);
        }

        selectedCard.card.isActivated = true;
        selectedCard.SetUp(selectedCard.card);
    }
}
