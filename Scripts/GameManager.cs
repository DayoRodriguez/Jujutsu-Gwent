using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
  public enum GameState{FirstDraw, RockPaperScissors, PlayerTurn, OpponentTurn, RoundEnd, GameEnd}
  public GameState actualState;

  public bool playerHasWonRPS = false;

  public int playerRoundWin = 0;
  public int opponentRoundWin = 0;

  private bool playerhaspass = false;
  private bool opponenthaspass = false;

  public BoardManager boardManager;
  public Text RoundResoultText;
  public Text PlayerPower;
  public Text OpponentPower;

  public GameObject rockPaperScissorsPanel;
  public GameObject RPSwinnerPanel;
  public Text rockPaperScissorsText;
  public Text RPSwinnerText;
  private string playerChoice = "";
  private string opponentChoice = "";

  void Start()  
  {
    actualState = GameState.RockPaperScissors;
    StartCoroutine(TurnManager());
  }
  IEnumerator TurnManager()
  {
    while(actualState != GameState.GameEnd)
    {
      switch(actualState)
      {
        case GameState.FirstDraw :
          yield return StartCoroutine(FirstDraw());
          break;
        case GameState.RockPaperScissors:
          yield return StartCoroutine(RockPaperScissors());
          break;
        case GameState.PlayerTurn :
          yield return StartCoroutine(PlayerTurn());
          break;
        case GameState.OpponentTurn :
          yield return StartCoroutine(OpponentTurn());
          break;
        case GameState.RoundEnd :
          yield return StartCoroutine(RoundEnd());
          break;        
      }
    }

    ShowFinalResoult();
  }

  IEnumerator FirstDraw()
  {
    PlayerDeck playerDeck = FindObjectOfType<PlayerDeck>();
    playerDeck.FirstDraw(playerDeck.PlayerHand, playerDeck.playerDeck);
    playerDeck.FirstDraw(playerDeck.OpponentPlayerHand, playerDeck.OpponentPlayerDeck);
    StartCoroutine(playerDeck.FirtsDrawPhases());
    yield return new WaitUntil(() => !playerDeck.StartedSMS.activeSelf);
    if(playerHasWonRPS) actualState = GameState.PlayerTurn;
    else actualState = GameState.OpponentTurn;
  }

  IEnumerator RockPaperScissors()
  {
    rockPaperScissorsPanel.SetActive(true);
    rockPaperScissorsText.text = "Player, choose Rock, Paper, Scissors";

    yield return new WaitUntil(() => playerChoice != "");

    rockPaperScissorsPanel.SetActive(false);

    yield return new WaitForSeconds(1);

    rockPaperScissorsPanel.SetActive(true);
    rockPaperScissorsText.text = "Opponent, choose Rock, Paper, Scissors";

    yield return new WaitUntil(() => opponentChoice != "");

    rockPaperScissorsPanel.SetActive(false);

    yield return StartCoroutine(DeterminateRPSWinner());
  }

  public void ChooseRock()
  {
    if(rockPaperScissorsText.text.Contains("Player")) playerChoice = "Rock";
    else if(rockPaperScissorsText.text.Contains("Opponent")) opponentChoice = "Rock";
  }
  public void ChoosePaper()
  {
    if(rockPaperScissorsText.text.Contains("Player")) playerChoice = "Paper";
    else if(rockPaperScissorsText.text.Contains("Opponent")) opponentChoice = "Paper";
  }
  public void ChooseScissors()
  {
    if(rockPaperScissorsText.text.Contains("Player")) playerChoice = "Scissors";
    else if(rockPaperScissorsText.text.Contains("Opponent")) opponentChoice = "Scissors";
  }
  
  private IEnumerator DeterminateRPSWinner()
  {
    if(playerChoice == opponentChoice)
    {
      playerChoice = "";
      opponentChoice = "";
      StartCoroutine(RockPaperScissors());
    }
    else if((playerChoice == "Rock" && opponentChoice == "Scissors") ||
            (playerChoice == "Paper" && opponentChoice == "Rock") ||
            (playerChoice == "Scissors" && opponentChoice == "Paper"))
            {
              RPSwinnerPanel.SetActive(true);
              RPSwinnerText.text = "Player wins, Player's turn";
              yield return new WaitForSeconds(1);
              RPSwinnerPanel.SetActive(false);
              playerHasWonRPS = true;
              actualState = GameState.FirstDraw;
            }
    else
    {
      RPSwinnerPanel.SetActive(true);
      RPSwinnerText.text = "Opponent wins, Opponent's turn";
      yield return new WaitForSeconds(1);
      RPSwinnerPanel.SetActive(false);
      actualState = GameState.FirstDraw;
    }        
  }

  IEnumerator PlayerTurn()
  {
    yield return new WaitUntil(() => playerhaspass || !boardManager.PlayerHasEarnigActions());

    if(playerhaspass && opponenthaspass)
    {
        actualState = GameState.RoundEnd;
    }
    else
    {
        actualState = GameState.OpponentTurn;
    }
  }

  
  IEnumerator OpponentTurn()
  {
    yield return new WaitUntil(() => opponenthaspass || !boardManager.OpponentHasEarningActions());

    if(playerhaspass && opponenthaspass)
    {
        actualState = GameState.RoundEnd;
    }
    else
    {
        actualState = GameState.PlayerTurn;
    }
  }
  
  IEnumerator RoundEnd()
  {
    int playerPower = Int32.Parse(PlayerPower.text);
    int opponentPower = Int32.Parse(OpponentPower.text);

    if(playerPower > opponentPower)
    {
        playerRoundWin++;
        RoundResoultText.text = "Player has Won";
    }
    if(playerPower < opponentPower)
    {
        opponentRoundWin++;
        RoundResoultText.text = "Opponent has Won";
    }
    else
    {
        playerRoundWin++;
        opponentRoundWin++;
        RoundResoultText.text = "There was a Draw";
    }

    yield return new WaitForSeconds(3);

    playerhaspass = false;
    opponenthaspass = false;

    boardManager.CleanBoard();

    if((playerRoundWin >=2 && playerRoundWin > opponentRoundWin) || 
    (opponentRoundWin >= 2 && opponentRoundWin > playerRoundWin))
    {
        actualState = GameState.GameEnd; 
    }
    else
    {
        if(playerPower > opponentPower)
        {
            actualState = GameState.PlayerTurn;
        }
        else if(opponentPower > playerPower)
        {
            actualState = GameState.OpponentTurn;
        }
    }
  }

  void ShowFinalResoult()
  {
    if(playerRoundWin >=2 && playerRoundWin > opponentRoundWin)
    {
        RoundResoultText.text = "Player has Won the Game";
    }
    else if(opponentRoundWin >= 2 && opponentRoundWin > playerRoundWin)
    {
        RoundResoultText.text = "Opponent has won the Game";
    }
  }

  public void PlayerPass()
  {
    playerhaspass = true;
  }
  public void OpponentPass()
  {
    opponenthaspass = true;
  }
}


