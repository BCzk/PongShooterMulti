using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private BallModel _model;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _model = GetComponent<BallModel>();
    }

    private void Start()
    {
        int randomDir = Random.Range(0, 2);
        if (randomDir == 0)
        {
            AddForceToBall(new Vector2(0.2f, 1));
        }
        else
        {
            AddForceToBall(new Vector2(-0.2f, -1));
        }


        //WIP
        //_rb.velocity = Vector2.one * _model.Speed;   //_model.ChooseRandomDirection();
    }

    private void AddForceToBall(Vector2 dir)
    {
        _rb.AddForce(_model.Speed * Time.deltaTime * dir);
    }
}
