using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArmController : MonoBehaviour
{
    public bool IsMoving { get; private set; }
    public bool IsHooked { get; private set; }
    public Vector2 JointAcnhor { get; private set; }

    [Range(0, 1)] [SerializeField] private int _button;
    [SerializeField] private GameObject _target;
    [SerializeField] private LayerMask _hitchLayerMask;
    [SerializeField] private LayerMask _avalanceLayerMask;
    [SerializeField] private Transform _deafultPosition;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private GameObject _limbSolver;
    [SerializeField] private GameObject _otherLimbSolver;
    [SerializeField] private ArmController _otherArmController;
    [SerializeField] private DistanceJoint2D _otherJoint;
    [SerializeField] private SnowStormManager _stormManager;
    [SerializeField] private StaminaTimer _staminaTimer;
    [SerializeField] private float _stormForce;
    [SerializeField] private float _maxStaminaTime;
    [SerializeField] private float _maxStaminaTimeWhileStorm;
    [SerializeField] private float _grappingRadius;

    private static bool _isOnOneHand;
    private DistanceJoint2D _joint;
    private float _timeOnOneHand;
    private float _maxTime;

    void Start()
    {
        _maxTime = _maxStaminaTime;
        _timeOnOneHand = _maxTime;
        IsHooked = true;
        _isOnOneHand = false;
        IsMoving = false;
        _joint = _target.GetComponent<DistanceJoint2D>();
        JointAcnhor = _joint.connectedAnchor;
    }

    void Update()
    {
        if (GameManager.instance.isPaused)
            return;
        transform.position = _target.transform.position;

        Controls();

        if (IsMoving) Move();

        OneHandTiming();

        if (IsHooked && !_otherArmController.IsHooked) _isOnOneHand = true;
        if (IsHooked && _otherArmController.IsHooked) _isOnOneHand = false;

        if (Physics2D.OverlapCircleAll(_deafultPosition.position, _grappingRadius, _avalanceLayerMask).Length > 0)
            Unhook();

        SnowStormBehavior();

        _staminaTimer.fillAmount = _timeOnOneHand / _maxTime;
    }

    private void OneHandTiming()
    {
        if (_isOnOneHand && !IsHooked)
        {
            _timeOnOneHand -= Time.deltaTime;
            _staminaTimer.gameObject.SetActive(true);
            if (_timeOnOneHand < 0)
            {
                Fall(true);
            }
        }
        else
        {
            _timeOnOneHand = _maxTime;
            _staminaTimer.gameObject.SetActive(false);
        }
    }

    private void Controls()
    {
        if (Input.GetMouseButtonDown(_button))
            StartMove();
        if (Input.GetMouseButtonUp(_button) && IsMoving)
            TryCling();
    }

    private void SnowStormBehavior()
    {
        if (_stormManager.IsStorm)
        {
            _maxTime = _maxStaminaTimeWhileStorm;
            if (_button == 1 && (_isOnOneHand || (!IsHooked && !_otherArmController.IsHooked)))
            {
                _rigidbody.AddForce(new Vector2(_stormManager.Velocity.x > 0 ? 1 : -1,0) * _stormForce, ForceMode2D.Force);
            }
        }
        else
        {
            _maxTime = _maxStaminaTime;
        }
    }

    private void Move()
    {
        _target.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void Fall(bool isMovable)
    {
        _otherJoint.enabled = false;
        _otherLimbSolver.SetActive(false);
        _staminaTimer.gameObject.SetActive(false);
        _joint.enabled = false;
        _limbSolver.SetActive(false);
        _otherArmController.IsHooked = false;
        IsMoving = false;
        IsHooked = !isMovable;
        _isOnOneHand = !isMovable;
        _joint.autoConfigureConnectedAnchor = true;
        _otherJoint.autoConfigureConnectedAnchor = true;
    }

    private void DeafultJointsAnchor()
    {
        _joint.connectedAnchor = JointAcnhor;
        _otherJoint.connectedAnchor = _otherArmController.JointAcnhor;
    }

    private void Unhook()
    {
        _joint.enabled = false;
        _limbSolver.SetActive(false);
        _staminaTimer.gameObject.SetActive(false);
        IsMoving = false;
        IsHooked = false;
        if (_otherArmController.IsHooked == false)
        {
            _joint.autoConfigureConnectedAnchor = true;
            _otherJoint.autoConfigureConnectedAnchor = true;
            _isOnOneHand = false;
        }
        else _isOnOneHand = true;
    }

    private void TryCling()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_deafultPosition.position, _grappingRadius, _hitchLayerMask);

        if (colliders.Length > 0)
        {
            if (_joint.autoConfigureConnectedAnchor) 
            {
                _otherJoint.autoConfigureConnectedAnchor = false;
                _joint.autoConfigureConnectedAnchor = false;
                DeafultJointsAnchor();
            }
            _target.transform.position = _deafultPosition.position;
            IsHooked = true;
            AudioSource audioSource = colliders[0].GetComponent<AudioSource>();
            if (audioSource.clip is not null) audioSource.Play();
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
        if (_otherArmController.IsHooked || (!IsHooked && !_otherArmController.IsHooked))
        {
            IsHooked = false;
            IsMoving = true;
            if (_joint.enabled == false) _joint.enabled = true;
            if (!_limbSolver.activeSelf) _limbSolver.SetActive(true);
        }
    }
}
