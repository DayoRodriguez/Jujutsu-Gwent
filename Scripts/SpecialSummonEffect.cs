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
    private Card activingCard;
    private BoardManager board;
    public GameObject auxCard;
    private CardEffects cardEffect;

    public void Execute(GameObject activingCard)
    {
        Initialize();
        DisplayCard activingC = activingCard.GetComponent<DisplayCard>();
        //cardEffect = FindObjectOfType<CardEffects>();

        //auxCard = cardEffect.activingCard;

        switch(activingC.card.effect)
        {
            case "SummonFromGraveyard":
                Debug.Log("Activando el efecto de la carta " + activingC.card.name);
                SummonFromGraveyard();
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

    public void Initialize()
    {
        //cardSelectionPanel = GameObject.Find("cardSelectionPanel");
        //cardListContainer = GameObject.Find("cardListContainer").GetComponent<Transform>();
        board = FindObjectOfType<BoardManager>();
    }

    public void ShowMessagePanel(string sms)
    {
        Debug.Log(sms);
    }

    public bool CanActive()
    {
        //Revisar este metodo antes de hacerlo
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

    //Blame Me'S Effect
    public void SummonFromGraveyard()
    {
        DisplayCard[] playerGraveyard = board.transformGraveyard.GetComponents<DisplayCard>();
        DisplayCard[] opponentGraveyard = board.opponentTransformGraveyard.GetComponents<DisplayCard>();
        List<DisplayCard> cardsToShow = new List<DisplayCard>();
        
        if(playerGraveyard.Length != 0)
        {
            foreach(DisplayCard c in playerGraveyard)
            {
                if(c.card.isUnit) cardsToShow.Add(c);
            }
        }
        if(opponentGraveyard.Length != 0)
        {
            foreach(DisplayCard c in opponentGraveyard)
            {
                if(c.card.isUnit) cardsToShow.Add(c);
            }
        }
        DisplayCard[] auxCards = cardsToShow.ToArray();
        ShowCardSelectionPanel(auxCards);
    }

    //Fishiguro Megumi's effect
    public void SummonToken(GameObject activingCard, Transform rowParent)
    {
        int randomIndex = UnityEngine.Random.Range(0, CardDataBase.tokenData.Count);
        Card tokenCardToSummon = CardDataBase.tokenData[randomIndex];

        GameObject token = Instantiate(auxCard, rowParent);
        
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
        int cardsInMelee = HowManyCardsByRow(board.transformMeleeRow);
        int cardsInRanged = HowManyCardsByRow(board.transformRangedRow);
        int cardsInSeige = HowManyCardsByRow(board.transformSeigeRow);

        int cardsInOppMelee = HowManyCardsByRow(board.opponentTransformMeleeRow);
        int cardsInOppRanged = HowManyCardsByRow(board.opponentTransformRangedRow);
        int cardsInOppSeige = HowManyCardsByRow(board.opponentTransformSeigeRow);
        
        int cardsInPlayerField = cardsInMelee + cardsInRanged + cardsInSeige;
        int cardsInOppField = cardsInOppMelee + cardsInOppRanged + cardsInOppSeige;

        DisplayCard aux = activingCard.GetComponent<DisplayCard>();

        if(aux.card.owner == Card.Owner.Player)
        {
            if(cardsInOppField > cardsInPlayerField)
            {
                if(cardsInMelee <= cardsInRanged && cardsInMelee <= cardsInSeige)
                {
                    for(int i = 0; i < cardsInOppField - cardsInPlayerField; i++)
                    {
                        SummonToken(activingCard, board.transformMeleeRow);
                    }   
                }
                else if(cardsInRanged <= cardsInMelee && cardsInRanged <= cardsInSeige)
                {
                    for(int i = 0; i < cardsInOppField - cardsInPlayerField; i++)
                    {
                        SummonToken(activingCard, board.transformRangedRow);
                    }   
                }
                else
                {
                    for(int i = 0; i < cardsInOppField - cardsInPlayerField; i++)
                    {
                        SummonToken(activingCard, board.transformSeigeRow);
                    }
                }
            }
        }
        else
        {
            if(cardsInPlayerField > cardsInOppField)
            {
                if(cardsInOppMelee <= cardsInOppRanged && cardsInOppMelee <= cardsInOppSeige)
                {
                    for(int i = 0; i < cardsInPlayerField - cardsInOppField; i++)
                    {
                        SummonToken(activingCard, board.opponentTransformMeleeRow);
                    }   
                }
                else if(cardsInOppRanged <= cardsInOppMelee && cardsInOppRanged <= cardsInOppSeige)
                {
                    for(int i = 0; i < cardsInPlayerField - cardsInOppField; i++)
                    {
                        SummonToken(activingCard, board.opponentTransformRangedRow);
                    }   
                }
                else
                {
                    for(int i = 0; i < cardsInPlayerField - cardsInOppField; i++)
                    {
                        SummonToken(activingCard, board.opponentTransformSeigeRow);
                    }
                }
            }
        }
    }

    //Kechizu
    public IEnumerator ReturnCard(GameObject activingCard)
    {
        yield return board.WaitForSelection<GameObject>
        (
            selectedCard =>
            {
                DisplayCard activingAux = activingCard.GetComponent<DisplayCard>();
                DisplayCard targedAux = selectedCard.GetComponent<DisplayCard>();

                if(activingAux.card.owner == targedAux.card.owner)
                {
                    SummonToken(selectedCard, selectedCard.transform.parent);

                    DisplayCard[] tokensInRow = selectedCard.transform.parent.GetComponents<DisplayCard>();
                    DisplayCard lastToken = tokensInRow[tokensInRow.Length - 1];

                    if(lastToken != null)
                    {
                    lastToken.card.SetAttack(0);
                    }

                    selectedCard.transform.SetParent(board.transformPlayerHand);
                }
            },
            () => board.effectSelectedCard == null
        );     
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

                if(mahoragaCard != null) Summon(mahoragaCard.gameObject, board.transformMeleeRow);
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

                if(mahoragaCard != null) Summon(mahoragaCard.gameObject, board.opponentTransformMeleeRow);
                else Debug.Log("Mahoraga no se encuentra ni en el deck ni en la mano");
            }
            else Debug.Log("No hay cartas en la fila Melee para sacrificar.");
        }
    }

    //Yuuta Okkotsu's effect
    public IEnumerator SummonCopy(GameObject activingCard)
    {
        yield return board.WaitForSelection<GameObject>
        (
            selectedCard =>
            {
                DisplayCard activingC = activingCard.GetComponent<DisplayCard>();
                DisplayCard targedC = selectedCard.GetComponent<DisplayCard>();
                DisplayCard cardToSummon;
                DisplayCard[] cardsInDeck;
                DisplayCard[] cardsInHand;

                if(!targedC.card.isSpecial)
                {
                    if(activingC.card.owner == Card.Owner.Player)
                    {
                        cardsInDeck = board.transformDeck.GetComponents<DisplayCard>();
                        cardsInHand = board.transformPlayerHand.GetComponents<DisplayCard>();

                        if(IsThere(cardsInDeck, targedC))
                        {
                            cardToSummon = targedC;
                            Summon(selectedCard, selectedCard.transform.parent);
                        }
                        else if(IsThere(cardsInHand, targedC))
                        {
                            cardToSummon = targedC;
                            Summon(selectedCard, selectedCard.transform.parent);
                        }
                    }
                    else 
                    {
                        cardsInDeck = board.opponentTransformDeck.GetComponents<DisplayCard>();
                        cardsInHand = board.opponentTransformPlayerHand.GetComponents<DisplayCard>();

                        if(IsThere(cardsInDeck, targedC))
                        {
                            cardToSummon = targedC;
                            Summon(selectedCard, selectedCard.transform.parent);
                        }
                        else if(IsThere(cardsInHand, targedC))
                        {
                            cardToSummon = targedC;
                            Summon(selectedCard, selectedCard.transform.parent);
                        }
                    }
                }
            },
            () => board.effectSelectedCard == null
        );     
    }

    //Saltamontes' curse's effect
    public void Saltamontes(GameObject activingCard)
    {
        DisplayCard activingC = activingCard.GetComponent<DisplayCard>();
        DisplayCard[] cardsInDeck;
        DisplayCard[] cardsInHand;
        DisplayCard[] cardsInGra;
        bool thereIsDeck = false;
        bool thereIsHand = false;
        bool thereIsGra = false;

        if(activingC.card.owner == Card.Owner.Player)
        {
            cardsInDeck = board.transformDeck.GetComponentsInChildren<DisplayCard>();
            cardsInHand = board.transformPlayerHand.GetComponentsInChildren<DisplayCard>();
            cardsInGra = board.transformGraveyard.GetComponentsInChildren<DisplayCard>();

            if(cardsInDeck.Length != 0 && IsThere(cardsInDeck, activingC))
            {
                thereIsDeck = true;
            }
            if(cardsInHand.Length != 0 && IsThere(cardsInHand, activingC))
            {
                thereIsHand = true;
            }
            if(cardsInGra.Length != 0 && IsThere(cardsInGra, activingC))
            {
                thereIsGra = true;
            }      

            List<string> availableOptions = new List<string>();

            if (thereIsDeck)
            {
                availableOptions.Add("Deck");
            }
            if (thereIsHand)
            {
                availableOptions.Add("Hand");
            }
            if (thereIsGra)
            {
                availableOptions.Add("Graveyard");
            }

            if (availableOptions.Count > 0)
            {
                ShowInvocationOptions(availableOptions, activingC);
            }
            else
            {
                Debug.Log("No hay opciones disponibles para invocar la carta.");
            }
        }
        //--------------------------------------------------------------
        else 
        {
            cardsInDeck = board.transformDeck.GetComponentsInChildren<DisplayCard>();
            cardsInHand = board.transformPlayerHand.GetComponentsInChildren<DisplayCard>();
            cardsInGra = board.transformGraveyard.GetComponentsInChildren<DisplayCard>();

            if(cardsInDeck.Length != 0 && IsThere(cardsInDeck, activingC))
            {
                thereIsDeck = true;
            }
            if(cardsInHand.Length != 0 && IsThere(cardsInHand, activingC))
            {
                thereIsHand = true;
            }
            if(cardsInGra.Length != 0 && IsThere(cardsInGra, activingC))
            {
                thereIsGra = true;
            }      

            List<string> availableOptions = new List<string>();

            if (thereIsDeck)
            {
                availableOptions.Add("Deck");
            }
            if (thereIsHand)
            {
                availableOptions.Add("Hand");
            }
            if (thereIsGra)
            {
                availableOptions.Add("Graveyard");
            }

            if (availableOptions.Count > 0)
            {
                ShowInvocationOptions(availableOptions, activingC);
            }
            else
            {
                Debug.Log("No hay opciones disponibles para invocar la carta.");
            }
        }
    }

//-------Metodos basicos para utilizar en los efectos ----------------------------------------------------------------------------------
    private void Summon(GameObject card, Transform row)
    {
        card.transform.SetParent(row);
        card.transform.localPosition = Vector3.zero;
        card.transform.localRotation = Quaternion.identity;
        card.transform.localScale = Vector3.one;

        DisplayCard cardToSummon = card.GetComponent<DisplayCard>();
        if (cardToSummon != null)
        {
            cardToSummon.card.isActivated = true;
            cardToSummon.SetUp(cardToSummon.card);
        }
    }

    private bool IsThere(DisplayCard[] cards, DisplayCard cardToLook)
    {
        if(cards.Length != 0)
        {
            foreach(DisplayCard c in cards)
            {
                if(c.card.name.Equals(cardToLook.card.name))
                {
                    return true;
                }
            }
            return false;
        }
        else return false;
    }
    private int HowManyCardsByRow(Transform row)
    {
        DisplayCard[] cards = row.GetComponents<DisplayCard>();
        return cards.Length;
    }

    public void ShowCardSelectionPanel(DisplayCard[] cardsToShow)
    {
        //limpiar el contenedor de los botones
        foreach(Transform child in cardListContainer)
        {
            Destroy(child.gameObject);
        }
        
        foreach(DisplayCard c in cardsToShow)
        {
            GameObject buttonObj = Instantiate(cardButtonPrefab, cardListContainer);
            Button button =buttonObj.GetComponent<Button>();
            Text buttonText = buttonObj.GetComponentInChildren<Text>();
            buttonText.text = c.card.name;

            //Añadir evento para que se active la carta al hacer clic
            button.onClick.AddListener(() => 
            {
               Summon(c.gameObject, RandomRowCard(c));
               cardSelectionPanel.SetActive(false); 
            });

            cardSelectionPanel.SetActive(true);
        }
    }

    private Transform RandomRowCard(DisplayCard c)
    {
        if(c.card.isUnit)
        {
            int random = UnityEngine.Random.Range(0, c.card.GetKind().Length);
            if(c.card.owner != Card.Owner.Opponent)
            {
                if(c.card.GetKind().Equals("M")) return board.transformMeleeRow;
                else if(c.card.GetKind().Equals("R")) return board.transformRangedRow;
                else return board.transformSeigeRow;
            }
            else 
            {
                if(c.card.GetKind().Equals("M")) return board.opponentTransformMeleeRow;
                else if(c.card.GetKind().Equals("R")) return board.opponentTransformRangedRow;
                else return board.opponentTransformSeigeRow;
            }
        }
        else return null;
    }

    private void ShowInvocationOptions(List<string> options, DisplayCard activingC)
    {
        foreach(Transform child in cardListContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (string option in options)
        {
            GameObject buttonObj = Instantiate(cardButtonPrefab, cardListContainer);
            Button button = buttonObj.GetComponent<Button>();
            Text buttonText = buttonObj.GetComponentInChildren<Text>();
            buttonText.text = option;

            // Añadir evento de clic al botón para manejar la invocación según la opción seleccionada
            button.onClick.AddListener(() =>
            {
                switch (option)
                {
                    case "Deck":
                        if(activingC.card.owner == Card.Owner.Player) Summon(activingC.gameObject, board.transformDeck);
                        else Summon(activingC.gameObject, board.opponentTransformDeck);
                        break;
                    case "Hand":
                        if(activingC.card.owner == Card.Owner.Player) Summon(activingC.gameObject, board.transformPlayerHand);
                        else Summon(activingC.gameObject, board.opponentTransformPlayerHand);
                        break;
                    case "Graveyard":
                        if(activingC.card.owner == Card.Owner.Player) Summon(activingC.gameObject, board.transformGraveyard);
                        else Summon(activingC.gameObject, board.opponentTransformGraveyard);
                        break;
                }
                cardSelectionPanel.SetActive(false);
            });

            cardSelectionPanel.SetActive(true);
        }
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

    private DisplayCard FindCardByName(string cardName, Transform row)
    {
        DisplayCard[] cards = row.GetComponentsInChildren<DisplayCard>();

        foreach (DisplayCard card in cards)
        {
            if (card.card.name.Equals(cardName))
            {
                return card;
            }
        }
        return null; 
    }
}

