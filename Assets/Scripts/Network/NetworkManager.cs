using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks {

    // Start is called before the first frame update
    void Start()
    {
        ConnectToServer();
    }

    void ConnectToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Try Connect To Server...");
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Server.");
        
        base.OnConnectedToMaster();

        PhotonNetwork.JoinOrCreateRoom(
            "Room 1",
            new RoomOptions
            {
                MaxPlayers = 10,
                IsVisible = true,
                IsOpen = true
            },
            TypedLobby.Default
        );
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room.");
        
        base.OnJoinedRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("A new player joined the room.");
        base.OnPlayerEnteredRoom(newPlayer);
    }
}
