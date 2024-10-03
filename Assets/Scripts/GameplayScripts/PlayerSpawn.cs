using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] private List<Transform> startingPositions;
    [SerializeField] private List<GameObject> playerPrefabs;
    
    private void Start()
    {
        switch (PhotonNetwork.CurrentRoom.PlayerCount)
        {
            case 1:
                var player1 = PhotonNetwork.Instantiate(playerPrefabs[0].name, startingPositions[0].position, Quaternion.identity);
                player1.GetComponent<PlayerModel>().StartingPosition = startingPositions[0].position;
                break;
            case 2:
                var player2 = PhotonNetwork.Instantiate(playerPrefabs[1].name, startingPositions[1].position, Quaternion.identity);
                player2.GetComponent<PlayerModel>().StartingPosition = startingPositions[1].position;
                break;
        }
    }
}
