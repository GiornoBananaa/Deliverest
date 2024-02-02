using System;
using UnityEngine;

namespace Character
{
    public class HandHook : MonoBehaviour
    {
        public bool IsMoving { get; private set; }
        public bool IsHooked { get; private set; }

        public Vector2 JointAnchor => _target.connectedAnchor;
    
        [SerializeField] private DistanceJoint2D _target;
        [SerializeField] private LayerMask _hitchLayerMask;
        [SerializeField] private LayerMask _obstacleLayerMask;
        [SerializeField] private Transform _defaultPosition;
        [SerializeField] private GameObject _limbSolver;
        
        private float _hookRadius;

        public Action<GameObject> OnHandHooked;
        public Action<HandHook> OnObstacleHit;

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
            _target.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        
        public void Unhook(bool isArmMovable)
        {
            _target.enabled = false;
            _limbSolver.SetActive(false);
            IsMoving = false;
            IsHooked = false;
            if(!isArmMovable)
                _target.autoConfigureConnectedAnchor = true;
        }
        
        public bool TryHook()
        {
            if(IsHooked) return false;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(_defaultPosition.position, _hookRadius, _hitchLayerMask);

            if (colliders.Length > 0)
            {
                _target.autoConfigureConnectedAnchor = false;
                DefaultJointsAnchor();
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
        
        public void FallJointsAnchor()
        {
            _target.autoConfigureConnectedAnchor = false;
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
