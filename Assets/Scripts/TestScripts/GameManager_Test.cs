using UnityEngine;
using System.Collections;
using Photon.Pun;

public class GameManager_Test : MonoBehaviourPunCallbacks
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene!");
            return;
        }

        StartCoroutine(TestGameFlow());
    }

    private IEnumerator TestGameFlow()
    {
        Debug.Log("Starting GameManager test...");

        // Wait for connection to Photon
        yield return new WaitUntil(() => PhotonNetwork.IsConnectedAndReady);
        Debug.Log("Connected to Photon");

        // Create or join a room
        if (!PhotonNetwork.InRoom)
        {
            PhotonNetwork.JoinRandomRoom();
        }

        yield return new WaitUntil(() => PhotonNetwork.InRoom);
        Debug.Log($"Joined room: {PhotonNetwork.CurrentRoom.Name}");

        // Wait for the game to start
        yield return new WaitUntil(() => gameManager.GetCurrentState() != GameManager.GameState.WaitingForPlayers);
        Debug.Log("Game started");

        // Simulate game flow
        while (gameManager.GetCurrentState() != GameManager.GameState.GameOver)
        {
            GameManager.GameState currentState = gameManager.GetCurrentState();
            float remainingTime = gameManager.GetRemainingTime();

            Debug.Log($"Current State: {currentState}, Time Remaining: {remainingTime:F2}");

            switch (currentState)
            {
                case GameManager.GameState.AskingQuestions:
                    if (remainingTime < 55f && PhotonNetwork.LocalPlayer.ActorNumber == 1)
                    {
                        gameManager.SubmitQuestion("Is the character male?");
                        Debug.Log("Submitted question: Is the character male?");
                    }
                    break;

                case GameManager.GameState.AnsweringQuestions:
                    if (remainingTime < 55f && PhotonNetwork.LocalPlayer.ActorNumber == 2)
                    {
                        gameManager.SubmitAnswer(true);
                        Debug.Log("Submitted answer: Yes");
                    }
                    break;

                case GameManager.GameState.GuessingCharacter:
                    if (remainingTime < 55f && PhotonNetwork.LocalPlayer.ActorNumber == 1)
                    {
                        gameManager.SubmitGuess("John");
                        Debug.Log("Submitted guess: John");
                    }
                    break;
            }

            yield return new WaitForSeconds(1f);
        }

        Debug.Log("Game Over");
        LogGameResults();
    }

    private void LogGameResults()
    {
        Debug.Log("Game Results:");
        foreach (var question in gameManager.GetPlayerQuestions())
        {
            Debug.Log($"Player {question.Key} asked: {question.Value}");
        }

        foreach (var answer in gameManager.GetPlayerAnswers())
        {
            Debug.Log($"Player {answer.Key} answered: {answer.Value}");
        }

        foreach (var guess in gameManager.GetPlayerGuesses())
        {
            Debug.Log($"Player {guess.Key} guessed: {guess.Value}");
        }
    }
}
