using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public Level[] levels; 
    public Level currentLevel
    {
        get => levels[currentLevelID];
    }

    private int currentLevelID;
    [HideInInspector] public float best_height;
    public float height
    {
        get => _height;
        set
        {
            _height = value;
            if (_height >= currentLevel.target_height)
                WinGame();
        }
    }
    [HideInInspector] public bool isPaused;
    [HideInInspector] public bool isInSafePlace;
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
        LoadValues();
    }

    public void StartNewGame()
    {
        isPaused = false;
        ResetProgress();
        SceneManager.LoadScene(1);
    }
    public void NextLevel()
    {
        
        if(++currentLevelID >= levels.Length)
        {
            currentLevelID = 0;
            OpenMainMenu();
        }else
        {
            SceneManager.LoadScene(1);
        }

    }

    public void LoseGame(bool loseTime)
    {
        if (height > best_height)
            best_height = height;
        if (loseTime)
            SceneManager.LoadScene(4);
        else
            SceneManager.LoadScene(2);

    }
    public void WinGame()
    {
        if (height > best_height)
            best_height = height;

        SceneManager.LoadScene(currentLevel.levelWinSceneID);
    }
    public void RestartLevel()
    {
        isPaused = false;
        if (height > best_height)
            best_height = height;

        if(currentLevelID >= 1)
        {
            height = levels[currentLevelID - 1].target_height;
        }
        else
        {
            height = 0;
        }

        SceneManager.LoadScene(1);
    }
    public void OpenMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ResetProgress()
    {
        height = 0;
        currentLevelID = 0;
    }

    private void SaveValues()
    {
        PlayerPrefs.SetFloat("bestHeight", best_height);
    }
    private void LoadValues()
    {
        best_height = PlayerPrefs.GetFloat("bestHeight");
    }
    private void OnApplicationQuit()
    {
        SaveValues();
    }
}
