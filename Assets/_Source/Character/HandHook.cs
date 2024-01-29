using UnityEngine;
using UnityEngine.Serialization;

namespace Character
{
    public class HandHook : MonoBehaviour
    {
        public bool IsMoving { get; private set; }
        public bool IsHooked { get; private set; }
        public Vector2 JointAcnhor { get; private set; }

    
        [SerializeField] private GameObject _target;
        [SerializeField] private LayerMask _hitchLayerMask;
        [FormerlySerializedAs("_avalanceLayerMask")] [SerializeField] private LayerMask _obstacleLayerMask;
        [SerializeField] private Transform _deafultPosition;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private GameObject _limbSolver;
        [SerializeField] private GameObject _otherLimbSolver;
        [FormerlySerializedAs("_otherArmController")] [SerializeField] private HandHook _otherHandHook;
        [SerializeField] private DistanceJoint2D _otherJoint;
        [SerializeField] private SnowStormManager _stormManager;
        [SerializeField] private StaminaTimer _staminaTimer;
        [SerializeField] private float _stormForce;
        [SerializeField] private float _maxStaminaTime;
        [SerializeField] private float _maxStaminaTimeWhileStorm;
        [SerializeField] private float _grappingRadius;

        public static bool _isOnOneHand;
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

            if (IsMoving) Move();

            OneHandTiming();
            
            if (IsHooked && !_otherHandHook.IsHooked) _isOnOneHand = true;
            if (IsHooked && _otherHandHook.IsHooked) _isOnOneHand = false;

            if (Physics2D.OverlapCircleAll(_deafultPosition.position, _grappingRadius, _obstacleLayerMask).Length > 0)
                Unhook();

            SnowStormBehavior();
        }
        
        //TODO: Move to new timer scipt
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
            
            _staminaTimer.fillAmount = _timeOnOneHand / _maxTime;
        }
        
        //TODO: Move to CharacterMovement
        private void SnowStormBehavior()
        {
            if (_stormManager.IsStorm)
            {
                _maxTime = _maxStaminaTimeWhileStorm;
                if (_isOnOneHand || (!IsHooked && !_otherHandHook.IsHooked))
                {
                    _rigidbody.AddForce(new Vector2(_stormManager.Velocity.x > 0 ? 1 : -1,0) * _stormForce/2, ForceMode2D.Force);
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
        
        //TODO: Move to characterMovement
        public void Fall(bool isMovable)
        {
            _otherJoint.enabled = false;
            _otherLimbSolver.SetActive(false);
            _staminaTimer.gameObject.SetActive(false);
            _joint.enabled = false;
            _limbSolver.SetActive(false);
            _otherHandHook.IsHooked = false;
            IsMoving = false;
            IsHooked = !isMovable;
            _isOnOneHand = !isMovable;
            _joint.autoConfigureConnectedAnchor = true;
            _otherJoint.autoConfigureConnectedAnchor = true;
        }

        private void DefaultJointsAnchor()
        {
            _joint.connectedAnchor = JointAcnhor;
            _otherJoint.connectedAnchor = _otherHandHook.JointAcnhor;
        }

        private void Unhook()
        {
            _joint.enabled = false;
            _limbSolver.SetActive(false);
            _staminaTimer.gameObject.SetActive(false);
            IsMoving = false;
            IsHooked = false;
            if (_otherHandHook.IsHooked == false)
            {
                _joint.autoConfigureConnectedAnchor = true;
                _otherJoint.autoConfigureConnectedAnchor = true;
                _isOnOneHand = false;
            }
            else _isOnOneHand = true;
        }

        public void TryCling()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(_deafultPosition.position, _grappingRadius, _hitchLayerMask);

            if (colliders.Length > 0)
            {
                if (_joint.autoConfigureConnectedAnchor) 
                {
                    _otherJoint.autoConfigureConnectedAnchor = false;
                    _joint.autoConfigureConnectedAnchor = false;
                    DefaultJointsAnchor();
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

        public void StartMove()
        {
            if (_otherHandHook.IsHooked || (!IsHooked && !_otherHandHook.IsHooked))
            {
                IsHooked = false;
                IsMoving = true;
                if (_joint.enabled == false) _joint.enabled = true;
                if (!_limbSolver.activeSelf) _limbSolver.SetActive(true);
            }
        }
    }
}
