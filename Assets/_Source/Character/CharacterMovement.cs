using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Character
{
    public class CharacterMovement : MonoBehaviour
    {
        //TODO: put character movement info in scriptable object
        [SerializeField] private float _lossHeight;
        [SerializeField] private float _lossY;
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _jumpReloadTime;
        [SerializeField] private float _fallingForLossTime;
        [FormerlySerializedAs("_leftArmController")] [SerializeField] private HandHook _leftHandHook;
        [FormerlySerializedAs("_rightArmController")] [SerializeField] private HandHook _rightHandHook;
        [SerializeField] private GameObject _body;
        [SerializeField] private GameObject _restartButton;
        [SerializeField] private HandHook _leftHand;
        [SerializeField] private HandHook _rightHand;
        
        private Rigidbody2D _rigidbody2D;
        private AudioSource _audioSource;
        private bool _isFallingForLoss;
        private float _timeForNextJump;

        public float NormalizedJumpReloadTime { get => _timeForNextJump / _jumpReloadTime; }
        public bool IsOnOneHand { get => _leftHandHook.IsHooked != _rightHandHook.IsHooked; }
        public bool IsOnTwoHands { get => _leftHandHook.IsHooked && _rightHandHook.IsHooked; }
        
        void Start()
        {
            _timeForNextJump = _jumpReloadTime;
            _isFallingForLoss = false;
            _rigidbody2D = _body.GetComponent<Rigidbody2D>();
            _audioSource = GetComponent<AudioSource>();
        }
        
        
        //TODO: Put timer in new script
        void Update()
        {
            _timeForNextJump += Time.deltaTime;

            if ((_body.transform.position.y <= _lossHeight || _body.transform.localPosition.x <= -_lossY || _body.transform.localPosition.x >= _lossY) && !_isFallingForLoss)
                StartCoroutine(FallLoss());
        }

        public void StartLeftArmMove()
        {
            if (!(IsOnOneHand && _leftHand.IsHooked)) 
                _leftHand.StartMove();
        }
        
        public void StartRightArmMove()
        {
            if (!(IsOnOneHand && _rightHand.IsHooked))
                _rightHand.StartMove();
        }
        
        public void EndLeftArmMove()
        {
            _leftHand.TryHook();
        }
        
        public void EndRightArmMove()
        {
            _rightHand.TryHook();
        }
        
        public void Jump()
        {
            if(!_leftHandHook.IsHooked || !_rightHandHook.IsHooked || _timeForNextJump < _jumpReloadTime)
                return;

            Fall();
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePosition - (Vector2)_body.transform.position).normalized;

            _rigidbody2D.AddForce(direction * _jumpForce, ForceMode2D.Impulse);

            _timeForNextJump = 0;
        }

        public void Fall()
        {
            _rightHand.Unhook();
            _leftHand.Unhook();
        }
        
        private IEnumerator FallLoss()
        {
            _restartButton.SetActive(true);
            _isFallingForLoss = true;
            _audioSource.Play();
            float fallingTime = 0;
            while (fallingTime< _fallingForLossTime)
            {
                fallingTime += Time.deltaTime;
                _audioSource.volume = Mathf.Lerp(_audioSource.volume, 0, 0.005f);
                yield return new WaitForEndOfFrame();
            }
            _restartButton.SetActive(false);
            GameManager.instance.LoseGame(false);
        }
    }
}
