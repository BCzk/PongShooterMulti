using System;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class GameEndedUI : MonoBehaviour
{
    [SerializeField] private GameObject gameEndedPanel;
    [SerializeField] private GameObject redWins;
    [SerializeField] private GameObject blueWins;

    private void Start()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().MatchFinished += UpdateUI;
        gameEndedPanel.SetActive(false);
        blueWins.SetActive(false);
        redWins.SetActive(false);
    }

    private void UpdateUI(string winner)
    {
        gameEndedPanel.SetActive(true);

        if (winner == "Red")
        {
            redWins.SetActive(true);
            blueWins.SetActive(false);
        }
        if (winner == "Blue")
        {
            blueWins.SetActive(true);
            redWins.SetActive(false);
        }
    }
}
