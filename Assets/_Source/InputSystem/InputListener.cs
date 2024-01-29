using Character;
using UnityEngine;

namespace InputSystem
{
    public class InputListener : MonoBehaviour
    {
        private CharacterMovement _characterMovement;
        
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
                _characterMovement.Jump();
        }
    }
}
