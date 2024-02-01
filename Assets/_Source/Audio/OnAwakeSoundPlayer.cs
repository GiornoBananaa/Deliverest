using UnityEngine;
using UnityEngine.Serialization;

namespace Audio
{
    public class OnAwakeSoundPlayer : SoundPlayer
    {
        [SerializeField] private string _soundName;
        [SerializeField] private AudioPlayer _audioPlayer;
        
        private void OnEnable()
        {
            //_audioPlayer.Play(_soundName);
        }
    }
}