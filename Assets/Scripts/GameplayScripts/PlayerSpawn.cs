using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] private List<Transform> startingPositions;
    [SerializeField] private List<GameObject> playerPrefabs;
    private GameObject player;
    private PhotonView pv;

    private static int _playersSpawned; //Esto aún no funciona
    
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        _playersSpawned++;
    }

    private void Start()
    {
        player = PhotonNetwork.Instantiate(playerPrefabs[_playersSpawned - 1].name, startingPositions[_playersSpawned-1].position, Quaternion.identity);

        //pv.RPC("SetColorToPlayers", RpcTarget.OthersBuffered);
    }

    [PunRPC]
    private void SetColorToPlayers()
    {
        if (pv.IsMine)
        {
            switch (PhotonNetwork.CurrentRoom.PlayerCount)
            {
                case 1:
                    player.GetComponent<SpriteRenderer>().color = Color.red;
                    break;
                case 2:
                    player.GetComponent<SpriteRenderer>().color = Color.blue;
                    break;
            }
        }
    }
}
