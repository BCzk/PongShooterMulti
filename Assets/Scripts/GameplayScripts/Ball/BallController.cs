using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private BallModel _model;
    private Vector2 _lastVelocity;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _model = GetComponent<BallModel>();
    }

    private void Start()
    {
        _rb.velocity = Random.insideUnitCircle.normalized * _model.Speed;
    }
    private void Update()
    {
        _lastVelocity = _rb.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var aux = 0.2f;

        Vector2 newDirection = Vector2.Reflect(_lastVelocity.normalized, collision.contacts[0].normal);

        Vector2 randomFactor = Random.insideUnitCircle * aux;
        newDirection += randomFactor;
        newDirection.Normalize();

        _rb.velocity = newDirection * _model.Speed;
    }
}
