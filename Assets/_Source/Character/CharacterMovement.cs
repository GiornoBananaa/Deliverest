using System;
using System.Collections;
using Core;
using UnityEngine;

namespace Character
{
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] private HandHook _leftHandHook;
        [SerializeField] private HandHook _rightHandHook;
        [SerializeField] private StaminaTimerView _staminaTimerView;
        [SerializeField] private JumpBarView _jumpBarView;
        [SerializeField] private RectTransform _staminaTimerRectTransform;
        [SerializeField] private Transform _leftStaminaTimerPoint;
        [SerializeField] private Transform _rightStaminaTimerPoint;
        [SerializeField] private GameObject _body;
        [SerializeField] private GameObject _restartButton;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private AudioSource _audioSource;
        
        private CharacterMovementData _movementData;
        private CharacterJump _characterJump;
        private SnowStormManager _stormManager;
        private UpdateTimer _oneHandTimer;
        private UpdateTimer _jumpTimer;
        private bool _isFallingForLoss;
        private bool _looseStamina = true;
        
        public Action<GameObject> OnHandHooked;
        
        public bool IsOnOneHand => _leftHandHook.IsHooked != _rightHandHook.IsHooked;
        public bool IsOnTwoHands => _leftHandHook.IsHooked && _rightHandHook.IsHooked;
        public bool IsOnNothing => !_leftHandHook.IsHooked && !_rightHandHook.IsHooked;
        
        public void Construct(SnowStormManager stormManager, CharacterMovementData movementData)
        {
            _movementData = movementData;
            _stormManager = stormManager;
            _oneHandTimer = new UpdateTimer(_movementData.MaxStaminaTime);
            _jumpTimer = new UpdateTimer(_movementData.JumpPreparationTime);
            _characterJump = new CharacterJump(_rigidbody, movementData, _jumpTimer);
            _jumpBarView.Construct(movementData, _characterJump);
            _leftHandHook.Construct(_movementData.HookRadius);
            _rightHandHook.Construct(_movementData.HookRadius);
            _isFallingForLoss = false;
            DisableStaminaTimer();
        }
        
        private void OnEnable()
        {
            _oneHandTimer.OnTimerEnd += Fall;
            _oneHandTimer.OnTimeChanged += ChangeStaminaView;
            _leftHandHook.OnObstacleHit += HitArmByObstacle;
            _rightHandHook.OnObstacleHit += HitArmByObstacle;
            GameManager.instance.OnSnowStormStart += SnowStormStart;
            GameManager.instance.OnSnowStormEnd += SnowStormEnd;
            _leftHandHook.OnHandHooked+=InvokeOnHandHook;
            _rightHandHook.OnHandHooked+=InvokeOnHandHook;
        }
        
        private void Update()
        {
            _jumpTimer.Update();
            _oneHandTimer.Update();
            SnowStormBehavior();
            CheckLossHeight();
        }
        
        private void OnDisable()
        {
            _oneHandTimer.OnTimerEnd -= Fall;
            _oneHandTimer.OnTimeChanged -= ChangeStaminaView;
            _leftHandHook.OnObstacleHit -= HitArmByObstacle;
            _rightHandHook.OnObstacleHit -= HitArmByObstacle;
            _leftHandHook.OnHandHooked-=InvokeOnHandHook;
            _rightHandHook.OnHandHooked-=InvokeOnHandHook;
            GameManager.instance.OnSnowStormStart -= SnowStormStart;
            GameManager.instance.OnSnowStormEnd -= SnowStormEnd;
        }
        
        public void StartLeftArmMove()
        {
            if (IsOnOneHand && _leftHandHook.IsHooked)
                return;
            if(IsOnTwoHands)
                EnableStaminaTimer(_rightStaminaTimerPoint);
            _leftHandHook.StartMove(!IsOnNothing);
        }
        
        public void StartRightArmMove()
        {
            if (IsOnOneHand && _rightHandHook.IsHooked) 
                return;
            if(IsOnTwoHands)
                EnableStaminaTimer(_leftStaminaTimerPoint);
            _rightHandHook.StartMove(!IsOnNothing);
        }
        
        public void EndLeftArmMove()
        {
            if (_leftHandHook.TryHook())
            {
                DisableStaminaTimer();
            }
        }
        
        public void EndRightArmMove()
        {
            if (_rightHandHook.TryHook())
            {
                DisableStaminaTimer();
            }
        }
        
        public void Jump()
        {
            if(!IsOnTwoHands)
                return;
            if(_characterJump.Jump())
                Fall();
        }
        
        public void SetJumpDirection(Vector3 mousePosition)
        {
            if(!IsOnTwoHands)
                return;

            Vector2 jumpDirection = mousePosition - _rigidbody.transform.position;
            
            _characterJump.SetJumpDirection(jumpDirection);
        }
        
        public void PrepareForJump()
        {
            if(!IsOnTwoHands)
                return;
            
            _characterJump.PrepareForJump();
        }

        public void EnableStamina(bool enable)
        {
            _looseStamina = enable;
        }
        
        private void Fall()
        {
            DisableStaminaTimer();
            _rightHandHook.FallJointsAnchor();
            _leftHandHook.FallJointsAnchor();
            _rightHandHook.Unhook(false);
            _leftHandHook.Unhook(false);
        }
        
        private void CheckLossHeight()
        {
            if ((_body.transform.position.y <= _movementData.LossHeight || _body.transform.localPosition.x <= -_movementData.LossY || _body.transform.localPosition.x >= _movementData.LossY) && !_isFallingForLoss)
                StartCoroutine(FallLoss());
        }
        
        private void EnableStaminaTimer(Transform parent)
        {
            if(!_looseStamina) return;
            var timerTransform = _staminaTimerRectTransform;
            timerTransform.SetParent(parent, false);
            timerTransform.localPosition = Vector3.zero;
            _staminaTimerView.gameObject.SetActive(true);
            _oneHandTimer.Restart();
        }
        
        private void DisableStaminaTimer()
        {
            _staminaTimerView.gameObject.SetActive(false);
            _oneHandTimer.Stop();
        }
        
        private void ChangeStaminaView(float elapsedTime)
        {
            _staminaTimerView.fillAmount = (_oneHandTimer.MaxTime-elapsedTime) / _oneHandTimer.MaxTime;
        }
        
        private void HitArmByObstacle(HandHook hand)
        {
            if (IsOnOneHand && hand.IsHooked)
            {
                Fall();
                return;
            }
            if(IsOnTwoHands)
            {
                hand.Unhook(IsOnTwoHands);
                EnableStaminaTimer(hand!=_leftHandHook?_leftStaminaTimerPoint:_rightStaminaTimerPoint);
            }
            if (IsOnNothing)
            {
                Fall();
            }
        }
            
        private void SnowStormStart()
        {
            _oneHandTimer.SetMaxTime(_movementData.MaxStaminaTimeWhileStorm);
        }
        
        private void SnowStormEnd()
        {
            _oneHandTimer.SetMaxTime(_movementData.MaxStaminaTime);
        }
        
        private void SnowStormBehavior()
        {
            if (_stormManager.IsStorm && (IsOnOneHand || IsOnNothing))
            {
                _rigidbody.AddForce(
                    new Vector2(_stormManager.Velocity.x > 0 ? 1 : -1, 0) * _movementData.StormForce,
                    ForceMode2D.Force);
            }
        }
        
        private IEnumerator FallLoss()
        {
            _restartButton.SetActive(true);
            _isFallingForLoss = true;
            _audioSource.Play();
            float fallingTime = 0;
            while (fallingTime< _movementData.FallingForLossTime)
            {
                fallingTime += Time.deltaTime;
                _audioSource.volume = Mathf.Lerp(_audioSource.volume, 0, 0.005f);
                yield return new WaitForEndOfFrame();
            }
            _restartButton.SetActive(false);
            GameManager.instance.LoseGame(false);
        }

        private void InvokeOnHandHook(GameObject grip)
        {
            OnHandHooked?.Invoke(grip);
        }
    }
}