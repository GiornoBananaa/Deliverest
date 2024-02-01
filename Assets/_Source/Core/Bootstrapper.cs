using System;
using System.Collections.Generic;
using System.Linq;
using Audio;
using Character;
using InputSystem;
using Tutorial;
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
        [SerializeField] private TutorialScenario _tutorialScenario;
        
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
            {
                _inputListener.Construct(_characterMovement);
                _game.OnPause += PauseInputListener;
            }
            if(_audioPlayer!=null)
            {
                _audioData = Resources.Load<AudioDataSO>(_audioDataPath);
                Dictionary<string, Sound> sounds = _audioData.Sounds.ToDictionary(sound => sound.Name);
                _audioPlayer.Construct(sounds);
            }
            
            if(_tutorialScenario!=null)
            {
                _tutorialScenario.Construct(_game);
            }
        }

        private void OnDestroy()
        {
            _game.OnPause -= PauseInputListener;
        }

        private void PauseInputListener(bool isPaused)
        {
            _inputListener.gameObject.SetActive(!isPaused);
        }
    }
}
