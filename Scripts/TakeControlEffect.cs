using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class TakeControlEffect : MonoBehaviour , ICardEffect
{
    public GameObject cardSelectionPanel;  // Panel que contendrá las cartas
    public GameObject cardButtonPrefab;    // Prefab de un botón para cada carta
    public Transform cardListContainer;
    public GameObject cardPrefabs;

    public GameObject card;
    public BoardManager board;
    public Transform effectRow;
    private CardEffects cardEffect;

    void Start()
    {
        cardEffect = FindObjectOfType<CardEffects>();
        board = FindObjectOfType<BoardManager>();
    }
    public void Execute(GameObject activingCard)
    {
        DisplayCard activingC = activingCard.GetComponent<DisplayCard>();

        switch(activingC.card.effect)
        {
            case "TakeControl" :
                Debug.Log("Activando el efecto de la carta " + activingC.card.name);
                List<DisplayCard> cardsToControl = new List<DisplayCard>();
                Transform[] rowsRivals = GetRows(activingC.card.owner == Card.Owner.Player ? false : true);
                foreach(Transform t in rowsRivals)
                {
                    foreach(DisplayCard c in GetCards(t,true)) cardsToControl.Add(c);
                }
                if(cardsToControl.Count != 0) ShowCardToControl(cardsToControl);
                EndEffect(activingCard);
                break;
            default :
                break;    
        }
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

    //Doble Alma's effect
    public void TakeControl(GameObject selectedCard)
    {
        selectedCard.transform.SetParent(effectRow);
        selectedCard.transform.localPosition = Vector3.zero;
        selectedCard.transform.localScale = Vector3.one;               
        selectedCard.transform.localRotation = Quaternion.identity;  

         cardSelectionPanel.SetActive(false);
    }

    private List<DisplayCard> GetCards(Transform orig, bool b)
    {
        List<DisplayCard> cardsToAdd = new List<DisplayCard>();
        DisplayCard[] cards = orig.GetComponentsInChildren<DisplayCard>();

        foreach(DisplayCard c in cards)
        {
            if(b && c.card.isUnit) cardsToAdd.Add(c);
            else if(!c.card.isUnit) cardsToAdd.Add(c);
        }
        return cardsToAdd;
    }    

    private void ShowCardToControl(List<DisplayCard> cardsToAdd)
    {
        foreach(Transform child in cardListContainer)
        {
            Destroy(child.gameObject);
        }

        foreach(DisplayCard c in cardsToAdd)
        {
            GameObject cardB = Instantiate(cardButtonPrefab, cardListContainer);
            cardB.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>(c.card.name);
            cardB.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => OnCardSelected(c.gameObject));
        }        
        cardSelectionPanel.SetActive(true);
    }

    private void OnCardSelected(GameObject selectedCard)
    {
        if(selectedCard.transform.parent == board.transformMeleeRow) effectRow = board.opponentTransformMeleeRow;
        else if(selectedCard.transform.parent == board.transformRangedRow) effectRow = board.opponentTransformRangedRow;
        else if(selectedCard.transform.parent == board.transformSeigeRow) effectRow = board.opponentTransformSeigeRow;
        else if(selectedCard.transform.parent == board.opponentTransformMeleeRow) effectRow = board.transformMeleeRow;
        else if(selectedCard.transform.parent == board.opponentTransformRangedRow) effectRow = board.transformRangedRow;
        else effectRow = board.transformSeigeRow;

        TakeControl(selectedCard);
    }

    private Transform[] GetRows(bool b)
    {
        if(b) return new Transform[] {board.transformMeleeRow, board.transformRangedRow, board.transformSeigeRow};
        else return new Transform[] {board.opponentTransformMeleeRow, board.opponentTransformRangedRow, board.opponentTransformSeigeRow};
    }
}
