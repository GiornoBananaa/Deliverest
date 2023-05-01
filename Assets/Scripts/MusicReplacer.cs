using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicReplacer : MonoBehaviour
{
    [SerializeField] private AudioSource _musicSource;

    void Start()
    {
        _musicSource.clip = GameManager.instance.currentLevel.music_on_level;
        _musicSource.Play();
    }
}
