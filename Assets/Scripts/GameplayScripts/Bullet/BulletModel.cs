using ExitGames.Client.Photon;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletModel : MonoBehaviourPun
{
    [SerializeField] private float speed;
    private bool bIsPendingKill = false;

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }
    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == EventCodeConsts.ON_ROUND_FINISHED_EVENT)
        {
            DestroyBullet();
        }
    }

    public void Move()
    {
        transform.Translate(speed * Time.deltaTime,0,0);
    }


    private void Update()
    {
        Move();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (photonView.IsMine)
        {
            if (collision.gameObject.CompareTag("Shield") && !collision.gameObject.GetPhotonView().IsMine)
            {
                collision.gameObject.GetComponent<ShieldModel>().photonView.RPC("OnShieldHit", RpcTarget.All);
                DestroyBullet();

            }
            else if (collision.gameObject.CompareTag("Player") && !collision.gameObject.GetPhotonView().IsMine)
            {
                DestroyBullet();
                collision.gameObject.GetComponent<PlayerModel>().LoseRound();
            }
        }
    }

    private void OnBecameInvisible()
    {
        DestroyBullet();
    }

    private void DestroyBullet()
    {
        if (PhotonNetwork.IsConnected && photonView.IsMine && !bIsPendingKill)
        {
            bIsPendingKill = true;
            PhotonNetwork.Destroy(photonView);
        }
    }
}
