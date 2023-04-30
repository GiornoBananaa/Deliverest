using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioSource _clickSource;

    public void StartNewGame()
    {
        _clickSource.Play();
        StartCoroutine(StartDelay());
    }

    public void OpenInstruction()
    {
        _clickSource.Play();
    }

    public void Autors()
    {
        _clickSource.Play();
        
    }

    private IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(0.2f);
        GameManager.instance.StartNewGame();
    }
}
