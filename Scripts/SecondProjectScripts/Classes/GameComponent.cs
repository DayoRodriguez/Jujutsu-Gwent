using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public abstract class GameComponent
{
    public Player owner;
    public List<Card> cards = new List<Card>();
    public abstract void Push(Card card);
    public abstract Card Pop();
    public abstract void SendBottom(Card card);
    public abstract void Remove(Card card);
    public int Size {get => cards.Count;}
    public void Shuffle()
    {
        for (int i=cards.Count-1;i>0;i--)
        {
            int randomIndex=UnityEngine.Random.Range(0,i+1);
            Card container=cards[i];
            cards[i]=cards[randomIndex];
            cards[randomIndex]=container;
        }
    }
}
