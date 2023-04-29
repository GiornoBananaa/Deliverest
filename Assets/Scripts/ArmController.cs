using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmController : MonoBehaviour
{
    [Range(0,1)][SerializeField] private int _button;
    [SerializeField] private GameObject _target;
    [SerializeField] private int _hitchLayer;
    [SerializeField] private Transform _deafultPosition;
    [SerializeField] private GameObject _limbSolver;

    private Vector2 _startPosition;
    private bool _isMoving;
    private DistanceJoint2D _joint;

    void Start()
    {
        
        _isMoving = false;
        _joint = _target.GetComponent<DistanceJoint2D>();
    }

    void Update()
    {
        transform.position = _target.transform.position;
        if (Input.GetMouseButtonDown(_button))
            StartMove();
        if (Input.GetMouseButtonUp(_button) && _isMoving)
            TryCling();

        if (_isMoving) Move();
    }

    private void Move()
    {
        _target.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void TryCling()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_deafultPosition.position, 1, _hitchLayer);
        if (colliders.Length > 0)
        {
            _target.transform.position = _deafultPosition.position;
        }
        else
        {
            _joint.enabled = false;
            _limbSolver.SetActive(false);
        }

        _isMoving = false;
    }

    private void StartMove()
    {
        Debug.Log("StartMove");
        _isMoving = true;
        if (_joint.enabled == false) _joint.enabled = true;
        if (!_limbSolver.activeSelf) _limbSolver.SetActive(true);
    }
}
