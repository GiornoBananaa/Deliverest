using Character;
using InputSystem;
using UnityEngine;

namespace Core
{
    public class Bootstrapper : MonoBehaviour
    {
        private const string _characterDataPath = "CharacterDataSO";
        
        [SerializeField] private CharacterMovement _characterMovement;
        [SerializeField] private InputListener _inputListener;
        [SerializeField] private SnowStormManager _stormManager;
        
        private Game _game;
        private CharacterDataSO _characterDataSo;
        
        private void Awake()
        {
            _game = new Game();
            _characterDataSo = Resources.Load<CharacterDataSO>(_characterDataPath);
            _characterMovement.Construct(_stormManager, _characterDataSo.MovementData);
            _inputListener.Construct(_characterMovement);
        }
    }
}
