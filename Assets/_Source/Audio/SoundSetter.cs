using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSetter : MonoBehaviour
{
    private AudioSource _audioSource;
    private float _maxSound;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _maxSound = _audioSource.volume;
        _audioSource.volume *= GameManager.audoManager.SoundVolume;
    }

    private void OnSoundVolumeChange()
    {
        _audioSource.volume = GameManager.audoManager.SoundVolume * _maxSound;
    }

    private void OnEnable()
    {
        AudioManager.OnSoundVolumeChange += OnSoundVolumeChange;
    }

    private void OnDisable()
    {
        AudioManager.OnSoundVolumeChange -= OnSoundVolumeChange;
    }
}
