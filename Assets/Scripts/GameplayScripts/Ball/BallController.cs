using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviourPun
{
    private Rigidbody2D _rb;
    private BallModel _model;
    private Vector3 lastVelocity;

    private float increaseSpeedCounter = 0.0f;
    private const float TIME_TO_CALL_SPEED_INCREASE = 3.0f;
    private const float BALL_SPEED_INCREASE_AMOUNT = 25.0f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _model = GetComponent<BallModel>();
    }

    private void Start()
    {
        var rndDir = Random.insideUnitCircle.normalized;
        _rb.velocity = rndDir * _model.Speed * Time.fixedDeltaTime;
    }

    private void FixedUpdate()
    {
        lastVelocity = _rb.velocity;

        increaseSpeedCounter += Time.fixedDeltaTime;
        if (increaseSpeedCounter > TIME_TO_CALL_SPEED_INCREASE)
        {
            _model.photonView.RPC("IncreaseBallSpeed", RpcTarget.AllBufferedViaServer, BALL_SPEED_INCREASE_AMOUNT);
            increaseSpeedCounter = 0.0f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var aux = 0.2f;

        Vector2 newDirection = Vector2.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

        Vector2 randomFactor = Random.insideUnitCircle * aux;
        newDirection += randomFactor;
        newDirection.Normalize();

        _rb.velocity = newDirection * _model.Speed * Time.fixedDeltaTime;


        if (collision.gameObject.CompareTag("Shield") && collision.gameObject.GetPhotonView().IsMine)
        {
            _model.photonView.RPC("SetBallFactionOwner", RpcTarget.AllBufferedViaServer, 
                collision.gameObject.GetComponentInParent<PlayerModel>().PlayerTeamFaction);
        }

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(EventCodeConsts.ON_BALL_BOUNCE_EVENT, "BallBounceSFX", raiseEventOptions, SendOptions.SendUnreliable);
    }
}
