using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveCardEffect : MonoBehaviour , ICardEffect
{
    public GameObject cardSelectionPanel;  // Panel que contendrá las cartas
    public GameObject cardButtonPrefab;    // Prefab de un botón para cada carta
    public Transform cardListContainer;
    public GameObject cardPrefabs;

    public GameObject card;
    public BoardManager board;
    private CardEffects cardEffect;

    public void Execute(GameObject activingCard)
    {
        DisplayCard activingC = activingCard.GetComponent<DisplayCard>();

        Initialize();

        //cardEffect = FindObjectOfType<CardEffects>();

        //card = cardEffect.activingCard;

        switch(activingC.card.effect)
        {
            case "ActiveIncrease" :
                Debug.Log("Activando el efecto de la carta " + activingC.card.name);
                ActiveIncrease(activingCard);
                break;
            case "AcivateClimate" :  
                Debug.Log("Activando el efecto de la carta " + activingC.card.name);
                AcivateClimate(activingCard);
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

    public void ActiveCard(Transform originRow, string typeCard)
    {
        DisplayCard[] cards = originRow.GetComponentsInChildren<DisplayCard>();
        List<DisplayCard> cardsToChoose = new List<DisplayCard>();
        
        if(cards.Length != 0)
        {
            foreach(DisplayCard c in cards)
            {   
                if(!c.card.isUnit && c.card.GetKind()[0].Equals(typeCard))
                {
                    //Añadieno las cartas entre las que se van a elegir la carta a activar
                    cardsToChoose.Add(c);
                }
            }

            if(cardsToChoose.Count != 0)
            {
                ShowCardSelectionPanel(cardsToChoose);
            }
            else
            {
                ShowMessagePanel("No es posible activar la carta");
            }
        }
        else
        {
            ShowMessagePanel("No es posible activar la carta");
        }
    }

    private void ShowCardSelectionPanel(List<DisplayCard> cardsToChoose)
    {
        foreach(Transform child in cardListContainer)
        {
            Destroy(child.gameObject);
        }

        cardSelectionPanel.SetActive(true);

        foreach(DisplayCard c in cardsToChoose)
        {
            GameObject button = Instantiate(cardButtonPrefab, cardListContainer);
            button.GetComponentInChildren<UnityEngine.UI.Text>().text = " ";
            button.GetComponentInChildren<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>(c.card.name);

            button.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ActivateSelectedCard(c));
        }
    }

    private void ActivateSelectedCard(DisplayCard c)
    {
        cardSelectionPanel.SetActive(false);

        Transform destinationRow = c.card.owner == Card.Owner.Player
        ?board.GetPlayerRowForCard(c) 
        :board.GetOpponentRowForCard(c);

        GameObject cardObject = Instantiate(cardPrefabs);

        cardObject.transform.SetParent(destinationRow);
        cardObject.transform.localScale = Vector3.one;
        cardObject.transform.localPosition = Vector3.zero;
        cardObject.transform.localRotation = Quaternion.identity;
        
        c.card.isActivated = true;
        cardObject.GetComponent<DisplayCard>().SetUp(c.card);  

    }


    //Choso's effect
    public void ActiveIncrease(GameObject activingCard)
    {
        DisplayCard activingC = activingCard.GetComponent<DisplayCard>();

        if(activingC.card.owner == Card.Owner.Player)
        {
            ActiveCard(board.transformDeck, "Increase");
        }
        else 
        {
            ActiveCard(board.opponentTransformDeck, "Increase");            
        }
    }

    //Dagon's effect
    public void AcivateClimate(GameObject activingCard)
    {
        DisplayCard activingC = activingCard.GetComponent<DisplayCard>();

        if(activingC.card.owner == Card.Owner.Player)
        {
            ActiveCard(board.transformPlayerHand, "Climate");
        }
        else 
        {
            ActiveCard(board.opponentTransformPlayerHand, "Climate");
        }
    }

}
