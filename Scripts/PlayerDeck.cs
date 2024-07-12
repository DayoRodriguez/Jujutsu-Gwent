using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeck : MonoBehaviour
{
    public List<Card> deck = new List<Card>();
    public List<Card> opponentDeck = new List<Card>();

    public GameObject CardsDeck;
    public GameObject OpponentCardsDeck;

    public Transform PayerDeck;
    public Transform OpponentPayerDeck;

    public Transform PlayerHand;
    public Transform OpponentPlayerHand;

    // Start is called before the first frame update
    void Start()
    {
        DeckCreator("Magician");   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DeckCreator(string factionDeck)
    { 
        UnitCard cardLider = new UnitCard();

        for(int i = 0; i < CardDataBase.cardData.Count; i++)
        {
            if(CardDataBase.cardData[i].isUnit) cardLider = (UnitCard)CardDataBase.cardData[i];
            else continue;
            
            if(cardLider.isLider && !deck.Contains(cardLider) && cardLider.faction.ToLower().Equals(factionDeck.ToLower()))
            {
                deck.Add(cardLider);
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
