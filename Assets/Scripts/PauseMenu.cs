using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject Canvas;
    private void Start()
    {
        Canvas.SetActive(false);
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
        Canvas.SetActive(GameManager.instance.isPaused);
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
