using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    private ShieldModel _shieldModel;

    private void Start()
    {
        _shieldModel = GetComponent<ShieldModel>();
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space)) //Placeholder para probar
        // {
        //     _shieldModel.ShieldHit();
        // }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("Colisione con la bala");
            _shieldModel.ShieldHit();
        }
        else
        {
            Debug.Log("No me sirve pero colisione");
        }
    }
}
