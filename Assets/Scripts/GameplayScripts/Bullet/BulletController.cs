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

    private void OnBecameInvisible() // Por si el objeto se sale de la pantalla
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
