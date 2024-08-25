    using System.Collections;
    using System.Collections.Generic;
    using Unity.VisualScripting;
    using UnityEngine;
    using System;
    public class BoardManager : MonoBehaviour
    {
        public Transform transformMeleeRow;
        public Transform transformRangedRow;
        public Transform transformSeigeRow;
        public Transform transformSpecialCardSlot;
        public Transform transformWeatherMeleeSlot;
        public Transform transformWeatherRangedSlot;
        public Transform transformWeatherSeigeSlot;
        public Transform transformLeaderCardSlot;
        public Transform transformDeck;
        public Transform transformPlayerHand;
        public Transform transformGraveyard;


        public Transform opponentTransformMeleeRow;
        public Transform opponentTransformRangedRow;
        public Transform opponentTransformSeigeRow;
        public Transform opponentTransformSpecialCardSlot;
        public Transform opponentTransformWeatherMeleeSlot;
        public Transform opponentTransformWeatherRangedSlot;
        public Transform opponentTransformWeatherSeigeSlot;
        public Transform opponentTransformLeaderCardSlot;
        public Transform opponentTransformDeck;
        public Transform opponentTransformPlayerHand;
        public Transform opponentTransformGraveyard;

        public Transform selectedRow;
        public GameObject selectedCard;

        public Transform opponentSelectedRow;
        public GameObject opponentSelectedCard;

        public Transform effectSelectedRow;
        public GameObject effectSelectedCard;

        public GameManager gameManager;

        public CardEffects effectCard; 

        private bool playerHasPerformedAction = false;
        private bool opponentHasPerformedAction = false;

        void Start()
        {
            gameManager = FindObjectOfType<GameManager>();
            effectCard = FindObjectOfType<CardEffects>();
        }

        public void ActiveCard(GameObject card, Transform row)
        {
            card.transform.SetParent(row);
            card.transform.localPosition = Vector3.zero;
            card.transform.localRotation = Quaternion.identity;
            card.transform.localScale = Vector3.one;
            DisplayCard cardD = card.GetComponent<DisplayCard>();
            
            if(cardD.card.owner == Card.Owner.Player)
            {
                playerHasPerformedAction = true;
            }
            else
            {
                opponentHasPerformedAction = true;
            }
            
            if(effectCard != null) effectCard.SetActivingCard(card);
            else Debug.Log("La carta no se ha podido guardar");
            effectCard.selectedRow = SelectionRow();
                       
        }

        private void OnEnable()
        {
            RowClickHandler.OnRowClicked += HandlerRowClicked;
        }

        private void OnDisable()
        {
            RowClickHandler.OnRowClicked -= HandlerRowClicked;
        }

        private void HandlerRowClicked(RowClickHandler.RowType rowType)
        {
            switch (rowType)
                {
                    case RowClickHandler.RowType.Melee:
                        selectedRow = transformMeleeRow;
                        break;
                    case RowClickHandler.RowType.Ranged:
                        selectedRow = transformRangedRow;
                        break;
                    case RowClickHandler.RowType.Siege:
                        selectedRow = transformSeigeRow;
                        break;
                    case RowClickHandler.RowType.Special:
                        selectedRow = transformSpecialCardSlot;
                        break;
                    case RowClickHandler.RowType.WeatherMelee:
                        selectedRow = transformWeatherMeleeSlot;
                        break;
                    case RowClickHandler.RowType.WeatherRanged:
                        selectedRow = transformWeatherRangedSlot;
                        break;
                    case RowClickHandler.RowType.WeatherSiege:
                        selectedRow = transformWeatherSeigeSlot;
                        break;
                    case RowClickHandler.RowType.Leader:
                        selectedRow = transformLeaderCardSlot;
                        break;
                    case RowClickHandler.RowType.Deck:
                        selectedRow = transformDeck;
                        break;
                    case RowClickHandler.RowType.Hand:
                        selectedRow = transformPlayerHand;
                        break;
                    case RowClickHandler.RowType.Graveyard:
                        selectedRow = transformGraveyard;
                        break;
                    case RowClickHandler.RowType.OpponentMelee:
                        opponentSelectedRow = opponentTransformMeleeRow;
                        break;
                    case RowClickHandler.RowType.OpponentRanged:
                        opponentSelectedRow = opponentTransformRangedRow;
                        break;
                    case RowClickHandler.RowType.OpponentSiege:
                        opponentSelectedRow = opponentTransformSeigeRow;
                        break;
                    case RowClickHandler.RowType.OpponentSpecial:
                        opponentSelectedRow = opponentTransformSpecialCardSlot;
                        break;
                    case RowClickHandler.RowType.OpponentWeatherMelee:
                        opponentSelectedRow = opponentTransformWeatherMeleeSlot;
                        break;
                    case RowClickHandler.RowType.OpponentWeatherRanged:
                        opponentSelectedRow = opponentTransformWeatherRangedSlot;
                        break;
                    case RowClickHandler.RowType.OpponentWeatherSiege:
                        opponentSelectedRow = opponentTransformWeatherSeigeSlot;
                        break;
                    case RowClickHandler.RowType.OpponentLeader:
                        opponentSelectedRow = opponentTransformLeaderCardSlot;
                        break;
                    case RowClickHandler.RowType.OpponentDeck:
                        opponentSelectedRow = opponentTransformDeck;
                        break;
                    case RowClickHandler.RowType.OpponentHand:
                        opponentSelectedRow = opponentTransformPlayerHand;
                        break;
                    case RowClickHandler.RowType.OpponentGraveyard:
                        opponentSelectedRow = opponentTransformGraveyard;
                        break;    
                    default:
                        selectedRow = null;
                        opponentSelectedRow = null;
                        break;
            }
            if(gameManager.actualState == GameManager.GameState.PlayerTurn)
            {
                if(selectedCard != null && selectedRow != null)
                {
                    if(selectedCard.transform.parent == transformPlayerHand)
                    {
                        HandleCardActivation(selectedCard, selectedRow);
                        selectedCard = null;
                        selectedRow = null;
                    }
                }
            }
            else if(gameManager.actualState == GameManager.GameState.OpponentTurn)
            {
                if(opponentSelectedCard != null && opponentSelectedRow != null)
                {
                    if(opponentSelectedCard.transform.parent == opponentTransformPlayerHand)
                    {
                        HandleCardActivation(opponentSelectedCard, opponentSelectedRow);
                        opponentSelectedCard = null;
                        opponentSelectedRow = null;   
                    }                
                }
            }
        }

        public void HandleCardActivation(GameObject cardObject, Transform row)
        {
            Card card = cardObject.GetComponent<DisplayCard>().card;
            if (card.isUnit)
            {
                string[] types = card.GetKind();
                if ((types[0] == "M" && row == transformMeleeRow) ||
                    (types[1] == "R" && row == transformRangedRow) ||
                    (types[2] == "S" && row == transformSeigeRow) ||
                    (types[0] == "M" && row == opponentTransformMeleeRow) ||
                    (types[1] == "R" && row == opponentTransformRangedRow) ||
                    (types[2] == "S" && row == opponentTransformSeigeRow))
                {
                    //Verificamos que la carta no sea Mahoraga ya que esta solo puede ser invcada por su ritual
                    DisplayCard aux = cardObject.GetComponent<DisplayCard>();
                    if(!aux.card.name.Equals("Divine General Mahoraga"))
                    {    
                        ActiveCard(cardObject, row);
                    }
                }
            }
            else
            {
                string[] type = card.GetKind();
                if ((type[0] == "Climate" || type[0] == "Dump") && 
                    (row == transformWeatherMeleeSlot || row == transformWeatherRangedSlot || row == transformWeatherSeigeSlot ||
                    row == opponentTransformWeatherMeleeSlot || row == opponentTransformWeatherRangedSlot || row == opponentTransformWeatherSeigeSlot))
                {
                    if(row.childCount == 0)
                    {   
                       ActiveCard(cardObject, row);
                    }
                }
                if (type[0] == "Increase" && (row == transformSpecialCardSlot || row == opponentTransformSpecialCardSlot))
                {
                    ActiveCard(cardObject, row);
                }
            }
        }

        public bool PlayerHasEarnigActions()
        {
            if(!playerHasPerformedAction) return true;
            else 
            {
                playerHasPerformedAction = false;
                return false;
            } 
        }

        public bool OpponentHasEarningActions()
        {
            if(!opponentHasPerformedAction) return true;
            else 
            {
                opponentHasPerformedAction = false;
                return false;
            } 
        }

        public void CleanBoard()
        {
            CleanRow(transformMeleeRow, transformGraveyard);
            CleanRow(transformRangedRow, transformGraveyard);
            CleanRow(transformSeigeRow, transformGraveyard);
            CleanRow(transformSpecialCardSlot, transformGraveyard);
            CleanRow(transformWeatherMeleeSlot, transformGraveyard);
            CleanRow(transformWeatherRangedSlot, transformGraveyard);
            CleanRow(transformWeatherSeigeSlot, transformGraveyard);

            CleanRow(opponentTransformMeleeRow, opponentTransformGraveyard);
            CleanRow(opponentTransformRangedRow, opponentTransformGraveyard);
            CleanRow(opponentTransformSeigeRow, opponentTransformGraveyard);
            CleanRow(opponentTransformSpecialCardSlot, opponentTransformGraveyard);
            CleanRow(opponentTransformWeatherMeleeSlot, opponentTransformGraveyard);
            CleanRow(opponentTransformWeatherRangedSlot, opponentTransformGraveyard);
            CleanRow(opponentTransformWeatherSeigeSlot, opponentTransformGraveyard);

            playerHasPerformedAction = false;
            opponentHasPerformedAction = false;
        }

        public void CleanRow(Transform row, Transform graveyard)
        {
            DisplayCard[] cardsToGraveyard = row.GetComponentsInChildren<DisplayCard>();

            foreach(DisplayCard c in cardsToGraveyard)
            {
                c.transform.SetParent(graveyard);
                c.transform.localPosition = Vector3.zero;
                c.transform.localRotation = Quaternion.identity;
                c.transform.localScale = Vector3.one;
            }
        }

        public void SelectObject(GameObject clicObject)
        {
            DisplayCard cardComponent = clicObject.GetComponent<DisplayCard>();
            RowClickHandler clicRow  = clicObject.GetComponent<RowClickHandler>();

            if(cardComponent != null)
            {
                effectSelectedCard = clicObject;
            }
            else if(clicRow.rowType == RowClickHandler.RowType.Melee 
                    || clicRow.rowType == RowClickHandler.RowType.Ranged 
                    || clicRow.rowType == RowClickHandler.RowType.Siege)
            {
                if(clicRow != null)
                {
                    effectSelectedRow.AddComponent<RowClickHandler>();
                    //effectSelectedRow.rowType = clicRow.rowType;
                }    
                effectSelectedRow = clicObject.transform;
            }
        }

        public Transform SelectionRow()
        {
            effectSelectedRow = null;
            opponentSelectedRow = null;
            selectedRow = null;

            while(effectSelectedRow == null)
            {
                Debug.Log("Selecciona una fila para activar el efecto");
                if(selectedRow != null)
                {
                    effectSelectedRow = selectedRow;
                }
                else if(opponentSelectedRow != null)
                {
                    effectSelectedRow = opponentSelectedRow;
                }
            }
            return effectSelectedRow;
        }

        public GameObject SelectionCard()
        {
            effectSelectedCard = null;
            opponentSelectedCard = null;
            selectedCard = null;

            while(effectSelectedCard == null)
            {
                Debug.Log("Selecciona una fila para activar el efecto");
                if(selectedCard != null)
                {
                    effectSelectedCard = selectedCard;
                }
                else if(opponentSelectedCard != null)
                {
                    effectSelectedCard = opponentSelectedCard;
                }
            }
            return effectSelectedCard;
        }    

        //===========================================================================

        public void SelectionRowIfNeeded(Action<Transform> effectAction)
        {
            if(effectCard != null && effectCard.NeedsRowSelection())
            {
                effectSelectedRow = null;
                Debug.Log("Selecciona una fila para seguir con el efecto");

                StartCoroutine(WaitForRowSelection(() =>
                {
                    if(effectSelectedRow != null)
                    {
                        effectAction(effectSelectedRow);
                    }
                    else Debug.LogWarning("No se ha seleccionado ninguna fila.");
                }));
            }
            else 
            {
                effectAction(null);
            }
        }

        public void SelectionCardIfNeeded(Action<GameObject> effectAction)
        {
            if (effectCard != null && effectCard.NeedsCardSelection())
            {
                effectSelectedCard = null;
                Debug.Log("Selecciona una carta para activar el efecto.");

                StartCoroutine(WaitForCardSelection(() =>
                {
                    if (effectSelectedCard != null)
                    {
                        effectAction(effectSelectedCard);
                    }
                    else
                    {
                        Debug.LogWarning("No se ha seleccionado ninguna carta.");
                    }
                }));
            }
            else
            {
                effectAction(null); // Si no es necesario seleccionar una carta
            }
        }

        private IEnumerator WaitForRowSelection(Action callback)
        {
            while (effectSelectedRow == null)
            {
                yield return null; // Espera hasta que se seleccione una fila
            }
            callback();
        }

        private IEnumerator WaitForCardSelection(Action callback)
        {
            while (effectSelectedCard == null)
            {
                yield return null; // Espera hasta que se seleccione una carta
            }
            callback();
        }
    }
