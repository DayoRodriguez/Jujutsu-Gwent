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
            ResetCardTransform(card);

            
            DisplayCard cardD = card.GetComponent<DisplayCard>();
            
            if(cardD.card.owner == Card.Owner.Player)
            {
                playerHasPerformedAction = true;
            }
            else
            {
                opponentHasPerformedAction = true;
            }
            
            effectCard?.SetActivingCard(card);
            effectCard.Excute(card);
                       
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
            selectedRow = GetRowFromType(rowType);
            opponentSelectedRow = GetRowFromType(rowType);

            if (gameManager.actualState == GameManager.GameState.PlayerTurn && selectedCard != null && selectedRow != null)
            {
                if (IsCardInPlayerHand(selectedCard))
                {
                    HandleCardActivation(selectedCard, selectedRow);
                    ClearSelections();
                }
            }
            else if (gameManager.actualState == GameManager.GameState.OpponentTurn && opponentSelectedCard != null && opponentSelectedRow != null)
            {
                if (IsCardInOpponentHand(opponentSelectedCard))
                {
                    HandleCardActivation(opponentSelectedCard, opponentSelectedRow);
                    ClearSelections();
                }
            }
        }

        public void HandleCardActivation(GameObject cardObject, Transform row)
        {
            Card card = cardObject.GetComponent<DisplayCard>().card;
            string[] types = card.GetKind();

            if (card.isUnit && IsValidUnitRow(types, row))
            {
                
                if (!IsMahoragaCard(cardObject))
                {
                    ActiveCard(cardObject, row);    
                }
            }
            else if(IsValidSpecialRow(card, row))
            {                      
               ActiveCard(cardObject, row);
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

        //================Nuevos Codigos Optimizados=========================================
        private bool IsValidUnitRow(string[] types, Transform row)
        {
            return  (types[0] == "M" && row == transformMeleeRow) ||
                    (types[1] == "R" && row == transformRangedRow) ||
                    (types[2] == "S" && row == transformSeigeRow) ||
                    (types[0] == "M" && row == opponentTransformMeleeRow) ||
                    (types[1] == "R" && row == opponentTransformRangedRow) ||
                    (types[2] == "S" && row == opponentTransformSeigeRow);
        }
        
        private bool IsValidSpecialRow(Card card, Transform row)
        {
            string[] type = card.GetKind();
            if ((type[0] == "Climate" || type[0] == "Dump") && IsValidWeatherRow(row))
            {
                return row.childCount == 0;
            }
            return type[0] == "Increase" && IsSpecialSlot(row);
        }
        
        private bool IsValidWeatherRow(Transform row) =>
        row == transformWeatherMeleeSlot || row == transformWeatherRangedSlot || row == transformWeatherSeigeSlot ||
        row == opponentTransformWeatherMeleeSlot || row == opponentTransformWeatherRangedSlot || row == opponentTransformWeatherSeigeSlot;
        
        private bool IsSpecialSlot(Transform row) => row ==transformSpecialCardSlot || row == opponentTransformSpecialCardSlot;
        public void SelectionRowIfNeeded(Action<Transform> effectAction)
        {
            StartCoroutine(WaitForSelection<Transform>(effectSelectedRow => effectAction(effectSelectedRow), () => effectSelectedRow == null));
        }

        public void SelectionCardIfNeeded(Action<GameObject> effectAction)
        {
            StartCoroutine(WaitForSelection<GameObject>(effectSelectedCard => effectAction(effectSelectedCard), () => effectSelectedCard == null));
        }

        public IEnumerator WaitForSelection<T>(Action<T> effectAction, Func<bool> selectionNeeded)
        {
            while(selectionNeeded())
            {
                yield return null;
            }
            effectAction?.Invoke(default);
        }
        private void ResetCardTransform(GameObject card)
        {
            card.transform.localPosition = Vector3.zero;
            card.transform.localRotation = Quaternion.identity;
            card.transform.localScale = Vector3.one;
        }

        private bool IsCardInPlayerHand(GameObject card) => card.transform.parent == transformPlayerHand;
        private bool IsCardInOpponentHand(GameObject card) => card.transform.parent == opponentTransformPlayerHand;
        private bool IsMahoragaCard(GameObject cardObject)
        {
            DisplayCard aux = cardObject.GetComponent<DisplayCard>();
            return aux.card.name.Equals("Divine General Mahoraga");
        }

        private void ClearSelections()
        {
            selectedCard = null;
            selectedRow = null;
            opponentSelectedCard = null;
            opponentSelectedRow = null;
        }

        private Transform GetRowFromType(RowClickHandler.RowType rowType)
        {
            return rowType switch
            {
                RowClickHandler.RowType.Melee => transformMeleeRow,
                RowClickHandler.RowType.Ranged => transformRangedRow,
                RowClickHandler.RowType.Siege => transformSeigeRow,
                RowClickHandler.RowType.Special => transformSpecialCardSlot,
                RowClickHandler.RowType.WeatherMelee => transformWeatherMeleeSlot,
                RowClickHandler.RowType.WeatherRanged => transformWeatherRangedSlot,
                RowClickHandler.RowType.WeatherSiege => transformWeatherSeigeSlot,
                RowClickHandler.RowType.Leader => transformLeaderCardSlot,
                RowClickHandler.RowType.Deck => transformDeck,
                RowClickHandler.RowType.Hand => transformPlayerHand,
                RowClickHandler.RowType.Graveyard => transformGraveyard,
                RowClickHandler.RowType.OpponentMelee => opponentTransformMeleeRow,
                RowClickHandler.RowType.OpponentRanged => opponentTransformRangedRow,
                RowClickHandler.RowType.OpponentSiege => opponentTransformSeigeRow,
                RowClickHandler.RowType.OpponentSpecial => opponentTransformSpecialCardSlot,
                RowClickHandler.RowType.OpponentWeatherMelee => opponentTransformWeatherMeleeSlot,
                RowClickHandler.RowType.OpponentWeatherRanged => opponentTransformWeatherRangedSlot,
                RowClickHandler.RowType.OpponentWeatherSiege => opponentTransformWeatherSeigeSlot,
                RowClickHandler.RowType.OpponentLeader => opponentTransformLeaderCardSlot,
                RowClickHandler.RowType.OpponentDeck => opponentTransformDeck,
                RowClickHandler.RowType.OpponentHand => opponentTransformPlayerHand,
                RowClickHandler.RowType.OpponentGraveyard => opponentTransformGraveyard,
                _ => null,
            };
        }

        public Transform GetPlayerRowForCard(DisplayCard card)
        {
            if (card.card.GetKind()[0] == "Climate")
            {
                // Verificamos cuál fila de clima está libre (sin hijos)
                if (transformWeatherMeleeSlot.childCount == 0)
                {
                    return transformWeatherMeleeSlot;
                }
                else if (transformWeatherRangedSlot.childCount == 0)
                {
                    return transformWeatherRangedSlot;
                }
                else if (transformWeatherSeigeSlot.childCount == 0)
                {
                    return transformWeatherSeigeSlot;
                }
                else return null;
            }
            else if (card.card.GetKind()[0] == "Increase")
            {
            // Verificamos si la fila especial de incrementos está libre (sin hijos)
                if (transformSpecialCardSlot.childCount == 0)
                {
                    return transformSpecialCardSlot;
                }
                else return null;    
            }
            else return null;
        }

        public Transform GetOpponentRowForCard(DisplayCard card)
        {
            if (card.card.GetKind()[0] == "Climate")
            {
                // Verificamos cuál fila de clima está libre (sin hijos)
                if (opponentTransformWeatherMeleeSlot.childCount == 0)
                {
                    return opponentTransformWeatherMeleeSlot;
                }
                else if (opponentTransformWeatherRangedSlot.childCount == 0)
                {
                    return opponentTransformWeatherRangedSlot;
                }
                else if (opponentTransformWeatherSeigeSlot.childCount == 0)
                {
                    return opponentTransformWeatherSeigeSlot;
                }
                else return null;
            }
            else if (card.card.GetKind()[0] == "Increase")
            {
            // Verificamos si la fila especial de incrementos está libre (sin hijos)
                if (opponentTransformSpecialCardSlot.childCount == 0)
                {
                    return opponentTransformSpecialCardSlot;
                }
                else return null;    
            }
            else return null;
        }
    }
