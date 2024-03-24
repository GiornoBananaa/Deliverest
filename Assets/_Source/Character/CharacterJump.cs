using System;
using Core;
using UnityEngine;

namespace Character
{
    public class CharacterJump
    {
        public Vector2 JumpDirection { get; private set; }
        public float JumpForce { get; private set; }
        private Rigidbody2D _rigidbody;
        private CharacterMovementData _movementData;
        private UpdateTimer _jumpTimer;
        private bool _isPreparation;
        
        public Action<Vector2> OnJumpDirectionChange;
        public Action<float> OnJumpForceChange;
        public Action OnJumpPreparationStart;
        public Action OnJump;
        
        public CharacterJump(Rigidbody2D rigidbody, CharacterMovementData movementData,UpdateTimer jumpTimer)
        {
            _rigidbody = rigidbody;
            _movementData = movementData;
            _jumpTimer = jumpTimer;
            _jumpTimer.SetMaxTime(_movementData.JumpPreparationTime);
            _jumpTimer.Stop();
        }
        
        public bool Jump()
        {
            if(!_isPreparation) return false;
            _rigidbody.AddForce(JumpDirection * Mathf.Lerp(_movementData.MinimumJumpForce, 
                _movementData.MaximumJumpForce, 
                _jumpTimer.ElapsedTime/_jumpTimer.MaxTime), ForceMode2D.Impulse);
            _isPreparation = false;
            EndPreparation();
            OnJump?.Invoke();
            return true;
        }
        
        public void SetJumpDirection(Vector2 direction)
        {
            if(!_isPreparation ) return;
            JumpDirection = direction.normalized;
            OnJumpDirectionChange?.Invoke(JumpDirection);
        }
        
        public void PrepareForJump()
        {
            _jumpTimer.SetMaxTime(_movementData.JumpPreparationTime);
            _jumpTimer.Restart();
            _jumpTimer.OnTimeChanged+=ChangeJumpForce;
            _jumpTimer.OnTimerEnd+=EndPreparation;
            _isPreparation = true;
            OnJumpPreparationStart?.Invoke();
        }
        
        private void EndCooldown()
        {
            _jumpTimer.OnTimerEnd -= EndCooldown;
        }
        
        private void EndPreparation()
        {
            _jumpTimer.OnTimerEnd -= EndCooldown;
        }
        
        private void ChangeJumpForce(float preparationTimeElapsed)
        { 
            JumpForce = Mathf.Lerp(_movementData.MinimumJumpForce,_movementData.MaximumJumpForce, preparationTimeElapsed / _jumpTimer.MaxTime);
            OnJumpForceChange?.Invoke(JumpForce);
        }
    }
}