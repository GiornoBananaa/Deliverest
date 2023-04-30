using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public float target_height, best_height;
    public float height
    {
        get => _height;
        set
        {
            _height = value;
            if (_height >= target_height)
                WinGame();
        }
    }
    public bool isPaused;
    public bool isInSafePlace;
    private float _height;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void StartNewGame()
    {
        isPaused = false;
        ResetAllProgress();
        SceneManager.LoadScene(1);
    }
    public void LoadGame()
    {
        LoadValues();
        SceneManager.LoadScene(1);
    }
    public void LoseGame()
    {
        if (height > best_height)
            best_height = height;
        SceneManager.LoadScene(2);
    }
    public void WinGame()
    {
        if (height > best_height)
            best_height = height;
        SceneManager.LoadScene(3);
    }
    public void OpenMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void RestartLevel()
    {
        ResetLevelProgress();
        isPaused = false;
        SceneManager.LoadScene(1);
    }
    public void ResetLevelProgress()
    {
        height = 0;
    }
    public void ResetAllProgress()
    {
        ResetLevelProgress();
        PlayerPrefs.DeleteAll();
    }

    private void SaveValues()
    {

       PlayerPrefs.SetFloat("bestHeight", best_height);
       PlayerPrefs.SetFloat("height", height);

    }
    private void LoadValues()
    {
        height = PlayerPrefs.GetFloat("height");
        best_height = PlayerPrefs.GetFloat("height");
    }
    private void OnApplicationQuit()
    {
        SaveValues()
    }
}
