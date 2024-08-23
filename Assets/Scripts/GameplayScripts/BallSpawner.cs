using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] private float timeToStartSpawning;
    [SerializeField] private float timeBetweenSpawn;

    private float timer;
    private bool readyToSpawn;
    private bool matchStarted;
    private GameObject ballPrefab;

    private void Start()
    {
        timer = 0;
    }

    private void Update()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            matchStarted = true;
            if (PhotonNetwork.IsMasterClient)
            {
                timer += Time.deltaTime;
                if (!readyToSpawn && timeToStartSpawning < timer)
                {
                    readyToSpawn = true;
                    timer = 0;
                }

                if (readyToSpawn && timer > timeBetweenSpawn)
                {
                    timer = 0;
                    PhotonNetwork.Instantiate(ballPrefab.name, new Vector2(Random.Range(-4, 4), Random.Range(-4, 4)), Quaternion.identity);
                }
            }
        }
        else if (PhotonNetwork.CurrentRoom.PlayerCount < 2 && matchStarted)
        {
        
        }
    }
}
