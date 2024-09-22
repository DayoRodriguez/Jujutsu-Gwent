using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateLeaderEffect : MonoBehaviour
{
    public CardEffects activeEffect;

    public Transform leaderPlayerSlot;

    public Transform leaderOppSlot;

    private BoardManager board;

    private AudioSource musicControler;

    private bool playerLeaderEffectActivate = false;
    private bool oppLeaderEffectActivate = false;
    void Start()
    {
        activeEffect = FindObjectOfType<CardEffects>();
        board = FindObjectOfType<BoardManager>();
    }

    public void ActivateLeaderplayerEffect()
    {
        if(!playerLeaderEffectActivate)
        {
            DisplayCard playerLeader = leaderPlayerSlot.GetComponentInChildren<DisplayCard>();
            musicControler = gameObject.AddComponent<AudioSource>();
            musicControler.clip = board.debil;
            musicControler.Play();
            activeEffect.Execute(playerLeader.gameObject);
            board.playerHasPerformedAction = true;
            playerLeaderEffectActivate = true;
        }
    }

    public void ActivateOppLeaderEffect()
    {
        if(!oppLeaderEffectActivate)
        {
            DisplayCard oppLeader = leaderOppSlot.GetComponentInChildren<DisplayCard>();
            musicControler = gameObject.AddComponent<AudioSource>();
            musicControler.clip = board.animo;
            musicControler.Play();
            activeEffect.Execute(oppLeader.gameObject);
            board.opponentHasPerformedAction = true;
            oppLeaderEffectActivate = true;
        }
    }
}
