using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public float height, target_height;
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
        ResetProgress();
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
    public void ResetProgress()
    {
        height = 0;
    }
}
