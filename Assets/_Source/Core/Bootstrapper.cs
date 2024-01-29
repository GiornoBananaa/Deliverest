using System;
using Character;
using InputSystem;
using UnityEngine;

namespace Core
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private CharacterMovement _characterMovement;
        [SerializeField] private InputListener _inputListener;
        
        private void Awake()
        {
            _inputListener.Construct(_characterMovement);
        }
    }
}
