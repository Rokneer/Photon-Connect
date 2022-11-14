using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class EditColor : MonoBehaviourPun
{
    private Color _playerColor;

    void Start()
    {
        photonView.RPC("ChangeColor", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void ChangeColor()
    {
        if (photonView.IsMine)
        {
            _playerColor = (string)PhotonNetwork.LocalPlayer.CustomProperties["color"] switch
            {
                "Green" => Color.green,
                "Blue" => Color.blue,
                "Red" => Color.red,
                "Yellow" => Color.yellow,
                _ => Color.green
            };
        }
        else
        {
            _playerColor = (string)PhotonNetwork.PlayerList[Equals(PhotonNetwork.PlayerList[1],
                    PhotonNetwork.LocalPlayer) ? 0 : 1].CustomProperties["color"] switch
                {
                    "Green" => Color.green,
                    "Blue" => Color.blue,
                    "Red" => Color.red,
                    "Yellow" => Color.yellow,
                    _ => Color.green
                };
        }

        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        foreach (var t in renderers)
        {
            t.material.color = _playerColor;
        }
    }
}
