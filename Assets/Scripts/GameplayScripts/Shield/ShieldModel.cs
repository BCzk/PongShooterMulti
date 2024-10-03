using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldModel : MonoBehaviourPun
{
    [Range(0, 1)] [SerializeField] private float shieldLostPerHit;
    private Vector3 _auxiliarScaleVector;
    private bool bIsShieldDestroyed = false;
    private const float SHIELD_RESET_TIME = 5.0f;
    private float shieldResetCounter;

    private void Start()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().ResetThings += ResetShield;
    }

    private void FixedUpdate()
    {
        if (bIsShieldDestroyed)
        {
            shieldResetCounter += Time.fixedDeltaTime;

            if (shieldResetCounter > SHIELD_RESET_TIME)
            {
                photonView.RPC("ResetShield", RpcTarget.AllBufferedViaServer);
            }
        }
    }

    [PunRPC]
    public void ShieldHit()
    {
        _auxiliarScaleVector = transform.localScale;
        _auxiliarScaleVector.y -= shieldLostPerHit;
        transform.localScale = _auxiliarScaleVector;

        if (_auxiliarScaleVector.y <= 0.2f)
        {
            bIsShieldDestroyed = true;
        }
    }

    [PunRPC]
    public void ResetShield()
    {
        _auxiliarScaleVector.y = 1;
        transform.localScale = _auxiliarScaleVector;
        bIsShieldDestroyed = false;
        shieldResetCounter = 0.0f;
    }
}
