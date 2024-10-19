using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
 
    private const int MAX_PLAYERS = 4;
    private const float ROUND_DURATION = 10f; // 1 minute per round (change this after playtesting)
    
    //the different game state elements when adding or removing states change the 
    public enum GameState { AskingQuestions, AnsweringQuestions, DisplayingResults, GuessingCharacter, GameOver } 
    
    private GameState currentState = GameState.AskingQuestions;
    private float roundTimer;
    private Dictionary<int, string> playerQuestions = new Dictionary<int, string>();
    private Dictionary<int, bool> playerAnswers = new Dictionary<int, bool>();
    private Dictionary<int, string> playerGuesses = new Dictionary<int, string>();

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartGame();
        }
      
    }

    private void Update()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            UpdateGameState();
        }
    }

    private void StartGame()
    {
        Debug.Log("bro does the start game function even work?");
        //i changed this value for testing should only start game if there are two players
        if (PhotonNetwork.CurrentRoom.PlayerCount >= 0)
        {
            Debug.Log("GM: Starting game");
            photonView.RPC("SetGameState", RpcTarget.All, (int)GameState.AskingQuestions);
        }
        else{
             Debug.Log("hmm strange there are no players in this room"); //check if the gamestate does not work
        }
       
    }

    //GM script works just need to enter the user name and all that fun fist <3
    private void UpdateGameState()
    {
        roundTimer -= Time.deltaTime;

        if (roundTimer <= 0)
        {   
            Debug.Log("GM: Time's up! Moving to the next state.");
            Debug.Log(currentState);

            switch (currentState)
            {
                case GameState.AskingQuestions:
                    photonView.RPC("SetGameState", RpcTarget.All, (int)GameState.AnsweringQuestions);
                    break;
                case GameState.AnsweringQuestions:
                    photonView.RPC("SetGameState", RpcTarget.All, (int)GameState.DisplayingResults);
                    break;
                case GameState.DisplayingResults:
                    photonView.RPC("SetGameState", RpcTarget.All, (int)GameState.GuessingCharacter);
                    break;
                case GameState.GuessingCharacter:
                    photonView.RPC("SetGameState", RpcTarget.All, (int)GameState.GameOver);
                    break;
            }
        }
    }

    [PunRPC]
    private void SetGameState(int newState)
    {
        currentState = (GameState)newState;
        roundTimer = ROUND_DURATION;

        if (currentState == GameState.AskingQuestions)
        {
            playerQuestions.Clear();
        }
        else if (currentState == GameState.AnsweringQuestions)
        {
            playerAnswers.Clear();
        }
        else if (currentState == GameState.GuessingCharacter)
        {
            playerGuesses.Clear();
        }
    }

    public void SubmitQuestion(string question)
    {
        if (currentState == GameState.AskingQuestions)
        {
            photonView.RPC("RecordQuestion", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber, question);
        }
    }

    [PunRPC]
    private void RecordQuestion(int playerID, string question)
    {
        playerQuestions[playerID] = question;
    }

    public void SubmitAnswer(bool answer)
    {
        if (currentState == GameState.AnsweringQuestions)
        {
            photonView.RPC("RecordAnswer", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber, answer);
        }
    }

    [PunRPC]
    private void RecordAnswer(int playerID, bool answer)
    {
        playerAnswers[playerID] = answer;
    }

    public void SubmitGuess(string characterGuess)
    {
        if (currentState == GameState.GuessingCharacter)
        {
            photonView.RPC("RecordGuess", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber, characterGuess);
        }
    }

    [PunRPC]
    private void RecordGuess(int playerID, string guess)
    {
        playerGuesses[playerID] = guess;
    }

    public GameState GetCurrentState()
    {
        return currentState;
    }

    public float GetRemainingTime()
    {
        return roundTimer;
    }

    public Dictionary<int, string> GetPlayerQuestions()
    {
        return playerQuestions;
    }

    public Dictionary<int, bool> GetPlayerAnswers()
    {
        return playerAnswers;
    }

    public Dictionary<int, string> GetPlayerGuesses()
    {
        return playerGuesses;
    }
}
