using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public enum RegionCode
{
    AUTO,
    CAE,
    EU,
    US,
    USW,
    SA
}

public class ConnectController : MonoBehaviourPunCallbacks
{
    [SerializeField] private string gameVersion = "1";
    [SerializeField] private string regionCode = null;
    [SerializeField] private GameObject panelConnect;
    [SerializeField] private GameObject panelRoom;
    [SerializeField] private GameObject playerTank;
    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    public void SetRegion(int index)
    {
        RegionCode region = (RegionCode)index;
        regionCode = region == RegionCode.AUTO ? null : region.ToString();
        
        Debug.Log("Region selected: " + regionCode);
        
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = regionCode;
    }
    public void Connect()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }
    void SetButton(bool state, string msg)
    {
        GameObject.Find("Button").GetComponentInChildren<Text>().text = msg;
        GameObject.Find("Button").GetComponent<Button>().enabled = state;
    }
    void ShowRoomPanel()
    {
        panelConnect.SetActive(false);
        panelRoom.SetActive(true);
    }
    public void SetColor(int index)
    {
        string color = GameObject.Find("DropdownColors").GetComponent<Dropdown>().options[index].text;
        Debug.Log("Color: " + color);
        var propsToSet = new ExitGames.Client.Photon.Hashtable() { { "color", color } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(propsToSet);
    }
    public void SetReady()
    {
        var propsToSet = new ExitGames.Client.Photon.Hashtable() { { "ready", true } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(propsToSet);
    }
    
    #region MonoBehavioursPunCallbacks Callbacks

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() was called by PUN");
        SetButton(true, "Let's Battle");
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("OnDisconnected() was called by PUN with reason {0}", cause);
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed() was called by PUN. No rooms available, so we created one. \nCalling: PhotonNetwork.CreateRoom");
        PhotonNetwork.CreateRoom(null, new RoomOptions());
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Launcher: OnJoinedRoom() called by PUN. Now this client is in a room");
        SetButton(false, "Wating for Players");
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            Debug.Log("Room is Ready");
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + "Number of Player is room: " + PhotonNetwork.CurrentRoom.PlayerCount);
        
        if (PhotonNetwork.CurrentRoom.PlayerCount != 2 && PhotonNetwork.IsMasterClient) return;
        Debug.Log("Room is full");
        ShowRoomPanel();

    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey("color"))
        {
            var playerColor = playerTank.GetComponentInChildren<Renderer>().material;
            if (changedProps.ContainsValue("Green")) playerColor.SetColor(BaseColor, Color.green);
            else if (changedProps.ContainsValue("Blue")) playerColor.SetColor(BaseColor, Color.blue);
            else if (changedProps.ContainsValue("Red")) playerColor.SetColor(BaseColor, Color.red);
            else if (changedProps.ContainsValue("Yellow")) playerColor.SetColor(BaseColor, Color.yellow);
        }
        if (changedProps.ContainsKey("ready"))
        {
            int playersReady = 0;
            foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
            {
                bool ready = (bool)player.CustomProperties["ready"];
                Debug.Log(player.NickName + "is ready? " + ready);
                
                if (ready) playersReady++;
                if (playersReady == PhotonNetwork.CurrentRoom.MaxPlayers) PhotonNetwork.LoadLevel("Game");
            }
        }
    }

    #endregion
}
