using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class MineSystem : MonoBehaviourPun
{
    [SerializeField] private GameObject minePrefab;
    [SerializeField] private Transform mineSpawn;
    
    void Update()
    {
        if (photonView.IsMine && Input.GetKeyUp(KeyCode.Return))
        {
            photonView.RPC("SetMine", RpcTarget.AllBuffered);
            SetMine();
        }
    }
    [PunRPC]
    void SetMine()
    {
        Debug.Log("Placing Mine");
        Instantiate(minePrefab, mineSpawn);
    }
}
