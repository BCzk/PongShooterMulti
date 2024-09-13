using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviourPun
{
    private PlayerModel _playerModel;
    
    private void Awake()
    {
        _playerModel = GetComponent<PlayerModel>();
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetAxis("Vertical") != 0)
            {
                _playerModel.Move(Input.GetAxis("Vertical"));
                ClampPlayerMoveToScreen();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _playerModel.Shoot();
            }
        }
    }
    
    private void ClampPlayerMoveToScreen()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.y = Mathf.Clamp(pos.y, 0.1f, 0.9f); //Valores arbitrarios
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }
}
