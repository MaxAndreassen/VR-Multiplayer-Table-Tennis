using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[System.Serializable]
public class DefaultRoom
{
    public string Name;
    
    public int SceneIndex;
    
    public int MaxPlayer;
}

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public List<DefaultRoom> defaultRooms;
    public GameObject roomUI;
    
    public void ConnectToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Try Connect To Server...");
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Server.");
        
        base.OnConnectedToMaster();

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        
        Debug.Log("Joined The Lobby.");
        
        roomUI.SetActive(true);
    }

    public void InitializeRoom(int defaultRoomIndex)
    {
        var roomSettings = defaultRooms[defaultRoomIndex];
        
        PhotonNetwork.LoadLevel(roomSettings.SceneIndex);
        
        PhotonNetwork.JoinOrCreateRoom(
            roomSettings.Name,
            new RoomOptions
            {
                MaxPlayers = (byte)roomSettings.MaxPlayer,
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
