using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowStorm : MonoBehaviour
{
    private Rigidbody2D _rigidBody;

    public Vector2 Velocity;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _rigidBody.velocity = Velocity;
    }
}
