using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class RoomPlayerSpawner : MonoBehaviour
{
    [SerializeField] private List<Transform> startingPositions;
    [SerializeField] private List<GameObject> playerPrefabs;

    private void Start()
    {
        switch (PhotonNetwork.CurrentRoom.PlayerCount)
        {
            case 1:
                var player1 = PhotonNetwork.Instantiate(playerPrefabs[0].name, startingPositions[0].position, Quaternion.identity);
                PlayerModel model = player1.GetComponent<PlayerModel>();
                model.StartingPosition = startingPositions[0].position;
                model.photonView.RPC("SetPlayerFaction", RpcTarget.AllBufferedViaServer, TeamFactionConsts.RED_TEAM);
                break;
            case 2:
                var player2 = PhotonNetwork.Instantiate(playerPrefabs[1].name, startingPositions[1].position, Quaternion.identity);
                PlayerModel model2 = player2.GetComponent<PlayerModel>();
                model2.StartingPosition = startingPositions[1].position;
                model2.photonView.RPC("SetPlayerFaction", RpcTarget.AllBufferedViaServer, TeamFactionConsts.BLUE_TEAM);
                break;
        }
    }
}
