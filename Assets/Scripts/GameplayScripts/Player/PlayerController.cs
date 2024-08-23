using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerModel _playerModel;

    private void Start()
    {
        _playerModel = GetComponent<PlayerModel>();
    }

    private void Update()
    {
        if (Input.GetAxis("Vertical") != 0)
        {
            _playerModel.Move(Input.GetAxis("Vertical"));
            ClampPlayerMoveToScreen();
        }
    }
    
    private void ClampPlayerMoveToScreen()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.y = Mathf.Clamp(pos.y, 0.2f, 0.8f); //Valores arbitrarios
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }
}
