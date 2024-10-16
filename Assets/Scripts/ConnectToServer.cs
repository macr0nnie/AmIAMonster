using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    public  TMP_InputField usernameInput;
    public  TMP_Text buttonText;

    public void OnClickConnect(){
        if (usernameInput.text.Length >= 2)
        {
            PhotonNetwork.NickName = usernameInput.text; //sets the photn
            buttonText.text = "Connecting...";
            PhotonNetwork.ConnectUsingSettings();   
            print(PhotonNetwork.NickName);
        }
    }
  
}
