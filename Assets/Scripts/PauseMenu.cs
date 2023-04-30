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
            Time.timeScale = 0f;
        else
            Time.timeScale = 1.0f;
    }

    public void RestartLevel()
    {
        GameManager.instance.RestartLevel();
    }
    public void GoToMainMenu()
    {
        GameManager.instance.OpenMainMenu();
    }
}
