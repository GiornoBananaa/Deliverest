using UnityEngine;
using TMPro;

public class LoseMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text bestHeight;
    [SerializeField] private TMP_Text height;
    void Start()
    {
        if (bestHeight != null)
        {

            bestHeight.text = $"Best height: {(int)GameManager.instance.best_height}";
        }
        
        if (height != null)
        {

            bestHeight.text = $"Height: {(int)GameManager.instance.height}";
        }
    }
    public void GoToMainMenu()
    {
        GameManager.instance.OpenMainMenu();

    }
    public void RestartLevel()
    {
        GameManager.instance.RestartLevel();
    }

}
