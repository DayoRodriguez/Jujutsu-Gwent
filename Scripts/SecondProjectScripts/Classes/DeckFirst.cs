using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DeckFirst
{
    public List<Card> cards;

    public DeckFirst()
    {
        // Constructor: Inicializa la lista de cartas
        cards = new List<Card>();
    }

    public void Shuffle()
    {   
        // Método para barajar las cartas en el mazo
        System.Random rng = new System.Random();
        int n = cards.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Card value = cards[k];
            cards[k] = cards[n];
            cards[n] = value;
        }
    }

    public Card GetCard(int x = 0)
    {
        // Método para obtener una carta del mazo por su índice
        return cards[x];
    }

    public void SetCard(int x, Card card)
    {
        // Método para establecer una carta en una posición específica del mazo
        cards[x] = card;
    }

    public void AddCard(Card card)
    {   
        //Agregar carta
        cards.Add(card);
    }

    public void RemoveCard(int x)
    {
        // Método para eliminar una carta del mazo por su índice
        cards.RemoveAt(x);
    }
}
