using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SpecialSummonEffect : MonoBehaviour , ICardEffect
{
    public GameObject cardSelectionPanel;  // Panel que contendrá las cartas
    public GameObject cardButtonPrefab;    // Prefab de un botón para cada carta
    public Transform cardListContainer;   
    public GameObject cardPrefabs;

    private Card activingCard;
    private BoardManager board;
    
    private CardEffects cardEffect;

    void Start()
    {
        board = FindObjectOfType<BoardManager>();
        cardEffect = FindObjectOfType<CardEffects>();
    }

    public void Execute(GameObject activingCard)
    {
        DisplayCard activingC = activingCard.GetComponent<DisplayCard>();

        switch(activingC.card.effect)
        {
            case "SummonFromGraveyard":
                Debug.Log("Activando el efecto de la carta " + activingC.card.name);
                SummonFromGraveyard(activingCard);
                EndEffect(activingCard);
                break;
            case "SummonToken" :
                Debug.Log("Activando el efecto de la carta " + activingC.card.name);        
                SummonToken(activingCard, activingCard.transform.parent);
                EndEffect(activingCard);
                break;    
            case "ShadowGarden" :
                Debug.Log("Activando el efecto de la carta " + activingC.card.name);
                ShadowGarden(activingCard);
                EndEffect(activingCard);
                break;
            case "ReturnCard" :
                Debug.Log("Activando el efecto de la carta " + activingC.card.name);
                ReturnCard(activingCard);
                EndEffect(activingCard);
                break;
            case "Ritual" :
                Debug.Log("Activando el efecto de la carta " + activingC.card.name);
                Ritual(activingCard);
                EndEffect(activingCard);
                break;
            case "SummonCopy" :    
                Debug.Log("Activando el efecto de la carta " + activingC.card.name);
                SummonCopy(activingCard);
                EndEffect(activingCard);
                break;
            case "Saltamontes":
                Debug.Log("Activando el efecto de la carta " + activingC.card.name);
                Saltamontes(activingCard);
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

    //Blame Me'S Effect
    public void SummonFromGraveyard(GameObject activingCard)
    {
        List<DisplayCard> cardsToShow = new List<DisplayCard>();
        Transform[] origenes = {board.transformGraveyard, board.opponentTransformGraveyard};
        foreach(Transform t in origenes)
        {
            foreach(DisplayCard c in GetCards(t,true)) cardsToShow.Add(c);
        }
        for(int i = 0; i < cardsToShow.Count; i++)
        {
            cardsToShow[i].card.owner = activingCard.GetComponent<DisplayCard>().card.owner;
        }
        
        if(cardsToShow.Count != 0) ShowCardSelectionPanel(cardsToShow);
    }

    //Fishiguro Megumi's effect
    public void SummonToken(GameObject activingCard, Transform rowParent)
    {
        int randomIndex = UnityEngine.Random.Range(0, CardDataBase.tokenData.Count);
        Card tokenCardToSummon = CardDataBase.tokenData[randomIndex];

        GameObject token = Instantiate(cardPrefabs, rowParent);
        
        token.transform.localScale = Vector3.one;
        token.transform.localPosition = Vector3.zero;
        token.transform.localRotation = Quaternion.identity;

        DisplayCard tokenCard = token.GetComponent<DisplayCard>();
        
        if(tokenCard != null)
        {
            tokenCard.card = tokenCardToSummon;
            tokenCard.card.isActivated = true;
            tokenCard.SetUp(tokenCard.card);
        }
        if(tokenCard.gameObject.GetComponent<CardLogic>() == null)
        {
            tokenCard.gameObject.GetComponent<CardLogic>();
        }

        DisplayCard aux = activingCard.GetComponent<DisplayCard>();
        tokenCard.card.owner = aux.card.owner; 
    }

    //Jardin Sombrio de quimeras' effect
    public void ShadowGarden(GameObject activingCard)
    {   
        int cardsInPlayerField = board.transformMeleeRow.childCount + board.transformRangedRow.childCount + board.transformSeigeRow.childCount;
        int cardsInOppField = board.opponentTransformMeleeRow.childCount + board.opponentTransformRangedRow.childCount + board.opponentTransformSeigeRow.childCount;
        Debug.Log("El campo rival tiene " + cardsInPlayerField);
        Debug.Log("El campo rival tiene " + cardsInOppField);
        DisplayCard aux = activingCard.GetComponent<DisplayCard>();

        if(aux.card.owner == Card.Owner.Player)
        {
            if(cardsInOppField > cardsInPlayerField)
            {
                Transform[] rows = {board.transformMeleeRow, board.transformRangedRow, board.transformSeigeRow};
                for(int i = 0; i < cardsInOppField - cardsInPlayerField; i++)
                {
                    int random = UnityEngine.Random.Range(0, rows.Length);
                    SummonToken(activingCard, rows[random]);
                }   
            }
        }
        else
        {
            if(cardsInPlayerField > cardsInOppField)
            {
                Transform[] rows = {board.opponentTransformMeleeRow, board.opponentTransformRangedRow, board.opponentTransformSeigeRow};
                for(int i = 0; i < cardsInPlayerField - cardsInOppField; i++)
                {
                    int random = UnityEngine.Random.Range(0, rows.Length);
                    SummonToken(activingCard, rows[random]);
                }                   
            }
        }
    }

    //Kechizu
    public void ReturnCard(GameObject activingCard)
    {
        DisplayCard activingC = activingCard.GetComponent<DisplayCard>();
        if(activingC.card.owner == Card.Owner.Player)
        {
            Transform[] rows = {board.transformMeleeRow, board.transformRangedRow, board.transformSeigeRow};
            List<DisplayCard> cardsToShow = new List<DisplayCard>();

            foreach(Transform t in rows)
            {
                foreach(DisplayCard c in GetCards(t, true)) cardsToShow.Add(c);
            }
            ShowCardToReturnHand(cardsToShow);
        }
        else 
        {
            Transform[] rows = {board.opponentTransformMeleeRow, board.opponentTransformRangedRow, board.opponentTransformSeigeRow};
            List<DisplayCard> cardsToShow = new List<DisplayCard>();

            foreach(Transform t in rows)
            {
                foreach(DisplayCard c in GetCards(t, true)) cardsToShow.Add(c);
            }
            ShowCardToReturnHand(cardsToShow);
        }    
    }

    //Mahoraga's Ritual effect
    public void Ritual(GameObject activingCard)
    {
        DisplayCard ritual = activingCard.GetComponent<DisplayCard>();
        DisplayCard cardToTribute;
        
        if(ritual.card.owner == Card.Owner.Player)
        {
            cardToTribute = MaxAttackCard(board.transformMeleeRow);
            if(cardToTribute != null) 
            {
                DestroyCard(cardToTribute.gameObject);

                DisplayCard mahoragaCard = FindCardByName("Divine General Mahoraga", board.transformDeck);
                if(mahoragaCard == null) mahoragaCard = FindCardByName("Divine General Mahoraga", board.transformPlayerHand);

                if(mahoragaCard != null)
                {
                    Summon(mahoragaCard.gameObject, board.transformMeleeRow);
                    cardEffect.Execute(mahoragaCard.gameObject);
                } 
                else Debug.Log("Mahoraga no se encuentra ni en el deck ni en la mano");
            }
            else Debug.Log("No hay cartas en la fila Melee para sacrificar.");
        }
        else
        {
            cardToTribute = MaxAttackCard(board.opponentTransformMeleeRow);
            if(cardToTribute != null) 
            {
                DestroyCard(cardToTribute.gameObject);

                DisplayCard mahoragaCard = FindCardByName("Divine General Mahoraga", board.opponentTransformDeck);
                if(mahoragaCard == null) mahoragaCard = FindCardByName("Divine General Mahoraga", board.opponentTransformPlayerHand);

                if(mahoragaCard != null)
                {
                    Summon(mahoragaCard.gameObject, board.opponentTransformMeleeRow);
                    cardEffect.Execute(mahoragaCard.gameObject);
                } 
                else Debug.Log("Mahoraga no se encuentra ni en el deck ni en la mano");
            }
            else Debug.Log("No hay cartas en la fila Melee para sacrificar.");
        }
    }

    //Yuuta Okkotsu's effect
    public void SummonCopy(GameObject activingCard)
    {
        DisplayCard activingC = activingCard.GetComponent<DisplayCard>();
        
        if(activingC.card.owner == Card.Owner.Player)
        {
            Transform[] rows = {board.transformMeleeRow, board.transformRangedRow, board.transformSeigeRow};
            Transform[] origins = {board.transformDeck, board.transformPlayerHand};
            List<DisplayCard> cardsToShow = new List<DisplayCard>();
            List<DisplayCard> cardsToCopy = new List<DisplayCard>();

            foreach(Transform t in rows)
            {
                foreach(DisplayCard c in GetCards(t, true)) cardsToShow.Add(c);
            }
            for(int i = 0; i < cardsToShow.Count; i++)
            {
                if(cardsToShow[i].card.isSpecial) cardsToShow.Remove(cardsToShow[i]);
            }

            foreach(Transform t in origins)
            {
                List<DisplayCard> aux = GetCards(t, true); 
                foreach(DisplayCard c in cardsToShow) 
                {
                    for(int i = 0; i < aux.Count; i ++)
                    if(c == aux[i]) cardsToCopy.Add(aux[i]);
                }
            }

            if(cardsToShow.Count != 0) ShowCardSelectionPanel(cardsToCopy);
        }
        else 
        {
            Transform[] rows = {board.opponentTransformMeleeRow, board.opponentTransformRangedRow, board.opponentTransformSeigeRow};
            Transform[] origins = {board.opponentTransformDeck, board.opponentTransformPlayerHand};
            List<DisplayCard> cardsToShow = new List<DisplayCard>();
            List<DisplayCard> cardsToCopy = new List<DisplayCard>();

            foreach(Transform t in rows)
            {
                foreach(DisplayCard c in GetCards(t, true)) cardsToShow.Add(c);
            }
            for(int i = 0; i < cardsToShow.Count; i++)
            {
                if(cardsToShow[i].card.isSpecial) cardsToShow.Remove(cardsToShow[i]);
            }

            foreach(Transform t in origins)
            {
                int i = 0;
                foreach(DisplayCard c in GetCards(t, true)) 
                {
                    if(c == cardsToShow[i]) cardsToCopy.Add(c);
                }
            }

            if(cardsToShow.Count != 0) ShowCardSelectionPanel(cardsToCopy);    
        }
    }

    //Saltamontes' curse's effect
    public void Saltamontes(GameObject activingCard)
    {
        DisplayCard activingC = activingCard.GetComponent<DisplayCard>();
        DisplayCard[] cardsInDeck;
        DisplayCard[] cardsInHand;
        DisplayCard[] cardsInGra;

        List<DisplayCard> cardsToSummon = new List<DisplayCard>();

        if(activingC.card.owner == Card.Owner.Player)
        {
            cardsInDeck = board.transformDeck.GetComponentsInChildren<DisplayCard>();
            cardsInHand = board.transformPlayerHand.GetComponentsInChildren<DisplayCard>();
            cardsInGra = board.transformGraveyard.GetComponentsInChildren<DisplayCard>();

            if(ThereIsIn(activingC, cardsInDeck))
            {
                cardsToSummon.Add(GetCardFrom(cardsInDeck, activingC));
            }
            if(ThereIsIn(activingC, cardsInHand))
            {
                cardsToSummon.Add(GetCardFrom(cardsInHand, activingC));
            }
            if(ThereIsIn(activingC, cardsInGra))
            {
                cardsToSummon.Add(GetCardFrom(cardsInGra, activingC));
            }      

            if(cardsToSummon.Count != 0) ShowCardSelectionPanel(cardsToSummon);
            else 
            {
                Debug.Log("No hay opciones disponibles para invocar la carta. Istanciamos un prefab");

                GameObject copyCard = Instantiate(cardPrefabs, activingCard.transform.parent);
                copyCard.transform.localPosition = Vector3.one;
                copyCard.transform.localScale = Vector3.zero;
                copyCard.transform.localRotation = Quaternion.identity;

                copyCard.GetComponent<DisplayCard>().SetUp(activingC.card); 
            }
        }
        //--------------------------------------------------------------
        else 
        {
            cardsInDeck = board.opponentTransformDeck.GetComponentsInChildren<DisplayCard>();
            cardsInHand = board.opponentTransformPlayerHand.GetComponentsInChildren<DisplayCard>();
            cardsInGra = board.opponentTransformGraveyard.GetComponentsInChildren<DisplayCard>();

            if(ThereIsIn(activingC, cardsInDeck))
            {
                cardsToSummon.Add(GetCardFrom(cardsInDeck, activingC));
            }
            if(ThereIsIn(activingC, cardsInHand))
            {
                cardsToSummon.Add(GetCardFrom(cardsInHand, activingC));
            }
            if(ThereIsIn(activingC, cardsInGra))
            {
                cardsToSummon.Add(GetCardFrom(cardsInGra, activingC));
            }      

            if(cardsToSummon.Count != 0) ShowCardSelectionPanel(cardsToSummon);
            else
            {
                Debug.Log("No hay opciones disponibles para invocar la carta. Istanciamos un prefab");

                GameObject copyCard = Instantiate(cardPrefabs, activingCard.transform.parent);
                copyCard.transform.localPosition = Vector3.zero;
                copyCard.transform.localScale = Vector3.one;
                copyCard.transform.localRotation = Quaternion.identity;

                copyCard.GetComponent<DisplayCard>().SetUp(activingC.card); 
            }    
        }
    }

//-------Metodos basicos para utilizar en los efectos ----------------------------------------------------------------------------------
    private List<DisplayCard> GetCards(Transform origin, bool b)
    {
        List<DisplayCard> cardsToShow = new List<DisplayCard>();
        DisplayCard[] cards = origin.GetComponentsInChildren<DisplayCard>();

        foreach(DisplayCard c in cards)
        {
            if(b && c.card.isUnit) cardsToShow.Add(c);
            else if(!c.card.isUnit) cardsToShow.Add(c);
        }
        return cardsToShow;
    }

    private DisplayCard GetCardFrom(DisplayCard[] cards, DisplayCard c)
    {
        foreach(DisplayCard cd in cards)
        {
            if(cd == c) return cd;
        }
        return null;
    }
    
    private void Summon(GameObject cardToSummon, Transform row)
    {
        cardToSummon.transform.SetParent(row);
        cardToSummon.transform.localPosition = Vector3.zero;
        cardToSummon.transform.localRotation = Quaternion.identity;
        cardToSummon.transform.localScale = Vector3.one;

        DisplayCard c = cardToSummon.GetComponent<DisplayCard>();
        c.card.isActivated = true;
        cardToSummon.GetComponent<DisplayCard>().SetUp(c.card);
    }

    public void ShowCardSelectionPanel(List<DisplayCard> cardsToShow)
    {
        foreach(Transform child in cardListContainer)
        {
            Destroy(child.gameObject);
        }
        
        foreach(DisplayCard c in cardsToShow)
        {
            GameObject buttonObj = Instantiate(cardButtonPrefab, cardListContainer);
            buttonObj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => 
            {
                SummonOn(c); 
            });
            buttonObj.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>(c.card.name);
            buttonObj.GetComponentInChildren<UnityEngine.UI.Text>().text = " ";

            cardSelectionPanel.SetActive(true);
        }
    }

    private void SummonOn(DisplayCard card)
    {
        Transform row = RandomRowCard(card);

        Summon(card.gameObject, row);

        cardSelectionPanel.SetActive(false);
    }

    private void ShowCardToReturnHand(List<DisplayCard> cards)
    {
        foreach(Transform child in cardListContainer)
        {
            Destroy(child.gameObject);
        }

        foreach(DisplayCard c in cards)
        {
            GameObject cardB = Instantiate(cardButtonPrefab, cardListContainer);
            cardB.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>(c.card.name);
            cardB.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ReturnCardToHandPlayer(c));
        }
        cardSelectionPanel.SetActive(true);
    }

    private void ReturnCardToHandPlayer(DisplayCard c)
    {
        TokenForCard(c, c.transform.parent);
        if(c.card.owner == Card.Owner.Player) ReturnCardToHand(c.gameObject, board.transformPlayerHand);
        else ReturnCardToHand(c.gameObject, board.opponentTransformPlayerHand);
    }

    private void ReturnCardToHand(GameObject cardToReturn, Transform hand)
    {
        if(hand.childCount >= 10 && hand == board.transformPlayerHand) cardToReturn.transform.SetParent(board.transformGraveyard);
        else if(hand.childCount >= 10 && hand == board.opponentTransformPlayerHand) cardToReturn.transform.SetParent(board.opponentTransformGraveyard);
        else cardToReturn.transform.SetParent(hand);

        cardToReturn.transform.localScale = Vector3.one;
        cardToReturn.transform.localPosition = Vector3.zero;
        cardToReturn.transform.localRotation = Quaternion.identity;
        cardSelectionPanel.SetActive(false);

    }
    private void TokenForCard(DisplayCard c, Transform rowParent)
    {
        int randomIndex = UnityEngine.Random.Range(0, CardDataBase.tokenData.Count);
        Card tokenCardToSummon = CardDataBase.tokenData[randomIndex];

        GameObject token = Instantiate(cardPrefabs, rowParent);
        
        token.transform.localScale = Vector3.one;
        token.transform.localPosition = Vector3.zero;
        token.transform.localRotation = Quaternion.identity;

        DisplayCard tokenCard = token.GetComponent<DisplayCard>();
        
        if(tokenCard != null)
        {
            tokenCard.card = tokenCardToSummon;
            tokenCard.card.SetAttack(0);
            tokenCard.card.isActivated = true;
            tokenCard.SetUp(tokenCard.card);
        }
        if(tokenCard.gameObject.GetComponent<CardLogic>() == null)
        {
            tokenCard.gameObject.GetComponent<CardLogic>();
        }

        tokenCard.card.owner = c.card.owner;

        cardSelectionPanel.SetActive(false); 
    }

    private Transform RandomRowCard(DisplayCard c)
    {
        if(c.card.isUnit)
        {
            int random = UnityEngine.Random.Range(0, 3);
            if(c.card.owner != Card.Owner.Opponent)
            {
                if(c.card.GetKind()[random].Equals("M")) return board.transformMeleeRow;
                else if(c.card.GetKind()[random].Equals("R")) return board.transformRangedRow;
                else return board.transformSeigeRow;
            }
            else 
            {
                if(c.card.GetKind()[random].Equals("M")) return board.opponentTransformMeleeRow;
                else if(c.card.GetKind()[random].Equals("R")) return board.opponentTransformRangedRow;
                else return board.opponentTransformSeigeRow;
            }
        }
        else return null;
    }

    private DisplayCard MaxAttackCard(Transform Row)
    {
        if(Row == board.transformMeleeRow || Row == board.transformRangedRow || Row == board.transformSeigeRow
          || Row == board.opponentTransformMeleeRow || Row == board.opponentTransformRangedRow || Row == board.opponentTransformSeigeRow)
        {
            DisplayCard[] cards = Row.GetComponentsInChildren<DisplayCard>();
            DisplayCard strongestCard;

            if(cards.Length != 0)
            {
                strongestCard = cards[0];
                for(int i = 1; i < cards.Length; i++)
                {
                    if(strongestCard.card.GetPower() < cards[i].card.GetPower())
                    {
                        strongestCard = cards[i];
                    }
                }
                return strongestCard;
            }
            else return null;
        }
        else return null;    
    }

    private void DestroyCard(GameObject targedCard)
    {
        DisplayCard targedC = targedCard.GetComponent<DisplayCard>();
        
        if(targedC.card.owner == Card.Owner.Player)
        {
            targedCard.transform.SetParent(board.transformGraveyard);
            targedCard.transform.localPosition = Vector3.zero;
            targedCard.transform.localScale = Vector3.one;
            targedCard.transform.localRotation = Quaternion.identity;
        }
        else
        {
            targedCard.transform.SetParent(board.opponentTransformGraveyard);
            targedCard.transform.localPosition = Vector3.zero;
            targedCard.transform.localScale = Vector3.one;
            targedCard.transform.localRotation = Quaternion.identity;
        }
    }

    private bool ThereIsIn(DisplayCard c, DisplayCard[] cards)
    {
        foreach (DisplayCard card in cards)
        {
            if (card == c)
            {
                return true;
            }
        }
        return false; 
    }

    private DisplayCard FindCardByName(string cardName, Transform t)
    {
        DisplayCard[] aux = t.GetComponentsInChildren<DisplayCard>();

        foreach(DisplayCard c in aux)
        {
            if(c.card.name.Equals(cardName)) return c;
        }
        return null;
    }
}

