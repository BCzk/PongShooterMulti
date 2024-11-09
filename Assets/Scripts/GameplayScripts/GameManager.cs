using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;

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

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        CloseRoom(true);
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
        if (!_matchStarted)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            while (players.Length < MAX_ROOM_PLAYERS)
            {
                players = GameObject.FindGameObjectsWithTag("Player");
                roomPlayers = players;
                yield return new WaitForSeconds(0.1f);
            }

            photonView.RPC("ReplicateMatchActiveStatus", RpcTarget.All, true);
            PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
            StartCoroutine(StartRound());
        }
    }

    [PunRPC]
    private void ReplicateMatchActiveStatus(bool bIsActive)
    {
        _matchStarted = bIsActive;
    }

    private IEnumerator StartRound()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            timerAnimator.SetBool("StartTimer", true);

            GlobalRoomReset();

            ChangeGlobalRoomInputEnabledStatus(false);
            yield return new WaitForSeconds(3);
            ChangeGlobalRoomInputEnabledStatus(true);

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(EventCodeConsts.ON_ROUND_STARTED_EVENT, null, raiseEventOptions, SendOptions.SendReliable);

            timerAnimator.SetBool("StartTimer", false);
        }
    }

    private void HandleRoundFinished(string factionLoser)
    { 
        if (_matchStarted)
        {
            if (factionLoser == TeamFactionConsts.RED_TEAM)
            {
                _bluePoints++;
                UpdatePointsUI();

                if (_bluePoints >= 3)
                {
                    EndMatch(false, TeamFactionConsts.BLUE_TEAM);
                }
                else
                {
                    StartCoroutine(StartRound());
                }
            }
            else if (factionLoser == TeamFactionConsts.BLUE_TEAM)
            {
                _redPoints++;
                UpdatePointsUI();

                if (_redPoints >= 3)
                {
                    EndMatch(false, TeamFactionConsts.RED_TEAM);
                }
                else
                {
                    StartCoroutine(StartRound());
                }
            }

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(EventCodeConsts.ON_ROUND_FINISHED_EVENT, null, raiseEventOptions, SendOptions.SendReliable);
        }
    }

    private void EndMatch(bool bIsWinByAbandon, string winnerFaction)
    {
        if (PhotonNetwork.IsMasterClient && _matchStarted)
        {
            GlobalRoomReset();
            ChangeGlobalRoomInputEnabledStatus(false);

            PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
            photonView.RPC("ReplicateMatchActiveStatus", RpcTarget.All, false);
            _redPoints = 0;
            _bluePoints = 0;

            object[] eventData = new object[] { winnerFaction };
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(EventCodeConsts.ON_MATCH_FINISHED_EVENT, eventData, raiseEventOptions, SendOptions.SendReliable);

            if (!bIsWinByAbandon)
            {
                StartCoroutine(HandlePostEndMatch());
            }
        }
    }
    private IEnumerator HandlePostEndMatch()
    {
        timerAnimator.SetBool("StartTimer", true);
        yield return new WaitForSeconds(3.0f);
        timerAnimator.SetBool("StartTimer", false);
        CloseRoom(false);
    }

    private void CloseRoom(bool bClosedByAbandon)
    {
        if (bClosedByAbandon)
        {
            EndMatch(bClosedByAbandon, GetRemainingPlayerTeamFaction());
        }
        else
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(EventCodeConsts.ON_MATCH_FINISHED_TIMEOUT_KICK_EVENT, null, raiseEventOptions, SendOptions.SendReliable);
        }

        PhotonNetwork.CurrentRoom.IsOpen = false;
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
        if (roomPlayers != null)
        {
            foreach (GameObject player in roomPlayers)
            {
                PlayerController controller = player.GetComponent<PlayerController>();
                controller.photonView.RPC("SetInputEnabledStatus", RpcTarget.All, bIsEnabled);
            }
        }
    }

    private void GlobalRoomReset()
    {
        if (roomPlayers != null)
        {
            foreach (GameObject player in roomPlayers)
            {
                PlayerModel model = player.GetComponent<PlayerModel>();
                model.photonView.RPC("ResetPosition", RpcTarget.All);
                ShieldModel shieldModel = player.GetComponentInChildren<ShieldModel>();
                shieldModel.photonView.RPC("ResetShield", RpcTarget.All);
            }
        }

        if (PhotonNetwork.IsMasterClient)
        {
            KillPreviousRoundObjects("Ball");
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

    private string GetRemainingPlayerTeamFaction()
    {
        GameObject remainingPlayer = GameObject.FindGameObjectWithTag("Player");
        return remainingPlayer.GetComponent<PlayerModel>().PlayerTeamFaction;
    }
}
