using System.Collections.Generic;
using System.Linq;
using Audio;
using Character;
using InputSystem;
using UnityEngine;

namespace Core
{
    public class Bootstrapper : MonoBehaviour
    {
        private const string _characterDataPath = "CharacterDataSO";
        private const string _audioDataPath = "AudioDataSO";
        
        [SerializeField] private CharacterMovement _characterMovement;
        [SerializeField] private InputListener _inputListener;
        [SerializeField] private SnowStormManager _stormManager;
        [SerializeField] private AudioPlayer _audioPlayer;
        
        private Game _game;
        private CharacterDataSO _characterDataSo;
        private AudioDataSO _audioData;
        
        private void Awake()
        {
            _game = new Game();
            if(_characterMovement!=null)
            {
                _characterDataSo = Resources.Load<CharacterDataSO>(_characterDataPath);
                _characterMovement.Construct(_stormManager, _characterDataSo.MovementData);
            }
            if(_inputListener!=null)
                _inputListener.Construct(_characterMovement);
            if(_audioPlayer!=null)
            {
                _audioData = Resources.Load<AudioDataSO>(_audioDataPath);
                Dictionary<string, Sound> sounds = _audioData.Sounds.ToDictionary(sound => sound.Name);
                _audioPlayer.Construct(sounds);
            }
        }
    }
}
