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

        //pv.RPC("SetColor", RpcTarget.OthersBuffered);
    }

    /*[PunRPC]
    private void SetColorToPlayers()
    {
        foreach (KeyValuePair<int, Photon.Realtime.Player> playersData in PhotonNetwork.CurrentRoom.Players)
        {
            switch (playersData.Value.UserId)
            {

            }
        }
    }*/
}
