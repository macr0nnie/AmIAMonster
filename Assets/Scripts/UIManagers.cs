using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [System.Serializable]
    private class UIPanel
    {
        public string name;
        public GameObject panel;
    }

    [SerializeField] private List<UIPanel> uiPanels = new List<UIPanel>();

    [Header("Common UI Elements")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI stateText;

    [Header("Character Selection")]
    [SerializeField] private TMP_Dropdown characterDropdown;

    [Header("Question Panel")]
    [SerializeField] private TMP_InputField questionInput;
    [SerializeField] private Button submitQuestionButton;

    [Header("Answer Panel")]
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    [Header("Results Panel")]
    [SerializeField] private TextMeshProUGUI resultsText;

    [Header("Guess Panel")]
    [SerializeField] private TMP_InputField guessInput;
    [SerializeField] private Button submitGuessButton;

    private Dictionary<string, GameObject> panelDictionary = new Dictionary<string, GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializePanelDictionary();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePanelDictionary()
    {
        foreach (var uiPanel in uiPanels)
        {
            panelDictionary[uiPanel.name] = uiPanel.panel;
        }
    }

    public void ShowPanel(string panelName)
    {
        if (panelDictionary.TryGetValue(panelName, out GameObject panel))
        {
            panel.SetActive(true);
        }
        else
        {
            Debug.LogWarning($"Panel {panelName} not found.");
        }
    }

    public void HidePanel(string panelName)
    {
        if (panelDictionary.TryGetValue(panelName, out GameObject panel))
        {
            panel.SetActive(false);
        }
        else
        {
            Debug.LogWarning($"Panel {panelName} not found.");
        }
    }

    public void HideAllPanels()
    {
        foreach (var panel in panelDictionary.Values)
        {
            panel.SetActive(false);
        }
    }

    public void UpdateTimer(float time)
    {
        timerText.text = $"Time: {time:F1}";
    }

    public void UpdateGameState(GameManager.GameState state)
    {
        stateText.text = $"State: {state}";
        HideAllPanels();
        switch (state)
        {
            case GameManager.GameState.WaitingForPlayers:
                ShowPanel("WaitingPanel");
                break;
          //  case GameManager.GameState.ChoosingCharacter:
            //    ShowPanel("CharacterSelectionPanel");
              //  break;
            case GameManager.GameState.AskingQuestions:
                ShowPanel("QuestionPanel");
                break;
            case GameManager.GameState.AnsweringQuestions:
                ShowPanel("AnswerPanel");
                break;
            case GameManager.GameState.DisplayingResults:
                ShowPanel("ResultsPanel");
                break;
            case GameManager.GameState.GuessingCharacter:
                ShowPanel("GuessPanel");
                break;
            case GameManager.GameState.GameOver:
                ShowPanel("GameOverPanel");
                break;
        }
    }

    public void SetupCharacterSelection(List<string> characters)
    {
        characterDropdown.ClearOptions();
        characterDropdown.AddOptions(characters);
    }

    public void SetupQuestionPanel(Action<string> onSubmitQuestion)
    {
        submitQuestionButton.onClick.RemoveAllListeners();
        submitQuestionButton.onClick.AddListener(() => onSubmitQuestion(questionInput.text));
    }

    public void SetupAnswerPanel(string question, Action<bool> onSubmitAnswer)
    {
        questionText.text = question;
        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();
        yesButton.onClick.AddListener(() => onSubmitAnswer(true));
        noButton.onClick.AddListener(() => onSubmitAnswer(false));
    }

    public void UpdateResults(string results)
    {
        resultsText.text = results;
    }

    public void SetupGuessPanel(Action<string> onSubmitGuess)
    {
        submitGuessButton.onClick.RemoveAllListeners();
        submitGuessButton.onClick.AddListener(() => onSubmitGuess(guessInput.text));
    }
}