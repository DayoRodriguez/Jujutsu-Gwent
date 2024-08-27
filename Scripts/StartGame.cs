using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public Button StartButton;
    public GameObject SelectionFactionPanel;
    public Text SelectionFactionText;

    public void StartGameButtonClick()
    {
        SelectionFactionText.text = "Player, Choose your Deck";
        StartButton.gameObject.SetActive(false);
        SelectionFactionPanel.SetActive(true);
        SelectionFactionText.gameObject.SetActive(true);
    }
}
