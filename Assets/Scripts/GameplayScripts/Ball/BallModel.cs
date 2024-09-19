using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class BallModel : MonoBehaviourPun
{

    public int GetBallOwnerID => ballPlayerOwnerId;
    public float Speed => speed;
    
    [SerializeField] private float speed;
    private int ballPlayerOwnerId = 0;

    [PunRPC]
    public void SetBallOwner(int PlayerID)
    {
        if (PlayerID == 1)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
        }

        ballPlayerOwnerId = PlayerID;
    }
}
