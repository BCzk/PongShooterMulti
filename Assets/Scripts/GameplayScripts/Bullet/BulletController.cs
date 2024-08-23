using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private BulletModel _bulletModel;
    
    void Start()
    {
        _bulletModel = GetComponent<BulletModel>();
    }
    
    void Update()
    {
        _bulletModel.Move();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // if (other.gameObject.tag == "") //Tag del escudo
        // {
        //     Destroy(gameObject); //Cambiar por lógica de pool
        // }
    }

    private void OnBecameInvisible() //Por si se sale de la pantalla
    {
        Destroy(gameObject); //Cambiar por lógica de pool
    }
}
