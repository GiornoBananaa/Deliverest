using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Character
{
    public class HandHook : MonoBehaviour
    {
        public bool IsMoving { get; private set; }
        public bool IsHooked { get; private set; }

        public Vector2 JointAnchor => _target.connectedAnchor;
    
        [SerializeField] private DistanceJoint2D _target;
        [SerializeField] private LayerMask _gripLayerMask;
        [SerializeField] private LayerMask _obstacleLayerMask;
        [SerializeField] private Transform _defaultPosition;
        [SerializeField] private GameObject _limbSolver;
        
        private float _hookRadius;

        public Action<GameObject> OnHandHooked;
        public Action<HandHook> OnObstacleHit;
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        void Awake()
        {
            IsHooked = true;
            IsMoving = false;
        }

        public void Construct(float hookRadius)
        {
            _hookRadius = hookRadius;
        }
        
        void Update()
        {
            if (GameManager.instance.isPaused)
                return;
            
            transform.position = _target.transform.position;
            
            if (IsMoving) 
                Move();
            
            //TODO: put obstacle check in another place
            CheckForObstacles();
        }
        
        private void Move()
        {
            _target.transform.position = (Vector2)_camera.ScreenToWorldPoint(Input.mousePosition);
        }
        
        public void Unhook(bool isArmMovesPlayer)
        {
            _target.enabled = false;
            _limbSolver.SetActive(false);
            IsMoving = false;
            IsHooked = false;
            if(!isArmMovesPlayer)
                _target.autoConfigureConnectedAnchor = false;
        }
        
        public bool TryHook()
        {
            if(IsHooked) return false;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(_defaultPosition.position, _hookRadius, _gripLayerMask);

            if (colliders.Length > 0)
            {
                _limbSolver.SetActive(true);
                DefaultJointsAnchor();
                _target.enabled = true;
                _target.autoConfigureConnectedAnchor = false;
                _target.transform.position = _defaultPosition.position;
                IsHooked = true;
                AudioSource audioSource = colliders[0].GetComponent<AudioSource>();
                if (audioSource.clip is not null) audioSource.Play();
                OnHandHooked?.Invoke(colliders[0].gameObject);
            }
            else
            {
                _target.enabled = false;
                _limbSolver.SetActive(false);
            }

            IsMoving = false;

            return colliders.Length > 0;
        }

        public void StartMove(bool affectBody)
        {
            IsHooked = false;
            IsMoving = true;
            if (affectBody) 
                _target.enabled = true;
            if (!_limbSolver.activeSelf) 
                _limbSolver.SetActive(true);
        }
        
        private void CheckForObstacles()
        {
            if (Physics2D.OverlapCircleAll(_defaultPosition.position, _hookRadius, _obstacleLayerMask).Length > 0)
            {
                OnObstacleHit?.Invoke(this);
            }
        }
        
        private void DefaultJointsAnchor()
        {
            _target.connectedAnchor = JointAnchor;
        }
    }
}
