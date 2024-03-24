using Character;
using UnityEngine;

namespace InputSystem
{
    public class InputListener : MonoBehaviour
    {
        private CharacterMovement _characterMovement;
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        public void Construct(CharacterMovement characterMovement)
        {
            _characterMovement = characterMovement;
        }
        
        private void Update()
        {
            ReadLeftArmMove();
            ReadRightArmMove();
            ReadJump();
        }
        
        private void ReadLeftArmMove()
        {
            if (Input.GetMouseButtonDown(0))
                _characterMovement.StartLeftArmMove();
            if (Input.GetMouseButtonUp(0))
                _characterMovement.EndLeftArmMove();
        }
        
        private void ReadRightArmMove()
        {
            if (Input.GetMouseButtonDown(1))
                _characterMovement.StartRightArmMove();
            if (Input.GetMouseButtonUp(1))
                _characterMovement.EndRightArmMove();
        }
        
        private void ReadJump()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                _characterMovement.PrepareForJump();
            else if (Input.GetKey(KeyCode.Space))
                _characterMovement.SetJumpDirection(_camera.ScreenToWorldPoint(Input.mousePosition));
            else if (Input.GetKeyUp(KeyCode.Space))
                _characterMovement.Jump();
        }
    }
}
