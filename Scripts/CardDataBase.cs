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

    void Awake()
    {
        LoadCard();
    }

    void LoadCard()
    {
        Card[] cards = Resources.LoadAll<Card>(cardFolder);
        foreach(Card card in cards)
        {
            cardData.Add(card);
            Debug.Log("La carta ha sido cargada " + card.name);
        }
        Debug.Log("La cardData ha sido cargada " + cardData.Count);
    }

}
