using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
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
}
