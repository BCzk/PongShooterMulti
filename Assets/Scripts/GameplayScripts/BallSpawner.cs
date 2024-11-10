using ExitGames.Client.Photon;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private float timeToStartSpawning;
    [SerializeField] private float timeBetweenSpawn;

    private float timer = 0.0f;
    private bool readyToSpawn;
    private bool roundStarted;

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }
    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }
    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == EventCodeConsts.ON_ROUND_STARTED_EVENT)
        {
            timer = 0.0f;
            readyToSpawn = false;
            roundStarted = true;
        }
        else if (photonEvent.Code == EventCodeConsts.ON_ROUND_FINISHED_EVENT)
        {
            timer = 0.0f;
            roundStarted = false;
        }
    }


    private void Update()
    {
        if (roundStarted)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                timer += Time.deltaTime;
                if (!readyToSpawn && timer > timeToStartSpawning)
                {
                    readyToSpawn = true;
                    timer = 0.0f;
                }

                if (readyToSpawn && timer > timeBetweenSpawn)
                {
                    timer = 0.0f;
                    PhotonNetwork.Instantiate(ballPrefab.name, spawnPoint.position, Quaternion.identity);
                }
            }
        }
    }
}
