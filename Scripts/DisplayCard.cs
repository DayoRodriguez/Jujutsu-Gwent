using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DisplayCard : MonoBehaviour
{
    public Card card;
    private Image imageCard;

    void Awake()
    {
        imageCard = GetComponent<Image>();
    }

    void Start()
    {

    }
    public void SetUp(Card c)
    {
        if(c != null)
        {
            card = c;
            if(imageCard == null) imageCard = GetComponent<Image>();
            if(!c.isActivated) imageCard.sprite = c.cardBack;
            else imageCard.sprite = c.spriteImage;
        }
    }
}
