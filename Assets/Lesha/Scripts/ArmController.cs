using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArmController : MonoBehaviour
{
    public bool IsMoving { get; private set; }

    [Range(0,1)][SerializeField] private int _button;
    [SerializeField] private GameObject _target;
    [SerializeField] private LayerMask _hitchLayerMask;
    [SerializeField] private Transform _deafultPosition;
    [SerializeField] private GameObject _limbSolver;
    [SerializeField] private GameObject _otherLimbSolver;
    [SerializeField] private ArmController _otherArmController;
    [SerializeField] private TMP_Text _timer;
    [SerializeField] private float _grappingRadius;

    private DistanceJoint2D _joint;

    void Start()
    {
        IsMoving = false;
        _joint = _target.GetComponent<DistanceJoint2D>();
    }

    void Update()
    {
        transform.position = _target.transform.position;
        if (Input.GetMouseButtonDown(_button))
            StartMove();
        if (Input.GetMouseButtonUp(_button) && IsMoving)
            TryCling();

        if (IsMoving) Move();
    }

    private void Move()
    {
        _target.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void TryCling()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_deafultPosition.position, _grappingRadius, _hitchLayerMask);

        if (colliders.Length > 0)
        {
            _target.transform.position = _deafultPosition.position;
        }
        else
        {
            _joint.enabled = false;
            _limbSolver.SetActive(false);
        }

        IsMoving = false;
    }

    private void StartMove()
    {
        if (_otherLimbSolver.activeSelf && !_otherArmController.IsMoving)
        {
            IsMoving = true;
            if (_joint.enabled == false) _joint.enabled = true;
            if (!_limbSolver.activeSelf) _limbSolver.SetActive(true);
        }
    }
}
