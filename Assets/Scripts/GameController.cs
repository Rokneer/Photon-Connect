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
    void Awake()
    {
        if(playerPrefab == null) Debug.Log("Missing Player Prefab reference");
        else
        {
            Transform spawnPoint = (PhotonNetwork.IsMasterClient) ? player1SpawPoint : player2SpawPoint;

            object[] initData = new object[1];
            initData[0] = "Data instance";
            PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, quaternion.identity, 0, initData);
        }
    }
}
