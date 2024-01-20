using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoseMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text bestHeight;
    void Start()
    {
        if (bestHeight != null)
        {

            bestHeight.text = $"Best height: {(int)GameManager.instance.best_height}";
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
