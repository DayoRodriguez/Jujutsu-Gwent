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
        if(card!= null)
        {
            if(!card.IsActivated) imageCard.sprite = card.cardBack;
            else imageCard.sprite = card.spriteImage;
        }
    }
}
