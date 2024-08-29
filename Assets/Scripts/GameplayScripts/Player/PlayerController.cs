using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerModel _playerModel;
    private PhotonView _pv;
    
    private void Start()
    {
        _playerModel = GetComponent<PlayerModel>();
        _pv = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (_pv.IsMine)
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
