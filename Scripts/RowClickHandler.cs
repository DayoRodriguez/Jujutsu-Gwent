using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RowClickHandler : MonoBehaviour, IPointerClickHandler
{
    public enum RowType {Melee, Ranged, Siege, Special, WeatherMelee, WeatherRanged, WeatherSiege, Leader, Deck, Hand, Graveyard, 
    OpponentMelee, OpponentRanged, OpponentSiege, OpponentSpecial, OpponentWeatherMelee, OpponentWeatherRanged, OpponentWeatherSiege, OpponentLeader, OpponentDeck, OpponentHand, OpponentGraveyard}
    public RowType rowType;

    public static event System.Action<RowType> OnRowClicked;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnRowClicked?.Invoke(rowType);
    }
}
