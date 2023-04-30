using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public float height, target_height;
    public bool isPaused;
    public bool isInSafePlace;
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
    public void LoseGame()
    {
        SceneManager.LoadScene(2);
    }
    public void WinGame()
    {
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
    }
}
