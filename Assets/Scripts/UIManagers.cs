using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagers : MonoBehaviour
{

   public GameObject answer_panel; //synced to everyone
   public GameObject question_panel; //synced to everyone
   public GameObject character_grid; //synced to everyone
   public GameObject character_notes; //not synced everyone has different notes 
   public GameObject game_over_panel; //synced to everyone but the winner gets a different message and different background color
   public GameObject playerlist; //individual players are able to check who is in the lobby by hitting tab
   public GameObject settings; //hitting esc will bring up the settings menu 
   
    //update global UI
    //when the game state changes from the game manager all players are displayed UI for that state
    
    //UI that is not affected by game state, indivial displays
  
   



    void Start()
    {
        
    }
}
