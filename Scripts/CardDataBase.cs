using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.IO;
using UnityEditor.PackageManager;
using System.Net.NetworkInformation;


public class CardDataBase : MonoBehaviour
{
    // [SerializeField] private Card[] cardData = new Card[35];
    
    // public Card[] CardData { get { return cardData; } private set{ cardData = CardData;}}


    // public Card[] GetCardData()

    // {

    //     return CardData;

    // }

    // public void Awake()
    // {
    //     Card[] cardsSO = Resources.FindObjectsOfTypeAll<Card>();

    //     for(int i = 0; i < cardsSO.Length; i++)
    //     {
    //         CardData[i] = cardsSO[i];
    //     }
    // }


    string cardFolder = "Cards";
    public static List<Card> cardData = new List<Card>();
    public static List<Card> tokenData = new List<Card>();
    public static List<Card> leaderData = new List<Card>();
    void Awake()
    {
        LoadCard();
    }

    void LoadCard()
    {
        Card[] cards = Resources.LoadAll<Card>(cardFolder);
        foreach(Card card in cards)
        {
            if(card.isLider)
            {
                leaderData.Add(card);
            }
            else if(!card.name.Contains("Token"))
            {
                cardData.Add(card);
                Debug.Log("La carta ha sido cargada " + card.name);
            }
            else
            {
                tokenData.Add(card);
                Debug.Log("La token ha sido cargada " + card.name);
                Debug.Log(tokenData.Count);
            }
        }
        Debug.Log("La cardData ha sido cargada " + cardData.Count);
    }

}
