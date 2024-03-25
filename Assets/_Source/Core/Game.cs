using UnityEngine.SceneManagement;

namespace Core
{
    public class Game
    {
        public void GoToMainMenu()
        {
            SceneManager.LoadScene(0);
        }
        
        public void StartArcadeGame()
        {
            SceneManager.LoadScene(1);
        }
    }
}
