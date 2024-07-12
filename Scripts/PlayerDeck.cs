using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeck : MonoBehaviour
{
    public List<Card> deck = new List<Card>();

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
        //Debug.Log(CardsData.Length); 
        //deck = new List<Card>();
        UnitCard cardLider = new UnitCard();
        EspecialCard specialCard = new EspecialCard();
        //Card auxCard = new Card();
        //Debug.Log(cardsData.Length);
        //for(int i = 0; i < CardsData.Length; i++) Debug.Log(cardsData[i]);
        for(int i = 0; i < CardDataBase.cardData.Count; i++)
        {
            cardLider = (UnitCard)CardDataBase.cardData[i];
            if(cardLider.isLider)
             {
                Debug.Log(CardDataBase.cardData[i]);
                Debug.Log(cardLider.name);
             }
            
            if(cardLider.isLider && !deck.Contains(cardLider) && cardLider.faction.ToLower().Equals(factionDeck.ToLower()))
            {
                deck.Add(cardLider);
                //cardLider = CardDataBase.cardData[i];
                Debug.Log(cardLider.name);
                break;
            }
        }
        int n = 1;
        while(n < 25)
        {
            int counter = Random.Range(0,CardDataBase.cardData.Count);
            Card auxCard = CardDataBase.cardData[counter];                       
            if(!auxCard.isLider && (auxCard.faction.ToLower().Equals(cardLider.faction.ToLower()) 
            || auxCard.faction.ToLower().Equals("neutral")))
            {
                if(!deck.Contains(auxCard))
                {
                    deck.Add(auxCard);
                    n++;
                } 
                else if(!auxCard.isSpecial && Count(auxCard, deck) < 3) 
                {
                    deck.Add(auxCard);
                    n++;
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
