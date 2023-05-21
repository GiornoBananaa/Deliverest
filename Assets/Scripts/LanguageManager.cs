using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class LanguageManager : MonoBehaviour
{
    private enum Language
    {
        English,
        Russian
    }

    [Serializable]
    private struct ImageLanguages
    {
        public Language language;
        [SerializeField] private Image image;
        [SerializeField] private Sprite[] languageSprites;
        private static Language gameLanguage;

        public static Language AllLanguage
        {
            get
            {
                return gameLanguage;
            }
            set
            {
                gameLanguage = value;
                languageChange.Invoke();
            }
        }

        public Language Language 
        {
            get
            {
                return language;
            }
            set
            {
                if ((int)value >= Enum.GetNames(typeof(Language)).Length || (int)value >= languageSprites.Length)
                    return;
                language = value;
                image.sprite = languageSprites[(int)value];
            }
        }
        
        public void ChangeLanguage()
        {
            Language = gameLanguage;
        }
    }


    [SerializeField] private ImageLanguages[] _imageLanguages;
    private static Action languageChange;

    private void Start()
    {
        for (int i = 0; i < _imageLanguages.Length; i++)
        {
            languageChange += _imageLanguages[i].ChangeLanguage;
        }
        ImageLanguages.AllLanguage = (Language)PlayerPrefs.GetInt("Language", 0);
    }

    public void LanguageChange(int language)
    {
        PlayerPrefs.SetInt("Language", language);
        ImageLanguages.AllLanguage = (Language)language;
    }
}
