using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource _musicSource;
        
        private Dictionary<string,Sound> _sounds;
        private Dictionary<string,AudioSource> _soundSources;
        private Dictionary<string,AudioSource> _spatialSources;
        
        public bool MusicIsMuted { get; private set; }
        public bool SoundIsMuted { get; private set; }
        public float SoundVolume { get; private set; }
        public float MusicVolume { get; private set; }

        public void Construct(Dictionary<string,Sound> sounds)
        {
            _sounds = sounds;
            
            _soundSources = new Dictionary<string, AudioSource>();
            _spatialSources = new Dictionary<string, AudioSource>();
            
            SoundVolume = PlayerPrefs.GetFloat("SoundVolume",0.5f);
            MusicVolume = PlayerPrefs.GetFloat("MusicVolume",0.5f);
            
            foreach (Sound sound in _sounds.Values)
            {
                AddSound(sound);
            }
            
            SoundVolumeChange(SoundVolume);
            MusicVolumeChange(MusicVolume);
        }
        
        public void SoundVolumeChange(float volume)
        {
            SoundVolume = volume;
            foreach (Sound sound in _sounds.Values)
            {
                if(sound.IsMusic) continue;
                
                sound.SetSourceVolume(_soundSources[sound.Name],SoundVolume);
                if (_spatialSources.TryGetValue(name, out var source))
                {
                    sound.SetSourceVolume(source,SoundVolume);
                }    
            }
        }
        
        public void MusicVolumeChange(float volume)
        {
            MusicVolume = volume;
            foreach (Sound sound in _sounds.Values)
            {
                if(!sound.IsMusic) continue;
                sound.SetSourceVolume(_soundSources[sound.Name],MusicVolume);
            }
        }
    
        public void EnableSound(bool enable)
        {
            SoundIsMuted = enable;
            foreach (AudioSource source in _soundSources.Values)
            {
                if(source == _musicSource) continue;
                    source.mute = !enable;
            }
            foreach (AudioSource source in _spatialSources.Values)
            {
                if(source == _musicSource) continue;
                source.mute = !enable;
            }
        }
        
        public void EnableMusic(bool enable)
        {
            MusicIsMuted = enable;
            
            _musicSource.mute = !enable;
        }
        
        private void AddSound(Sound sound)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            sound.SetSourceVolume(source,MusicVolume);
            if (sound.PlayOnAwake)
            {
                Play(sound);
            }
        }
        
        public void Play(Sound sound)
        {
            Play(sound.Name);
        }
        
        public void Play(string name)
        {
            if (_soundSources.TryGetValue(name, out var source))
            {
                Sound sound = _sounds[name];
                if (source == _musicSource)
                {
                    if(sound.Clip == _musicSource.clip)
                        return;
                    source.clip = sound.Clip;
                    sound.SetSourceVariables(_musicSource,MusicVolume);
                }
                source.Play();
            }
            else
                Debug.LogWarning("Sound " + name + " not found");
        }
        
        public void RegisterSpatial(string name, AudioSource audioSource)
        {
            if (_sounds.TryGetValue(name, out var sound))
            {
                sound.SetSourceVariables(audioSource,SoundVolume);
                
                _spatialSources.Add(name,audioSource);
            }
            else
                Debug.LogWarning("Sound " + name + " not found");
        }

        private void OnDestroy()
        {
            PlayerPrefs.SetFloat("MusicVolume", MusicVolume);
            PlayerPrefs.SetFloat("SoundVolume", SoundVolume);
            PlayerPrefs.Save();
        }
    }
}