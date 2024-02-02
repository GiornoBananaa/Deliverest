using System;
using UnityEngine;

namespace Audio
{
    [Serializable]
    public class Sound
    {
        public bool IsMusic;
        public string Name;
        [Range(0f, 1f)]
        public float Volume = 1;
        [Range(0.1f, 3f)]
        public float Pitch = 1;
        [Range(0f, 1f)]
        public float PitchVariation = 0;
        public bool Loop;
        public bool PlayOnAwake;
        
        [SerializeField] 
        private AudioClip[] Clips;
        
        public AudioClip Clip
        {
            get {return Clips[UnityEngine.Random.Range(0, Clips.Length)];}
        }
        
        public void SetSourceVariables(AudioSource source, float generalVolume)
        {
            source.clip = Clip;
            source.loop = Loop;
            source.pitch = Mathf.Clamp( Pitch + UnityEngine.Random.Range(-PitchVariation, PitchVariation), 0.1f, 3f);
            SetSourceVolume(source, generalVolume);
        }

        public void SetSourceVolume(AudioSource source, float generalVolume)
        {
            source.volume = Volume*generalVolume;
        }
    }
}