using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioSource _clickSource;
    [SerializeField] private GameObject[] instrusionSlides;
    [SerializeField] private GameObject instructionCanvas;

    private int slideID = 0;
    private void Start()
    {
        HideAllSlides();
        instructionCanvas.SetActive(false);
    }
    public void StartNewGame()
    {
        _clickSource.Play();
        StartCoroutine(StartDelay());
    }

    public void OpenInstruction()
    {
        slideID = 0;
        _clickSource.Play();
        instructionCanvas.SetActive(true);
        ShowSlide(0);
    }
    public void CloseInstruction()
    {
        instructionCanvas.SetActive(false);
       
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
    public void ShowNext()
    {
        if (++slideID < instrusionSlides.Length)
            ShowSlide(slideID);
        else
            CloseInstruction();
    }
    public void ShowPrevious()
    {
        if (--slideID >= 0)
            ShowSlide(slideID);
        else
            CloseInstruction();
    }
    private void ShowSlide(int ID)
    {
        HideAllSlides();
        instrusionSlides[ID].SetActive(true);
    }
    private void HideAllSlides()
    {
        foreach(GameObject slide in instrusionSlides)
        {
            slide.SetActive(false);
        }
    }

}
