using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    private GameObject player;
    private PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        player = PhotonNetwork.Instantiate(playerPrefab.name, new Vector2(Random.Range(-4, 4), Random.Range(-4, 4)), Quaternion.identity);

        pv.RPC("SetColor", RpcTarget.OthersBuffered);
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
