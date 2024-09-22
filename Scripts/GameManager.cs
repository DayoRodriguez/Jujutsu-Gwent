using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
  public enum GameState{FirstDraw, RockPaperScissors, PlayerTurn, OpponentTurn, RoundEnd, GameEnd, Draw}
  public GameState actualState;

  //public bool isMulliganPhase = false;
  private AudioSource musicControler;

  public bool playerHasWonRPS = false;

  public int playerRoundWin = 0;
  public int opponentRoundWin = 0;

  private bool playerhaspass = false;
  private bool opponenthaspass = false;

  public BoardManager board;
  public Text RoundResoultText;
  public Text PlayerPower;
  public Text OpponentPower;

  public GameObject rockPaperScissorsPanel;
  public GameObject winnerPanel;
  public Text rockPaperScissorsText;
  public Text winnerText;
  private string playerChoice = "";
  private string opponentChoice = "";

  PlayerDeck playerDeck;

  int playerPower = 0;
  int opponentPower = 0;

  void Start()  
  {
    actualState = GameState.RockPaperScissors;
    playerDeck = FindObjectOfType<PlayerDeck>();
    board = FindObjectOfType<BoardManager>();
    StartCoroutine(TurnManager());
  }

  // void Update()
  // {
  
  // }
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
        case GameState.Draw :
          yield return StartCoroutine(Draw());
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
    playerDeck.FirstDraw(board.transformPlayerHand, board.transformDeck);
        musicControler = gameObject.AddComponent<AudioSource>();
        musicControler.clip = board.gojoDominio;
        musicControler.Play();
        yield return new WaitForSeconds(2);
        musicControler.clip = board.vacio;
        musicControler.Play();
        yield return new WaitForSeconds(3);
    playerDeck.FirstDraw(board.opponentTransformPlayerHand, board.opponentTransformDeck);
        musicControler.clip = board.sukunaDominio;
        musicControler.Play();
        yield return new WaitForSeconds(3);
        musicControler.clip = board.templo;
        musicControler.Play();
        yield return new WaitForSeconds(3);
    //yield return StartCoroutine(playerDeck.FirtsDrawPhases());
    // yield return StartCoroutine(playerDeck.MulliganPhase(playerDeck.playerHand, playerDeck.playerDeck));
    // yield return StartCoroutine(playerDeck.MulliganPhase(playerDeck.opponentPlayerHand, playerDeck.opponentPlayerDeck));
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

  IEnumerator Draw()
  {
    playerDeck.DrawCard(board.transformPlayerHand, board.transformDeck, 2);
    playerDeck.DrawCard(board.opponentTransformPlayerHand, board.opponentTransformDeck, 2);
    yield return StartCoroutine(playerDeck.MulliganPhase(board.transformPlayerHand, board.transformDeck));
    yield return StartCoroutine(playerDeck.MulliganPhase(board.opponentTransformPlayerHand, board.opponentTransformDeck));
    if(playerPower > opponentPower)
    {
      actualState = GameState.PlayerTurn;
    }
    else if(opponentPower > playerPower)
    {
      actualState = GameState.OpponentTurn;
    }
    else 
    {
      actualState = GameState.PlayerTurn; 
    }
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
              winnerPanel.SetActive(true);
              winnerText.text = "Player wins, Player's turn";
              yield return new WaitForSeconds(1);
              winnerPanel.SetActive(false);
              playerHasWonRPS = true;
              actualState = GameState.FirstDraw;
            }
    else
    {
      winnerPanel.SetActive(true);
      winnerText.text = "Opponent wins, Opponent's turn";
      yield return new WaitForSeconds(1);
      winnerPanel.SetActive(false);
      actualState = GameState.FirstDraw;
    }        
  }

  IEnumerator PlayerTurn()
  {
    yield return new WaitUntil(() => playerhaspass || !board.PlayerHasEarnigActions());

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
    yield return new WaitUntil(() => opponenthaspass || !board.OpponentHasEarningActions());

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
    playerPower = Int32.Parse(PlayerPower.text);
    opponentPower = Int32.Parse(OpponentPower.text);

    if(playerPower > opponentPower)
    {
        playerRoundWin++;

        winnerPanel.SetActive(true);
        yield return new WaitForSeconds(1);
        winnerPanel.SetActive(false);
        winnerText.text = "Player has Won";
    }
    else if(playerPower < opponentPower)
    {
        opponentRoundWin++;

        winnerPanel.SetActive(true);
        yield return new WaitForSeconds(1);
        winnerPanel.SetActive(false);
        winnerText.text = "Opponent has Won";
    }
    else
    {
        playerRoundWin++;
        opponentRoundWin++;

        winnerPanel.SetActive(true);
        yield return new WaitForSeconds(1);
        winnerPanel.SetActive(false);
        winnerText.text = "There was a Draw";
    }

    yield return new WaitForSeconds(3);

    playerhaspass = false;
    opponenthaspass = false;

    board.CleanBoard();

    if((playerRoundWin >=2 && playerRoundWin > opponentRoundWin) || 
    (opponentRoundWin >= 2 && opponentRoundWin > playerRoundWin))
    {
        actualState = GameState.GameEnd; 
    }
    else
    {
        actualState = GameState.Draw; 
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


