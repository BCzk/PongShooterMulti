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
    private const float BALL_MAX_SPEED = 850.0f;

    [PunRPC]
    public void SetBallOwner(int playerID)
    {
        if (playerID == 1)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
        }

        ballPlayerOwnerId = playerID;
    }

    [PunRPC]
    public void IncreaseBallSpeed(float addSpeed)
    {
        speed = Mathf.Clamp(speed + addSpeed, 400.0f, BALL_MAX_SPEED);
    }
}
