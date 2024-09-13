using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldModel : MonoBehaviourPun
{
    [Range(0, 1)] [SerializeField] private float shieldLostPerHit;
    private Vector3 _auxiliarScaleVector;

    [PunRPC]
    public void ShieldHit()
    {
        _auxiliarScaleVector = transform.localScale;
        _auxiliarScaleVector.y -= shieldLostPerHit;
        transform.localScale = _auxiliarScaleVector;

        /* TODO: Reset
        if (transform.localScale.y <= 0)
        {
            ResetShield();
        }
        */
    }

    [PunRPC]
    public void ResetShield()
    {
        _auxiliarScaleVector.y = 1;
        transform.localScale = _auxiliarScaleVector;
    }
}
