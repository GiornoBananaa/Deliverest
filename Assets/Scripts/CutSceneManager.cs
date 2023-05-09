using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CutSceneManager : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private Image _image;
    [SerializeField] private int _nextScene;
    [SerializeField] private bool _nextLevel;

    private int _currentImage;

    void Start()
    {
        _currentImage = 0;
        _image.sprite = _sprites[_currentImage];
    }

    public void NextSprite()
    {
        if (_currentImage < _sprites.Length-1) 
        {
            _currentImage++;
            _image.sprite = _sprites[_currentImage];
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
