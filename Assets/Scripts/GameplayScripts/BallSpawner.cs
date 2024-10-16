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

    private float timer;
    private bool readyToSpawn;
    private bool matchStarted;

    private void Start()
    {
        timer = 0;
    }

    private void Update()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            matchStarted = true;
        }
        else if (PhotonNetwork.CurrentRoom.PlayerCount < 2 && matchStarted)
        {
            matchStarted = false;
        }

        if (matchStarted)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                timer += Time.deltaTime;
                if (!readyToSpawn && timer > timeToStartSpawning)
                {
                    readyToSpawn = true;
                    timer = 0;
                }

                if (readyToSpawn && timer > timeBetweenSpawn)
                {
                    Debug.Log(timer);
                    timer = 0;
                    PhotonNetwork.Instantiate(ballPrefab.name, spawnPoint.position, Quaternion.identity);
                }
            }
        }
    }
}
