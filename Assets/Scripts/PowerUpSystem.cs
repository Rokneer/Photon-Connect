using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PowerUpSystem : MonoBehaviour, IOnEventCallback
{
    private const byte CureEventCode = 1;
    [SerializeField] private GameObject curePrefab;
    [SerializeField] private Transform[] cureSpawnList;
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }
    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    private void Start()
    {
        if(PhotonNetwork.IsMasterClient) StartCoroutine(CureRespawnTimer());
    }

    private void CreateCure()
    {
        RaiseEventOptions eventOptions = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All,
            CachingOption = EventCaching.AddToRoomCache
        };
        PhotonNetwork.RaiseEvent(CureEventCode, null, eventOptions, SendOptions.SendReliable);
    }

    IEnumerator CureRespawnTimer()
    {
        yield return new WaitForSeconds(5.0f);
        CreateCure();
    }
    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code != CureEventCode) return;
        Debug.Log("Create Cure");
        for (int i = 0; i < cureSpawnList.Length; i++)
        {
            Instantiate(curePrefab, cureSpawnList[i]);
        }
    }
}
