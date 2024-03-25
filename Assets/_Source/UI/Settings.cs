using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _soundSlider;
    [SerializeField] private TMP_Dropdown _languageDropDown;

    void Start()
    {
        _musicSlider.value = GameManager.AudoPlayer.MusicVolume;
        _soundSlider.value = GameManager.AudoPlayer.SoundVolume;

        _musicSlider.onValueChanged.AddListener(GameManager.AudoPlayer.MusicVolumeChange);
        _soundSlider.onValueChanged.AddListener(GameManager.AudoPlayer.SoundVolumeChange);
    }
}
