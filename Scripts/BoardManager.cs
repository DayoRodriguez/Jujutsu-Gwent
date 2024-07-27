    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

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
        public Transform transformPayerHand;
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

        public void ActiveCard(GameObject card, Transform row)
        {
            card.transform.SetParent(row);
            card.transform.localPosition = Vector3.zero;
            card.transform.localRotation = Quaternion.identity;
            card.transform.localScale = Vector3.one;
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
                        selectedRow = transformPayerHand;
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

            if(selectedCard != null && selectedRow != null)
            {
                HandleCardActivation(selectedCard, selectedRow);
                selectedCard = null;
                selectedRow = null;   
            }
            if(opponentSelectedCard != null && opponentSelectedRow != null)
            {
                HandleCardActivation(opponentSelectedCard, opponentSelectedRow);
                opponentSelectedCard = null;
                opponentSelectedRow = null;   
            }
        }

        private void HandleCardActivation(GameObject cardObject, Transform row)
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
                ActiveCard(cardObject, row);
            }
        }
        else
        {
            string[] type = card.GetKind();
            if ((type[0] == "Climate" || type[0] == "Dump") && 
                (row == transformWeatherMeleeSlot || row == transformWeatherRangedSlot || row == transformWeatherSeigeSlot ||
                row == opponentTransformWeatherMeleeSlot || row == opponentTransformWeatherRangedSlot || row == opponentTransformWeatherSeigeSlot))
            {
                ActiveCard(cardObject, row);
            }
            if (type[0] == "Increase" && (row == transformSpecialCardSlot || row == opponentTransformSpecialCardSlot))
            {
                ActiveCard(cardObject, row);
            }
        }
    }
}
