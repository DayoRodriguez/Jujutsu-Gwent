using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class PlayerDeck : MonoBehaviour
{
    public List<Card> deck = new List<Card>();
    public List<Card> opponentDeck = new List<Card>();
    private bool playerLeaderInstantiated = false;
    private bool opponentLeaderInstantiated = false;
    private int handMaxSide = 10;

    //private AudioSource musicControler;

    public GameObject Cards;
    public GameObject StartedSMS;

    public BoardManager board;

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<BoardManager>();

        DeckCreator(FactionSelection.Instance.playerFactionDeck, deck, ref playerLeaderInstantiated, board.transformLeaderCardSlot);
        InstantiateDeck(deck, board.transformDeck); 

        DeckCreator(FactionSelection.Instance.opponentFactionDeck, opponentDeck, ref opponentLeaderInstantiated, board.opponentTransformLeaderCardSlot);  
        InstantiateDeck(opponentDeck, board.opponentTransformDeck);

        // FirstDraw(playerHand,playerDeck);
        // FirstDraw(opponentPlayerHand,opponentPlayerDeck);

        // StartCoroutine(FirtsDrawPhases());
;    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DeckCreator(string factionDeck, List<Card> deck, ref bool leaderInstantiated, Transform leaderCardSlot)
    { 
        UnitCard cardLider = null;

        for(int i = 0; i < CardDataBase.leaderData.Count; i++)
        {
            if(CardDataBase.leaderData[i].isUnit) cardLider = (UnitCard)CardDataBase.leaderData[i];
            else continue;
            
            if(!leaderInstantiated && cardLider.faction.ToLower().Equals(factionDeck.ToLower()))
            { 
                GameObject leader = Instantiate(Cards, leaderCardSlot);
                leader.transform.localScale = Vector3.one;

                RectTransform leaderTransform = leader.GetComponent<RectTransform>();
                if(leaderTransform != null)
                {
                    leaderTransform.anchoredPosition3D = Vector3.zero;
                    leaderTransform.localScale = Vector3.one;
                }
                else
                {
                    leaderTransform.localScale = Vector3.one;
                }

                DisplayCard leaderDisplayCard = leader.GetComponent<DisplayCard>();
                
                if(leaderDisplayCard != null && leader != null)
                {
                    cardLider.isActivated = true;
                    cardLider.owner = leaderCardSlot == board.transformLeaderCardSlot ? Card.Owner.Player : Card.Owner.Opponent;
                    leaderDisplayCard.SetUp(cardLider);
                }
                leaderInstantiated = true;

                break;
            }    
        }
        for(int j = 0; j < 1000 && deck.Count < 25; j++)
        {
            int counter = Random.Range(0,CardDataBase.cardData.Count);
            Card auxCard = CardDataBase.cardData[counter];                       
            if((auxCard.faction.ToLower().Equals(cardLider.faction.ToLower()) 
            || auxCard.faction.ToLower().Equals("neutral")))
            {
                if(!deck.Contains(auxCard))
                {
                    auxCard.owner = leaderCardSlot == board.transformLeaderCardSlot ? Card.Owner.Player : Card.Owner.Opponent;
                    deck.Add(auxCard);
                } 
                else if(!auxCard.isSpecial && Count(auxCard, deck) < 3) 
                {
                    auxCard.owner = leaderCardSlot == board.transformLeaderCardSlot ? Card.Owner.Player : Card.Owner.Opponent;
                    deck.Add(auxCard);
                }    
            }
            else continue;
        }
    }

    private void InstantiateDeck(List<Card> deck, Transform CardsInDeck)
    {
        foreach(Card c in deck)
        {
           
            GameObject cardinDeck = Instantiate(Cards, CardsInDeck);
            cardinDeck.transform.localScale = Vector3.one;

            RectTransform rectTransform = cardinDeck.GetComponent<RectTransform>();
            if(rectTransform != null)
            {
                rectTransform.anchoredPosition3D = Vector3.zero;
                rectTransform.localScale = Vector3.one;
            }
            else
            {
                cardinDeck.transform.localPosition = Vector3.zero;
            } 

            DisplayCard displayCard = cardinDeck.GetComponent<DisplayCard>();
            if(displayCard != null && c != null)
            {
                c.isActivated = false;
                displayCard.SetUp(c);
            }
            else Debug.Log("La carta no fue cargada");
        }
    }

    public void FirstDraw(Transform hand, Transform deck)
    {
        DisplayCard[] cardsToDraw = deck.GetComponentsInChildren<DisplayCard>();
        
        for(int i = 0; i < handMaxSide; i++)
        {
            int random = Random.Range(0, cardsToDraw.Length);
            DisplayCard selectedCard = cardsToDraw[random];
            cardsToDraw = RemovedCardFromArray(cardsToDraw, random);

            selectedCard.card.isActivated = true;
            Card copyCard = Instantiate(selectedCard.card);
            copyCard.isActivated = true;
            selectedCard.SetUp(copyCard);

            selectedCard.transform.SetParent(hand);
            selectedCard.transform.localPosition = Vector3.zero;
            selectedCard.transform.localRotation = Quaternion.identity;
            selectedCard.transform.localScale = Vector3.one;

            if(selectedCard.gameObject.GetComponent<CardLogic>() == null)
            {
                selectedCard.gameObject.AddComponent<CardLogic>();
            }
            
            if(hand == board.transformPlayerHand)
            {
                selectedCard.card.owner = Card.Owner.Player;
                Debug.Log("La carta es" + selectedCard.card.name);
                Debug.Log("La carta esta en la mano del " + selectedCard.card.owner.ToString());
            }
            else if(hand == board.opponentTransformPlayerHand)
            {
                selectedCard.card.owner = Card.Owner.Opponent;
                Debug.Log("La carta es" + selectedCard.card.name);
                Debug.Log("La carta esta en la mano del " + selectedCard.card.owner.ToString());
            }
        }    
    }

    public IEnumerator FirtsDrawPhases()
    {
        //Inicia la Fase Mulligan
        yield return StartCoroutine(StartMulliganPhase());

        //Muestra el sms de que comienza el duelo
        yield return StartCoroutine(ShowStartSMS());    
    } 

    private IEnumerator StartMulliganPhase()
    {
        //Fase Mulligan de Player
        // musicControler = gameObject.AddComponent<AudioSource>();
        // musicControler.clip = board.gojoDominio;
        // musicControler.Play();
        // yield return new WaitForSeconds(1);
        // musicControler.clip = board.vacio;
        // musicControler.Play();
        // yield return new WaitForSeconds(1);
        yield return StartCoroutine(MulliganPhase(board.transformPlayerHand, board.transformDeck));

        //Fase Mulligan de Opponent
        // musicControler.clip = board.sukunaDominio;
        // musicControler.Play();
        // yield return new WaitForSeconds(2);
        // musicControler.clip = board.templo;
        // musicControler.Play();
        // yield return new WaitForSeconds(2);
        yield return StartCoroutine(MulliganPhase(board.opponentTransformPlayerHand, board.opponentTransformDeck));

        Debug.Log("La fase de Mulligan ha sido completada");
    }

    public IEnumerator MulliganPhase(Transform hand, Transform deck)
    {
        //Revisar por que este metodo no se ejecuta correctamente el cambio de cartas el comenzar la partida
        DisplayCard[] cardsInHand = hand.GetComponentsInChildren<DisplayCard>();
        int maxMulligan = 2;
        int mulliganCount = 0;
        MulliganManager mulliganManager = FindObjectOfType<MulliganManager>();
        Debug.Log("La mano tiene " + cardsInHand.Length);
        if(mulliganManager == null)
        {
            Debug.Log("No se encontro MulliganManager en la escena");
            yield break;
        }

        mulliganManager.ShowMulliganUI(true);
        yield return new WaitUntil(() => !mulliganManager.mulliganPanel.activeSelf);

        float timer = 10f;
        while(timer > 0 && mulliganCount < maxMulligan)
        {
            timer -= Time.deltaTime;

                foreach(DisplayCard cardH in cardsInHand)
                {
                    if(cardH.gameObject.GetComponent<CardLogic>() == null)
                    {
                        cardH.gameObject.AddComponent<CardLogic>();
                    }
                    if(cardH.card.IsSelected && !cardH.card.HasBeenMulligan)
                    {
                        mulliganCount ++;
                        cardH.card.HasBeenMulligan = true;
                        cardH.card.IsSelected = false;
                        cardH.card.isActivated = false;
                        cardH.SetUp(cardH.card);

                        cardH.transform.SetParent(deck);
                        cardH.transform.localPosition = Vector3.zero;
                        cardH.transform.localRotation = Quaternion.identity;
                        cardH.transform.localScale = Vector3.one;

                        DrawCard(hand, deck, 1);
                    }
                }
                yield return null;
            }
            mulliganManager.ShowMulliganUI(false);
        }

    public IEnumerator ShowStartSMS()
    {
        StartedSMS.SetActive(true);
        yield return new WaitForSeconds(1);
        StartedSMS.SetActive(false); 
    }

    public void DrawCard(Transform hand, Transform deck, int count)
    {
        DisplayCard[] cardsToDraw = deck.GetComponentsInChildren<DisplayCard>();

        for(int i = 0; i < count; i ++)
        {    
            
            int selector = Random.Range(0, cardsToDraw.Length);
            DisplayCard cardD = cardsToDraw[selector];

            if(cardD.gameObject.GetComponent<CardLogic>() == null)
            {
                cardD.gameObject.AddComponent<CardLogic>();
            }

            cardD.card.isActivated = true;
            cardD.SetUp(cardD.card);

            if(hand.childCount < 10)
            {
                cardD.transform.SetParent(hand);
            }
            else 
            {
                if(hand == board.transformPlayerHand) cardD.transform.SetParent(board.transformGraveyard);
                else if(hand == board.opponentTransformPlayerHand) cardD.transform.SetParent(board.opponentTransformGraveyard);
            }
            cardD.transform.localPosition = Vector3.zero;
            cardD.transform.localRotation = Quaternion.identity;
            cardD.transform.localScale = Vector3.one;

            if(hand == board.transformPlayerHand) cardD.card.owner = Card.Owner.Player;
            else if(hand == board.opponentTransformPlayerHand) cardD.card.owner = Card.Owner.Opponent;
        }
    }

    public DisplayCard[] RemovedCardFromArray(DisplayCard[] array, int index)
    {
        List<DisplayCard> list = new List<DisplayCard>(array);
        list.RemoveAt(index);
        return list.ToArray();
    }
    
    public static int Count(Card card, List<Card> deck)
    {
        int count = 0;
        for(int i = 0; i < deck.Count; i++)
        {
            if(deck.Contains(card)) count++;
        }
        return count;
    }

}
