using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Counters : MonoBehaviour
{
    public Transform meleeRow;
    public Transform rangedRow;
    public Transform siegeRow;
    public Text textCounter; 

    // Update is called once per frame
    void Update()
    {
        //Revisar por que los contadores no se me muestran en la escena
        UpdatePower();
    }

    private void UpdatePower()
    {
        int power = Counter(meleeRow) + Counter(rangedRow) + Counter(siegeRow);
        //Debug.Log(power.ToString());
        textCounter.text = power.ToString();
    }

    private int Counter(Transform row)
    {
        DisplayCard[] cards = row.GetComponentsInChildren<DisplayCard>();
        int count = 0;

        foreach(DisplayCard c in cards)
        {
            count += c.card.GetPower();
        }
        return count;
    }
}
