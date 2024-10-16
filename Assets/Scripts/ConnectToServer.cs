using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    public TMP_InputField usernameInput;
    public TMP_Text buttonText;
    public bool configured_name = false;

    public void OnClickConnect()
    {
        if (usernameInput.text.Length >= 2)
        {
            PhotonNetwork.NickName = usernameInput.text;
            configured_name = true;
            buttonText.text = "Connecting...";

            if (PhotonNetwork.IsConnected)
            {
                Debug.Log("Already connected to Photon, attempting to join lobby");
                OnConnectedToMaster();
            }
            else
            {
                Debug.Log("Connecting to Photon...");
                PhotonNetwork.ConnectUsingSettings();
            }

            print(PhotonNetwork.NickName);
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster called");
        Debug.Log($"configured_name: {configured_name}");

        if (configured_name == true)
        {
            Debug.Log("Connected to Photon Master Server");
            PhotonNetwork.LoadLevel("Lobby");
        }
        else
        {
            Debug.Log("Name not configured");

        }

    }
}
