using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class BallModel : MonoBehaviour
{
    public float Speed => speed;
    
    //private Vector2 _auxDirectionVector;
    [SerializeField] private float speed;
    
    //public Vector2 ChooseRandomDirection()
    //{
    //    _auxDirectionVector.x = Random.Range(1, 360);
    //    _auxDirectionVector.y = Random.Range(1, 360);
    //    return _auxDirectionVector.normalized;
    //}

    //public void ChangeBallColor()
    //{
        
    //}
}
