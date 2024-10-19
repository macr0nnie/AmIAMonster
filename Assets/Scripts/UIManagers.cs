using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Panels")]
    public GameObject startPanel;
    public GameObject questionPanel;
    public GameObject answerPanel;
    public GameObject guessSelectionPanel;
    public GameObject characterGridPanel;

    [Header("Input Fields")]
    public TMP_InputField questionInput;

    [Header("Text Displays")]
    public TMP_Text currentPlayerText;
    public TMP_Text questionDisplayText;
    public TMP_Text timerText;

    [Header("Buttons")]
    public Button submitQuestionButton;
    public Button yesButton;
    public Button noButton;
    public Button makeGuessButton;

    [Header("Character Grid")]
    public GameObject characterPrefab;
    public Transform characterGridContainer;

    private Dictionary<string, GameObject> characterUIElements = new Dictionary<string, GameObject>();

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

    public void ShowPanel(GameObject panel)
    {
        startPanel.SetActive(panel == startPanel);
        questionPanel.SetActive(panel == questionPanel);
        answerPanel.SetActive(panel == answerPanel);
        guessSelectionPanel.SetActive(panel == guessSelectionPanel);
        characterGridPanel.SetActive(panel == characterGridPanel);
    }

    public void UpdateCurrentPlayerText(string playerName)
    {
        currentPlayerText.text = $"Current Player: {playerName}";
    }

    public void DisplayQuestion(string question)
    {
        questionDisplayText.text = question;
    }

    public void UpdateTimer(float timeLeft)
    {
        timerText.text = $"Time: {Mathf.CeilToInt(timeLeft)}";
    }

    public void PopulateCharacterGrid(List<Character> characters)
    {
        foreach (Transform child in characterGridContainer)
        {
            Destroy(child.gameObject);
        }
        characterUIElements.Clear();

        foreach (var character in characters)
        {
            GameObject charObj = Instantiate(characterPrefab, characterGridContainer);
            CharacterUI charUI = charObj.GetComponent<CharacterUI>();
            charUI.Initialize(character);
            characterUIElements[character.Name] = charObj;
        }
    }

    public void CrossOutCharacter(string characterName)
    {
        if (characterUIElements.TryGetValue(characterName, out GameObject charObj))
        {
            CharacterUI charUI = charObj.GetComponent<CharacterUI>();
            charUI.ToggleCrossOut();
        }
    }

    public void HighlightCharacter(string characterName)
    {
        if (characterUIElements.TryGetValue(characterName, out GameObject charObj))
        {
            CharacterUI charUI = charObj.GetComponent<CharacterUI>();
            charUI.ToggleHighlight();
        }
    }

    public void EnableSubmitQuestionButton(bool enable)
    {
        submitQuestionButton.interactable = enable;
    }

    public void EnableAnswerButtons(bool enable)
    {
        yesButton.interactable = enable;
        noButton.interactable = enable;
    }

    public void EnableMakeGuessButton(bool enable)
    {
        makeGuessButton.interactable = enable;
    }

    public void ClearQuestionInput()
    {
        questionInput.text = "";
    }

    // Add more UI-related methods as needed
}