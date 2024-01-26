using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class LanguageManager : MonoBehaviour
{
    public enum Language
    {
        English,
        Russian
    }

    private abstract class AMultiLanguage
    {
        protected Language language;

        public virtual Language Language { get; set; }

        public void ChangeLanguage()
        {
            Language = GameLanguage;
        }
    }

    [Serializable]
    private class ImageLanguages : AMultiLanguage
    {
        [SerializeField] private Image image;
        [SerializeField] private Sprite Russian;
        [SerializeField] private Sprite English;

        public override Language Language 
        {
            get
            {
                return language;
            }
            set
            {
                language = value;
                switch (language)
                {
                    case Language.Russian:
                        image.sprite = Russian;
                        break;
                    case Language.English:
                        image.sprite = English;
                        break;
                    default:
                        break;
                }
            }
        }
    }

    [Serializable]
    private class TextLanguages : AMultiLanguage
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private string Russian;
        [SerializeField] private string English;

        public override Language Language
        {
            get
            {
                return language;
            }
            set
            {
                language = value;
                switch (language)
                {
                    case Language.Russian:
                        text.text = Russian;
                        break;
                    case Language.English:
                        text.text = English;
                        break;
                    default:
                        break;
                }
            }
        }
    }

    [SerializeField] private ImageLanguages[] _imageLanguages;
    [SerializeField] private TextLanguages[] _textLanguages;

    private static Action languageChange;
    private static Language gameLanguage;

    private static Language GameLanguage
    {
        get
        {
            return gameLanguage;
        }
        set
        {
            gameLanguage = value;
            languageChange?.Invoke();
        }
    }

    private void Start()
    {
        if (_imageLanguages is not null) 
        {
            for (int i = 0; i < _imageLanguages.Length; i++)
            {
                languageChange += _imageLanguages[i].ChangeLanguage;
            }
        }
        if (_textLanguages is not null)
        {
            for (int i = 0; i < _textLanguages.Length; i++)
            {
                languageChange += _textLanguages[i].ChangeLanguage;
            }
        }
        GameLanguage = (Language)PlayerPrefs.GetInt("Language", 0);
    }

    public void LanguageChange(int language)
    {
        PlayerPrefs.SetInt("Language", language);
        GameLanguage = (Language)language;
    }
}
