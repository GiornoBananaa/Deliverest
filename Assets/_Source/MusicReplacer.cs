using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicReplacer : MonoBehaviour
{
    [SerializeField] private AudioSource _musicSource;

    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            _musicSource.clip = GameManager.instance.currentLevel.music_on_level;
            _musicSource.Play();
        }
    }
}
