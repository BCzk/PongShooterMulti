using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviourPun
{
    public Vector3 StartingPosition
    {
        get => _startingPosition;
        set => _startingPosition = value;
    }

    [SerializeField] private float speed;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Vector3 _startingPosition;

    private string playerFaction;


    public void Move(float dirY)
    {
        float movement = dirY * speed * Time.deltaTime;
        transform.Translate(0, movement,0);
    }

    public void Shoot()
    {
        PhotonNetwork.Instantiate(bullet.name, transform.position, Quaternion.identity);
    }

    public void LoseRound()
    {
        object[] eventData = new object[] { playerFaction };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(EventCodeConsts.ON_PLAYER_LOSE_ROUND_EVENT, eventData, raiseEventOptions, SendOptions.SendReliable);
    }

    [PunRPC]
    public void ResetPosition()
    {
        if (photonView.IsMine)
        {
            transform.position = _startingPosition;
        }
    }

    [PunRPC]
    public void SetPlayerFaction(string playerFaction)
    {
        this.playerFaction = playerFaction;
    }
}
