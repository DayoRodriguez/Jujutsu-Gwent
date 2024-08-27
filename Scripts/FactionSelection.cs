using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactionSelection : MonoBehaviour
{
    public static FactionSelection Instance{get; private set;}

    public string playerFactionDeck;
    public string opponentFactionDeck;
    private bool playerSelectedDeck = false;

    public Button MagicianButton;
    public Button CurseButton;
    public GameObject SelectionFactionPanel;
    public Text TextSelectionPanel;


    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void OnFactionSelected(string faction)
    {
        if(!playerSelectedDeck)
        {
            playerSelectedDeck = true;
            playerFactionDeck = faction;
            TextSelectionPanel.text = "Opponent, Choose your Deck";
        }
        else
        {
            opponentFactionDeck = faction;
            UnityEngine.SceneManagement.SceneManager.LoadScene("PartidaEscena");
        }
    }

    // public void OnClilkButtonFaction()
    // {
    //     if(MagicianButton.GetComponentInChildren<Text>().text.Equals("Magician"))
    //     {
    //         OnFactionSelected("Magician");
    //     }
    //     else 
    //     {
    //         OnFactionSelected("Curse");
    //     }
    // }
}
