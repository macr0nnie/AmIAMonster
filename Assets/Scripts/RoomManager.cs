using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class RoomManager : MonoBehaviourPunCallbacks
{
    // Constants for room properties
    private const string JoinCodeKey = "JoinCode";
    private const int JoinCodeLength = 6;

    // UI elements - assign these in the Unity Inspector
    public Button createRoomButton;
    public Button joinRoomButton;
    public TMP_InputField joinCodeInput;
    public TextMeshProUGUI roomCodeText;
    public Button startGameButton;

    private void Start()
    {
        // Ensure all clients load the same scene when the master client switches scenes
        PhotonNetwork.AutomaticallySyncScene = true;
        
        // Attempt to connect to the Photon server
        ConnectToPhotonServer();

        // Set up button listeners
        createRoomButton.onClick.AddListener(CreateRoom);
        joinRoomButton.onClick.AddListener(() => JoinRoomWithCode(joinCodeInput.text));
        startGameButton.onClick.AddListener(StartGame);

        // Initialize UI state
        UpdateUI();
    }

    private void ConnectToPhotonServer()
    {
        // Connect to Photon if not already connected
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon server");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Photon lobby");
        UpdateUI();
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

        PhotonNetwork.CreateRoom(joinCode, roomOptions);
    }

    public void JoinRoomWithCode(string joinCode)
    {
        if (string.IsNullOrEmpty(joinCode) || joinCode.Length != JoinCodeLength)
        {
            Debug.LogError("Invalid join code");
            return;
        }

        PhotonNetwork.JoinRoom(joinCode);
    }

    private string GenerateJoinCode()
    {
        const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, JoinCodeLength)
            .Select(s => s[Random.Range(0, s.Length)]).ToArray());
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Name);
        string joinCode = (string)PhotonNetwork.CurrentRoom.CustomProperties[JoinCodeKey];
        Debug.Log("Room join code: " + joinCode);

        UpdateUI();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Failed to join room: " + message);
    }

    private void UpdateUI()
    {
        bool inRoom = PhotonNetwork.InRoom;
        bool inLobby = PhotonNetwork.InLobby;

        //createRoomButton.interactable = inLobby && !inRoom;
        //joinRoomButton.interactable = inLobby && !inRoom;
        //joinCodeInput.interactable = inLobby && !inRoom;
        startGameButton.gameObject.SetActive(inRoom && PhotonNetwork.IsMasterClient);

        if (inRoom)
        {
            string joinCode = (string)PhotonNetwork.CurrentRoom.CustomProperties[JoinCodeKey];
            roomCodeText.text = "Room Code: " + joinCode;
        }
        else
        {
            roomCodeText.text = "Not in a room";
        }
    }

    public override void OnLeftRoom()
    {
        UpdateUI();
    }

    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LoadLevel("Game");
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        UpdateUI();
    }
}
