using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class BallModel : MonoBehaviourPun
{
    public string GetBallOwnerFaction => ballOwnerFaction;
    public float Speed => speed;
    
    [SerializeField] private float speed;
    private string ballOwnerFaction = "";
    private const float BALL_MAX_SPEED = 850.0f;
    private bool bIsPendingKill = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerModel playerModel = collision.gameObject.GetComponent<PlayerModel>();

            if (playerModel.PlayerTeamFaction != ballOwnerFaction)
            {
                DestroyBall();
                playerModel.LoseRound();
            }
        }
    }


    [PunRPC]
    public void SetBallFactionOwner(string inFaction)
    {
        ballOwnerFaction = inFaction;

        if (ballOwnerFaction == TeamFactionConsts.RED_TEAM)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
        }
    }

    [PunRPC]
    public void IncreaseBallSpeed(float addSpeed)
    {
        speed = Mathf.Clamp(speed + addSpeed, 400.0f, BALL_MAX_SPEED);
    }

    private void DestroyBall()
    {
        if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient && !bIsPendingKill)
        {
            bIsPendingKill = true;
            PhotonNetwork.Destroy(photonView);
        }
    }
}
