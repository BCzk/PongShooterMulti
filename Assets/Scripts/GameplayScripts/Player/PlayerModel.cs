using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    [SerializeField] private float speed;

    [SerializeField] private GameObject bullet;
    
    public void Move(float dirY)
    {
        float movement = dirY * speed * Time.deltaTime;
        transform.Translate(0, movement,0);
    }

    public void Shoot() //Seguramente para sincronizarlo haya que transformarlo a un PunRPC
    {
        Instantiate(bullet, transform.position, quaternion.identity);
    }
}
