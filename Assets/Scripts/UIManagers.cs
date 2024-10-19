using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

public class UIManagers : MonoBehaviourPunCallbacks
{
    public static UIManagers Instance { get; private set; }

    [Header("Panels")]
    public GameObject answer_panel;
    public GameObject question_panel;
    public GameObject character_grid;
    public GameObject character_notes;
    public GameObject game_over_panel;
    public GameObject playerlist;
    public GameObject settings;

    [Header("UI Elements")]
    public TMP_Text questionText;
    public TMP_InputField questionInput;
    public Button submitQuestionButton;
    public Button yesButton;
    public Button noButton;
    public TMP_Text currentPlayerText;
    public TMP_Text timerText;
    public TMP_Text stateText;

    private GameManager gameManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene!");
        }

        SetupButtonListeners();
        HideAllPanels();
    }

    void Update()
    {
        UpdateTimer();
        UpdateStateText();
    }

    private void SetupButtonListeners()
    {
        submitQuestionButton.onClick.AddListener(OnSubmitQuestionClicked);
        yesButton.onClick.AddListener(() => OnAnswerClicked(true));
        noButton.onClick.AddListener(() => OnAnswerClicked(false));
    }

    public void UpdateUIForGameState(GameManager.GameState state)
    {
        HideAllPanels();
        switch (state)
        {
            case GameManager.GameState.AskingQuestions:
                question_panel.SetActive(true);
                break;
            case GameManager.GameState.AnsweringQuestions:
                answer_panel.SetActive(true);
                break;
            case GameManager.GameState.DisplayingResults:
                // Implement a results panel if needed
                break;
            case GameManager.GameState.GuessingCharacter:
                character_grid.SetActive(true);
                break;
            case GameManager.GameState.GameOver:
                game_over_panel.SetActive(true);
                break;
        }
    }

    public void DisplayQuestion(string question)
    {
        questionText.text = question;
    }

    public void UpdateCurrentPlayer(string playerName)
    {
        currentPlayerText.text = $"Current Player: {playerName}";
    }

    private void UpdateTimer()
    {
        float remainingTime = gameManager.GetRemainingTime();
        timerText.text = $"Time: {Mathf.CeilToInt(remainingTime)}";
    }

    private void UpdateStateText()
    {
        stateText.text = $"Current State: {gameManager.GetCurrentState()}";
    }

    public void TogglePlayerList(bool show)
    {
        playerlist.SetActive(show);
    }

    public void ToggleSettings(bool show)
    {
        settings.SetActive(show);
    }

    public void ToggleCharacterNotes(bool show)
    {
        character_notes.SetActive(show);
    }

    private void HideAllPanels()
    {
        answer_panel.SetActive(false);
        question_panel.SetActive(false);
        character_grid.SetActive(false);
        game_over_panel.SetActive(false);
        playerlist.SetActive(false);
        settings.SetActive(false);
    }

    // UI Input methods
    public void OnSubmitQuestionClicked()
    {
        string question = questionInput.text;
        gameManager.SubmitQuestion(question);
        questionInput.text = "";
    }

    public void OnAnswerClicked(bool isYes)
    {
        gameManager.SubmitAnswer(isYes);
    }

    public void OnGuessSubmitted(string characterGuess)
    {
        gameManager.SubmitGuess(characterGuess);
    }

    // Method to update player list UI
    public void UpdatePlayerListUI(Dictionary<int, Player> players)
    {
        // Clear existing player list UI
        // For each player in the dictionary, add their name to the UI
        // You'll need to implement the specific UI for this
    }

    // Photon callback
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerListUI(PhotonNetwork.CurrentRoom.Players);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerListUI(PhotonNetwork.CurrentRoom.Players);
    }
}