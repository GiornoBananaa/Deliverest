using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CutSceneManager : MonoBehaviour
{
    private enum Language
    {
        English,
        Russian
    }

    [Serializable]
    private struct CutscenesLanguages
    {
        [SerializeField] private Sprite Russian;
        [SerializeField] private Sprite English;

        public Sprite GetSprite(Language language)
        {
            switch (language)
            {
                case Language.Russian:
                    return Russian;
                case Language.English:
                    return English;
                default:
                    return English;
            }
        }
    }

    [SerializeField] private CutscenesLanguages[] _spritesLanguage;
    [SerializeField] private Image _image;
    [SerializeField] private int _nextScene;
    [SerializeField] private bool _nextLevel;

    private int _currentImage;        
    private Language _language;        

    void Start()
    {
        _language = (Language)PlayerPrefs.GetInt("Language", 0);
        _currentImage = 0;
        _image.sprite = _spritesLanguage[_currentImage].GetSprite(_language);
    }

    public void NextSprite()
    {
        if (_currentImage < _spritesLanguage.Length-1) 
        {
            _currentImage++;
            _image.sprite = _spritesLanguage[_currentImage].GetSprite(_language);
        }
        else
        {
            if (_nextLevel)
                GameManager.instance.NextLevel();
            else
                SceneManager.LoadScene(_nextScene);
        }

    }
}
