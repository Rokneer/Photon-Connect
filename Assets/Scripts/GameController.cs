using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform player1SpawPoint;
    [SerializeField] private Transform player2SpawPoint;
    
    private Color _playerColor;
    private GameObject tankInstance;

    void Awake()
    {
        if(playerPrefab == null) Debug.Log("Missing Player Prefab reference");
        else
        {
            Transform spawnPoint = (PhotonNetwork.IsMasterClient) ? player1SpawPoint : player2SpawPoint;

            object[] initData = new object[1];
            initData[0] = "Data instance";
            tankInstance = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, quaternion.identity, 0, initData);
        }
    }
    [PunRPC]
    public void CrearTankes()
    {
        _playerColor = (string)PhotonNetwork.LocalPlayer.CustomProperties["color"] switch
        {
            "Green" => Color.green,
            "Blue" => Color.blue,
            "Red" => Color.red,
            "Yellow" => Color.yellow,
            _ => Color.green
        };
        
        MeshRenderer[] renderers = tankInstance.GetComponentsInChildren<MeshRenderer>();
        foreach (var t in renderers)
        {
            t.material.color = _playerColor;
        }
    }
}
