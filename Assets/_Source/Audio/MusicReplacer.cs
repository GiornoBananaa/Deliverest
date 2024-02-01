using UnityEngine;
using UnityEngine.SceneManagement;

namespace Audio
{
    public class MusicReplacer : MonoBehaviour
    {
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private string _soundName;
        
        private AudioPlayer _audioPlayer;
        
        public void Construct(AudioPlayer audioPlayer)
        {
            _audioPlayer = audioPlayer;
        }
        
        void Start()
        {
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                _musicSource.clip = GameManager.instance.currentLevel.music_on_level;
                _musicSource.Play();
            }
            else
            {
                _audioPlayer.RegisterSpatial(_soundName,_musicSource);
                _musicSource.Play();
            }
        }
    }
}
