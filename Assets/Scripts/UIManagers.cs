using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagers : MonoBehaviour
{

    //instance of the gamemanger
   public static GameManager instance;
   public GameObject answer_panel;
   public GameObject question_panel;
   public GameObject character_grid;
   public GameObject character_notes;
   public GameObject game_over_panel;

    //display the correspoding panel according to the gamemanager state
    void displayUIPanels()
    {
        answer_panel.SetActive(true);
        question_panel.SetActive(true);
        character_grid.SetActive(true);
        character_notes.SetActive(true);
    }

}
