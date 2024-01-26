using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinMenu : MonoBehaviour
{
    public void GoToMainMenu()
    {
        GameManager.instance.OpenMainMenu();
    }
    public void NextLevel()
    {
        GameManager.instance.NextLevel();
    }
}
