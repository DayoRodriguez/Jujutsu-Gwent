using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateLeaderEffect : MonoBehaviour
{
    public CardEffects activeEffect;

    public Transform leaderPlayerSlot;

    public Transform leaderOppSlot;

    private BoardManager board;
    void Start()
    {
        activeEffect = FindObjectOfType<CardEffects>();
        board = FindObjectOfType<BoardManager>();
    }

    public void ActivateLeaderplayerEffect()
    {
        DisplayCard playerLeader = leaderPlayerSlot.GetComponentInChildren<DisplayCard>();
        activeEffect.Execute(playerLeader.gameObject);
        board.playerHasPerformedAction = true;
    }

    public void ActivateOppLeaderEffect()
    {
        DisplayCard oppLeader = leaderOppSlot.GetComponentInChildren<DisplayCard>();
        activeEffect.Execute(oppLeader.gameObject);
        board.opponentHasPerformedAction = true;
    }
}
