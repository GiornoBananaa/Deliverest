using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] _scenesMusic;
    [SerializeField] private AudioSource _musicPrefab;

    private static AudioSource _music;
    public static Action OnSoundVolumeChange;

    private void Awake()
    {
        if (GameManager.audoManager is not null && GameManager.audoManager != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        _music = Instantiate(_musicPrefab).GetComponent<AudioSource>();
        _music.transform.parent = null;

        _music.volume = MusicVolume;

        DontDestroyOnLoad(_music.gameObject);
    }

    private void OnLevelWasLoaded(int level)
    {
        if (level == 1 && _music is not null)
        {
            _music.clip = GameManager.instance.currentLevel.music_on_level;
            _music.Play();
        }
        else if((level == 0) && _music is not null)
        {
            if (_music.clip is null || _scenesMusic[0].name != _music.clip.name) 
            {
                _music.clip = _scenesMusic[0];
                _music.Play(); 
            }
        }
        else
        {
            if (_scenesMusic[level] != null)
            {
                if (_music.clip is null || _scenesMusic[level].name != _music.clip.name)
                {
                    _music.clip = _scenesMusic[level];
                    _music.Play();
                }
            }
            else
            {
                _music.clip = null;
            }
        }
    }

    public void MuteAudio(bool value)
    {
        Mute = value;
    }
    public void MusicVolumeChange(float value)
    {
        MusicVolume = value;
    }
    public void SoundVolumeChange(float value)
    {
        SoundVolume = value;
        OnSoundVolumeChange.Invoke();
    }


    public bool Mute
    {
        get
        {
            return PlayerPrefs.GetInt("Mute", 0) == 1 ? true : false;
        }
        set
        {
            _music.mute = !value;
            PlayerPrefs.SetInt("Mute", value ? 1 : 0);
        }
    }

    public float SoundVolume
    {
        get
        {
            return PlayerPrefs.GetFloat("Sound", 1f);
        }
        set
        {
            PlayerPrefs.SetFloat("Sound", (float)value);
        }
    }

    public float MusicVolume
    {
        get
        {
            return PlayerPrefs.GetFloat("Music", 0.15f);
        }
        set
        {
            PlayerPrefs.SetFloat("Music", (float)value);
            _music.volume = PlayerPrefs.GetFloat("Music", 0.15f);
        }
    }
}
