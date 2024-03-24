using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Character
{
    public class JumpBarView: MonoBehaviour
    {
        [SerializeField] private GameObject _jumpDirectionArrow;
        [SerializeField] private Vector3 _minimumArrowSize;
        [SerializeField] private Vector3 _maximumArrowSize;
        [SerializeField] private float _minimumBlinkSpeed;
        [SerializeField] private float _maximumBlinkSpeed;
        [SerializeField] private SpriteRenderer[] _blinkingBodyParts;
        //TODO JumpBarView ui
        private CharacterMovementData _movementData;
        private CharacterJump _characterJump;

        private void Start()
        {
            _characterJump.OnJumpDirectionChange+=ChangeJumpDirection;
            _characterJump.OnJumpForceChange+=ChangeArrowSize;
            _characterJump.OnJumpPreparationStart +=EnableArrow;
            _characterJump.OnJump +=DisableArrow;
        }

        public void Construct(CharacterMovementData movementData,CharacterJump characterJump)
        {
            _movementData = movementData;
            _characterJump = characterJump;
        }

        #region BodyBlinking

        //TODO BodyBlinking
        
        private void ChangeBlinkSpeed(float force)
        {
            float blinkSpeed = Mathf.Lerp(_minimumBlinkSpeed,_maximumBlinkSpeed,force/_movementData.MaximumJumpForce);
            foreach (var sprite in _blinkingBodyParts)
            {
                sprite.DOColor(Color.yellow, blinkSpeed).SetLoops(-1,LoopType.Yoyo);
            }
        }

        #endregion
        
        #region JumpDirectionArrow
        
        private void ChangeJumpDirection(Vector2 direction)
        {
            _jumpDirectionArrow.transform.up = direction.normalized;
        }
        
        private void ChangeArrowSize(float force)
        {
            
            _jumpDirectionArrow.transform.localScale = Vector3.Lerp(_minimumArrowSize,_maximumArrowSize,
                (force-_movementData.MinimumJumpForce)/(_movementData.MaximumJumpForce-_movementData.MinimumJumpForce));
        }

        private void DisableArrow() => _jumpDirectionArrow.SetActive(false);
        private void EnableArrow()
        {
            _jumpDirectionArrow.SetActive(true);
            _jumpDirectionArrow.transform.localScale = _minimumArrowSize;
        }

        #endregion
        
        private void OnDestroy()
        {
            _characterJump.OnJumpDirectionChange -= ChangeJumpDirection;
            _characterJump.OnJumpForceChange -= ChangeArrowSize;
            _characterJump.OnJumpPreparationStart -= EnableArrow;
            _characterJump.OnJump -= DisableArrow;
        }
    }
}