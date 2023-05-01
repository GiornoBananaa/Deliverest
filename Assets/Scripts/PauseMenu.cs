using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel, pauseButton;

    private void Start()
    {
        panel.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchPause();
        }
    }

    public void SwitchPause()
    {
        GameManager.instance.isPaused = !GameManager.instance.isPaused;
        panel.SetActive(GameManager.instance.isPaused);
        pauseButton.SetActive(!GameManager.instance.isPaused);
        if (GameManager.instance.isPaused)
        {
            AudioListener.pause = true;
            Time.timeScale = 0f;
        }
        else
        {
            AudioListener.pause = false;
            Time.timeScale = 1f;
        }
    }

    public void RestartLevel()
    {
        Time.timeScale = 1.0f;
        GameManager.instance.StartNewGame();
    }
    public void GoToMainMenu()
    {
        Time.timeScale = 1.0f;
        GameManager.instance.OpenMainMenu();
    }

}
