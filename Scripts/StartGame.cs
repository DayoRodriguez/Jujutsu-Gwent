using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public AudioClip musicBackGround;
    private AudioSource musicControler;

    public Button StartButton;
    public GameObject SelectionFactionPanel;
    public Text SelectionFactionText;
    public Image startImage;
    void Start()
    {
        musicControler = gameObject.AddComponent<AudioSource>();
        musicControler.clip = musicBackGround;

        //Reproducir el audio en blucle
        musicControler.loop = true;

        musicControler.Play();
    }
    public void StartGameButtonClick()
    {
        SelectionFactionText.text = "Player, Choose your Deck";
        startImage.sprite = Resources.Load<Sprite>("SelectionDeckWallper");
        StartButton.gameObject.SetActive(false);
        SelectionFactionPanel.SetActive(true);
        SelectionFactionText.gameObject.SetActive(true);
    }
}
