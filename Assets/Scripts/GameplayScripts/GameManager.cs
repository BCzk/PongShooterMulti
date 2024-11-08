using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class GameManager : MonoBehaviourPunCallbacks
{
    private GameObject[] roomPlayers;

    private bool _matchStarted;

    private const int MAX_ROOM_PLAYERS = 2;

    private int _redPoints;
    private int _bluePoints;

    //Actualizaciones de UI
    [SerializeField] private Animator timerAnimator;
    [SerializeField] private Animator[] pointAnimators;

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == MAX_ROOM_PLAYERS)
        {
            StartCoroutine(StartMatch());
        }
    }
    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == EventCodeConsts.ON_PLAYER_LOSE_ROUND_EVENT)
        {
            object[] eventData = (object[])photonEvent.CustomData;
            string factionLoser = (string)eventData[0];
            HandleRoundFinished(factionLoser);
        }
    }

    private IEnumerator StartMatch()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        while (players.Length < MAX_ROOM_PLAYERS)
        {
            players = GameObject.FindGameObjectsWithTag("Player");
            roomPlayers = players;
            yield return new WaitForSeconds(0.1f);
        }

        if (!_matchStarted)
        {

            PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
            _matchStarted = true;

            StartCoroutine(StartRound());
        }
    }

    private IEnumerator StartRound()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GlobalRoomReset();
            // TODO: Sync bien en startmatch
            timerAnimator.SetTrigger("StartTimer");

            ChangeGlobalRoomInputEnabledStatus(false);
            yield return new WaitForSeconds(3);
            ChangeGlobalRoomInputEnabledStatus(true);
        }
    }

    private void HandleRoundFinished(string factionLoser)
    { 
        if (_matchStarted)
        {
            if (factionLoser == "Red")
            {
                _bluePoints++;

                if (_bluePoints >= 3)
                {
                    EndMatch("Blue");
                }
                else
                {
                    StartCoroutine(StartRound());
                }
            }
            else if (factionLoser == "Blue")
            {
                _redPoints++;

                if (_redPoints >= 3)
                {
                    EndMatch("Red");
                }
                else
                {
                    StartCoroutine(StartRound());
                }
            }

            UpdatePointsUI();
        }
    }

    private void EndMatch(string winnerFaction)
    {
        if (PhotonNetwork.IsMasterClient && _matchStarted)
        {
            PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
            _matchStarted = false;
        }
    }


    private void UpdatePointsUI()
    {
        for (int i = 0; i < _redPoints; i++)
        {
            pointAnimators[i].SetBool("Red", true);
        }

        for (int i = pointAnimators.Length - 1; i > (pointAnimators.Length - 1) - _bluePoints; i--)
        {
            pointAnimators[i].SetBool("Blue", true);
        }
    }

    private void ChangeGlobalRoomInputEnabledStatus(bool bIsEnabled)
    {
        foreach (GameObject player in roomPlayers)
        {
            PlayerController controller = player.GetComponent<PlayerController>();
            controller.photonView.RPC("SetInputEnabledStatus", RpcTarget.All, bIsEnabled);
        }
    }

    private void GlobalRoomReset()
    {
        foreach (GameObject player in roomPlayers)
        {
            PlayerModel model = player.GetComponent<PlayerModel>();
            model.photonView.RPC("ResetPosition", RpcTarget.All);
            ShieldModel shieldModel = player.GetComponentInChildren<ShieldModel>();
            shieldModel.photonView.RPC("ResetShield", RpcTarget.All);
        }

        if (PhotonNetwork.IsMasterClient)
        {
            KillPreviousRoundObjects("Ball");
            //KillPreviousRoundObjects("Bullet"); // falta resolver ownership 
        }
    }

    private void KillPreviousRoundObjects(string objectTag)
    {
        GameObject[] foundObjects = GameObject.FindGameObjectsWithTag(objectTag);
        List<PhotonView> objectPvs = new List<PhotonView>();

        foreach (GameObject obj in foundObjects)
        {
            objectPvs.Add(obj.GetComponent<PhotonView>());
        }

        for (int i = objectPvs.Count - 1; i >= 0; i--)
        {
            PhotonView pv = objectPvs[i];
            PhotonNetwork.Destroy(pv);
        }
    }
}
