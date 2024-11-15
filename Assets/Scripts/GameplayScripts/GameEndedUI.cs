using System;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;

public class GameEndedUI : MonoBehaviour
{
    [SerializeField] private GameObject gameEndedPanel;
    [SerializeField] private GameObject redWins;
    [SerializeField] private GameObject blueWins;
    [SerializeField] private TMPro.TextMeshProUGUI matchEndText;

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }
    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }
    private void Start()
    {
        gameEndedPanel.SetActive(false);
        blueWins.SetActive(false);
        redWins.SetActive(false);
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == EventCodeConsts.ON_MATCH_FINISHED_EVENT)
        {
            object[] eventData = (object[])photonEvent.CustomData;
            string winnerFaction = (string)eventData[0];
            bool bIsWinByAbandon = (bool)eventData[1];
            ShowWinScreen(winnerFaction, bIsWinByAbandon);
        }
    }

    public void LeaveAndGoBackToLobby()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LeaveRoom(false);
        SceneManager.LoadScene("MenuScene");
    }

    private void ShowWinScreen(string winner, bool bIsWInByAbandon)
    {
        gameEndedPanel.SetActive(true);

        if (winner == TeamFactionConsts.RED_TEAM)
        {
            redWins.SetActive(true);
            blueWins.SetActive(false);
        }
        if (winner == TeamFactionConsts.BLUE_TEAM)
        {
            blueWins.SetActive(true);
            redWins.SetActive(false);
        }

        if (bIsWInByAbandon)
        {
            matchEndText.text = "The other Player has left the Room. Waiting for you to leave.";
        }
        else
        {
            matchEndText.text = winner + " wins. The room will be automatically closed in 10 seconds.";
        }
    }
}
