using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeck : MonoBehaviour
{
    public List<Card> deck = new List<Card>();
    public List<Card> opponentDeck = new List<Card>();
    private bool playerLeaderInstantiated = false;
    private bool opponentLeaderInstantiated = false;

    public GameObject Cards;

    public Transform playerDeck;
    public Transform OpponentPlayerDeck;

    public Transform PlayerHand;
    public Transform OpponentPlayerHand;

    public Transform LeaderCard;
    public Transform OpponentLeaderCard;

    // Start is called before the first frame update
    void Start()
    {
        DeckCreator("Magician", deck, ref playerLeaderInstantiated, LeaderCard);
        InstantiateDeck(deck, playerDeck); 
        
        DeckCreator("Curse", opponentDeck, ref opponentLeaderInstantiated, OpponentLeaderCard);  
        InstantiateDeck(opponentDeck, OpponentPlayerDeck); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DeckCreator(string factionDeck, List<Card> deck, ref bool leaderInstantiated, Transform leaderCardSlot)
    { 
        UnitCard cardLider = null;

        for(int i = 0; i < CardDataBase.cardData.Count; i++)
        {
            if(CardDataBase.cardData[i].isUnit) cardLider = (UnitCard)CardDataBase.cardData[i];
            else continue;
            
            if(cardLider.isLider && !leaderInstantiated && !deck.Contains(cardLider) && cardLider.faction.ToLower().Equals(factionDeck.ToLower()))
            { 
                GameObject leader = Instantiate(Cards, leaderCardSlot);
                leader.transform.localScale = Vector3.one;

                RectTransform leaderTransform = leader.GetComponent<RectTransform>();
                if(leaderTransform != null)
                {
                    leaderTransform.anchoredPosition3D = Vector3.zero;
                    leaderTransform.localScale = Vector3.one;
                }
                else
                {
                    leaderTransform.localScale = Vector3.one;
                }

                DisplayCard leaderDisplayCard = leader.GetComponent<DisplayCard>();
                if(leaderDisplayCard != null && leader != null)
                {
                    cardLider.IsActivated = true;
                    leaderDisplayCard.SetUp(cardLider);
                }
                leaderInstantiated = true;
                    //deck.Add(cardLider);
                break;
            }    
        }
        for(int j = 0; j < 1000 && deck.Count < 25; j++)
        {
            int counter = Random.Range(0,CardDataBase.cardData.Count);
            Card auxCard = CardDataBase.cardData[counter];                       
            if(!auxCard.isLider && (auxCard.faction.ToLower().Equals(cardLider.faction.ToLower()) 
            || auxCard.faction.ToLower().Equals("neutral")))
            {
                if(!deck.Contains(auxCard))
                {
                    deck.Add(auxCard);
                } 
                else if(!auxCard.isSpecial && Count(auxCard, deck) < 3) 
                {
                    deck.Add(auxCard);
                }    
            }
            else continue;
        }
    }

    private void InstantiateDeck(List<Card> deck, Transform CardsInDeck)
    {
        foreach(Transform child in CardsInDeck)
        {
            Destroy(child.gameObject);
        }

        foreach(Card c in deck)
        {
           
            GameObject cardinDeck = Instantiate(Cards, CardsInDeck);
            cardinDeck.transform.localScale = Vector3.one;

            RectTransform rectTransform = cardinDeck.GetComponent<RectTransform>();
            if(rectTransform != null)
            {
                rectTransform.anchoredPosition3D = Vector3.zero;
                rectTransform.localScale = Vector3.one;
            }
            else
            {
                cardinDeck.transform.localPosition = Vector3.zero;
            } 

            DisplayCard displayCard = cardinDeck.GetComponent<DisplayCard>();
            if(displayCard != null && c != null)
            {
                c.IsActivated = false;
                displayCard.SetUp(c);
            }
            else Debug.Log("La carta no fue cargada");
        }
    }
    
    public static int Count(Card card, List<Card> deck)
    {
        int count = 0;
        for(int i = 0; i < deck.Count; i++)
        {
            if(deck.Contains(card)) count++;
        }
        return count;
    }

}
