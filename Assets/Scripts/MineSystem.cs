using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class MineSystem : MonoBehaviourPun
{
    [SerializeField] private GameObject minePrefab;
    [SerializeField] private Transform mineSpawn;
    [SerializeField] private float cooldown = 20;
    private float _currentCooldown;

    private void Start()
    {
        _currentCooldown = cooldown;
    }

    void Update()
    {
        if (photonView.IsMine && Input.GetKeyUp(KeyCode.Q) && _currentCooldown > cooldown)
        {
            photonView.RPC("SetMine", RpcTarget.AllBuffered);
            SetMine();
            _currentCooldown = 0;
        }

        _currentCooldown += Time.deltaTime;
    }
    [PunRPC]
    void SetMine()
    {
        Debug.Log("Placing Mine");
        Instantiate(minePrefab, mineSpawn);
    }
}
