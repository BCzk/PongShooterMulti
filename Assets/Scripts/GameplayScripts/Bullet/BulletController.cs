using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviourPun
{
    private BulletModel _bulletModel;
    private bool bIsPendingKill = false;
    
    private void Awake()
    {
        _bulletModel = GetComponent<BulletModel>();
    }
    private void Start()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().ResetThings += DestroyBullet;
    }

    private void Update()
    {
        _bulletModel.Move();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (photonView.IsMine)
        {
            if (collision.gameObject.CompareTag("Shield") && !collision.gameObject.GetPhotonView().IsMine)
            {
                collision.gameObject.GetComponent<ShieldModel>().photonView.RPC("ShieldHit", RpcTarget.AllBufferedViaServer);
                DestroyBullet();

            }
            else if (collision.gameObject.CompareTag("Player") && !collision.gameObject.GetPhotonView().IsMine)
            {
                //TODO: kill player
                DestroyBullet();
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
