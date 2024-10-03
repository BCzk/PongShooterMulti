using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    public Vector3 StartingPosition
    {
        get => _startingPosition;
        set => _startingPosition = value;
    }

    [SerializeField] private float speed;

    [SerializeField] private GameObject bullet;

    [SerializeField] private Vector3 _startingPosition;
    
    public void Move(float dirY)
    {
        float movement = dirY * speed * Time.deltaTime;
        transform.Translate(0, movement,0);
    }

    public void Shoot()
    {
        PhotonNetwork.Instantiate(bullet.name, transform.position, Quaternion.identity);
    }

    public void Die()
    {
        //Lo que sea que hagamos
        Debug.Log(gameObject.name + ": Me morí");
    }
    
    public void ResetPosition()
    {
        transform.position = _startingPosition;
    }
}
