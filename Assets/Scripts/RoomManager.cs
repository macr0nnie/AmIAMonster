using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviourPunCallbacks
{
    private const string JoinCodeKey = "JoinCode";
    private const int JoinCodeLength = 5;
    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        ConnectToPhotonServer();
    }

    private void ConnectToPhotonServer()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon server");
        JoinLobby();
    }

    private void JoinLobby()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Photon lobby");
    }

    public void CreateRoom()
    {
        string joinCode = GenerateJoinCode();
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = 6,
            CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { JoinCodeKey, joinCode } },
            CustomRoomPropertiesForLobby = new string[] { JoinCodeKey }
        };

        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    public void JoinRoomWithCode(string joinCode)
    {
        PhotonNetwork.JoinLobby();
        PhotonNetwork.JoinRoom(joinCode);
    }

    // Generate a random join code
    private string GenerateJoinCode()
    {
        const string chars = "abcdefghijklmnopqrstuvwxyz";
        return new string(Enumerable.Repeat(chars, JoinCodeLength)
            .Select(s => s[Random.Range(0, s.Length)]).ToArray());
    }
    //join room with code
    //update the room with the current join code
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Name);
        string joinCode = (string)PhotonNetwork.CurrentRoom.CustomProperties[JoinCodeKey];
        Debug.Log("Room join code: " + joinCode);

            PhotonNetwork.LoadLevel("Lobby");
    }
    //if the wrong code is entered or the room does not exist, display error message
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Failed to join room: " + message);
    }
}