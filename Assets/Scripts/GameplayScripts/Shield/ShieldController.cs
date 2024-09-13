using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviourPun
{
    /*
    private ShieldModel _shieldModel;

    private void Awake()
    {
        _shieldModel = GetComponent<ShieldModel>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (photonView.IsMine)
        {
            if (other.gameObject.CompareTag("Bullet") && !other.gameObject.GetPhotonView().IsMine)
            {
                Debug.Log("Colisione con la bala");
                
                _shieldModel.ShieldHit();
            }
        }
    }
    */
}
