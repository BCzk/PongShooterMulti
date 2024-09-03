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

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Shield") || other.CompareTag("Player"))
        {
            Debug.Log("Colisione me destruyo");
            DestroyBullet();
        }
        else
        {
            Debug.Log("No me sirve pero colisione");
        }
    }

    private void OnBecameInvisible() // Por si el objeto se sale de la pantalla
    {
        DestroyBullet();
    }

    private void DestroyBullet()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (!pv.IsMine)
            {
                Debug.Log("Intentando transferir propiedad al Master Client para el objeto " + gameObject, gameObject);
                pv.TransferOwnership(PhotonNetwork.MasterClient);

                StartCoroutine(WaitUntilOwnershipAndDestroy());
            }
            else
            {
                Debug.Log("El objeto ya es del Master Client, destruyéndolo " + gameObject, gameObject);
                PhotonNetwork.Destroy(pv);
            }
        }
    }

    private IEnumerator WaitUntilOwnershipAndDestroy()
    {
        while (!pv.IsMine)
        {
            yield return null;
        }

        Debug.Log("Propiedad transferida exitosamente al Master Client. Destruyendo " + gameObject, gameObject);
        PhotonNetwork.Destroy(pv);
    }

}
