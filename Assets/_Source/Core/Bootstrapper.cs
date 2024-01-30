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

        private Game _game;
        
        private void Awake()
        {
            _game = new Game();
            _inputListener.Construct(_characterMovement);
        }
    }
}
