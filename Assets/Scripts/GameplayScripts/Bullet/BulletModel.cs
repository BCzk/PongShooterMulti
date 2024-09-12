using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletModel : MonoBehaviour
{
    [SerializeField] private float speed;
    
    public void Move()
    {
        transform.Translate(speed * Time.deltaTime,0,0);
    }
}
