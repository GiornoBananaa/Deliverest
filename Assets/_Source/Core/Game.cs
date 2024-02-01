using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class Game
    {
        public Action<bool> OnPause;
        
        public void GoToMainMenu()
        {
            SceneManager.LoadScene(0);
        }
        
        public void StartArcadeGame()
        {
            SceneManager.LoadScene(1);
        }
        
        public void Pause(bool isPaused)
        {
            OnPause?.Invoke(isPaused);
            Time.timeScale = isPaused?0:1;
        }
    }
}
