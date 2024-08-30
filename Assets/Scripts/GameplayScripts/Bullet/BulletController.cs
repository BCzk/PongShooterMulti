using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private BulletModel _bulletModel;
    PhotonView pv;
    
    void Start()
    {
        pv = GetComponent<PhotonView>();
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
        if (!PhotonNetwork.IsMasterClient)
        {

            pv.RPC("RequestDestroy", RpcTarget.MasterClient, pv.ViewID);
        }
        
    }

    [PunRPC]
    private void RequestDestroy(int id)
    {
        PhotonNetwork.Destroy(PhotonView.Find(id).gameObject);
    }
}
