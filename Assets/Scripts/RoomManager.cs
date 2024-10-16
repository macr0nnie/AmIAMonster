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
    public Button startGameButton; //allow the host to start the game deactivate when done

    private void Start()
    {
        // Ensure all clients load the same scene when the master client switches scenes
        PhotonNetwork.AutomaticallySyncScene = true;
        
        // Attempt to connect to the Photon server
        ConnectToPhotonServer();

        // Set up button listeners
        createRoomButton.onClick.AddListener(CreateRoom);
        joinRoomButton.onClick.AddListener(() => JoinRoomWithCode(joinCodeInput.text));

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

    // Called when successfully connected to the Photon server
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon server");
        PhotonNetwork.JoinLobby();
    }

    // Called when successfully joined the Photon lobby
    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Photon lobby");
        UpdateUI();
    }

    // Create a new room with a random join code
    public void CreateRoom()
    {
        string joinCode = GenerateJoinCode();
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = 6, // Customize: Set the maximum number of players
            CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { JoinCodeKey, joinCode } },
            CustomRoomPropertiesForLobby = new string[] { JoinCodeKey }
        };

        PhotonNetwork.CreateRoom(joinCode, roomOptions);
    }

    // Attempt to join a room using the provided join code
    public void JoinRoomWithCode(string joinCode)
    {
        if (string.IsNullOrEmpty(joinCode) || joinCode.Length != JoinCodeLength)
        {
            Debug.LogError("Invalid join code");
            return;
        }

        PhotonNetwork.JoinRoom(joinCode);
    }

    // Generate a random join code
    private string GenerateJoinCode()
    {
        const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, JoinCodeLength)
            .Select(s => s[Random.Range(0, s.Length)]).ToArray());
    }

    // Called when successfully joined a room
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Name);
        string joinCode = (string)PhotonNetwork.CurrentRoom.CustomProperties[JoinCodeKey];
        Debug.Log("Room join code: " + joinCode);

        UpdateUI();
    }

    // Called when failed to join a room
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Failed to join room: " + message);
    }

    // Update UI elements based on current connection state
    private void UpdateUI()
    {
        bool inRoom = PhotonNetwork.InRoom;
        bool inLobby = PhotonNetwork.InLobby;

        createRoomButton.interactable = inLobby && !inRoom;
        joinRoomButton.interactable = inLobby && !inRoom;
        joinCodeInput.interactable = inLobby && !inRoom;

        if (inRoom)
        {
            string joinCode = (string)PhotonNetwork.CurrentRoom.CustomProperties[JoinCodeKey];
            roomCodeText.text = "Room Code: " + joinCode;
        }
        else
        {
            roomCodeText.text = "Join a Game!";
        }
    }

    // Called when the player leaves a room
    public override void OnLeftRoom()
    {
        UpdateUI();
    }
}

