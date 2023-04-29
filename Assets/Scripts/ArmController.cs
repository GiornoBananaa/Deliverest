using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmController : MonoBehaviour
{
    [SerializeField] private Transform _deafultPosition;

    private bool CanCling;
    private bool _isMoving;
    private Rigidbody2D _rigidbody;
    private DistanceJoint2D _joint;

    void Start()
    {
        CanCling = false;
        _isMoving = false;
        _joint = GetComponent<DistanceJoint2D>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            StartMove();
        if (Input.GetMouseButtonUp(0))
            TryCling();

        if (_isMoving) Move();
    }

    private void Move()
    {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void TryCling()
    {
        // -
        if (true)
        {
            transform.position = _deafultPosition.position;
        }
        else
        {
            _joint.enabled = false;
        }

        _isMoving = false;
    }

    private void StartMove()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider is not null && hit.transform.gameObject.name == transform.gameObject.name)
        {
            Debug.Log("StartMove");
            _isMoving = true;
            if (_joint.enabled == false) _joint.enabled = true;
        }
    }
}
