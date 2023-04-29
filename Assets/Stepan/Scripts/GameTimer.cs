using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameTimer : MonoBehaviour
{
    [SerializeField] private float startTime;
    [SerializeField] private Color fullColor, halfColor, quarterColor;
    [SerializeField] private Image fill;
    [SerializeField] private TMP_Text text;
    private float timeLeft;
    private void Start()
    {
        timeLeft = startTime;
        UpdatePorgressBar();
    }
    public void Reset()
    {
        timeLeft = startTime;
        UpdatePorgressBar();
    }
    private void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
        }
        else
        {
            timeLeft = 0;
            //Смерть
        }
        text.text = $"{timeLeft:00.00}";
        UpdatePorgressBar();
    }
    private void UpdatePorgressBar()
    {
        float ratio = timeLeft / startTime;
        if (ratio > 0.5f)
            fill.color = fullColor;
        else if (ratio > 0.25f)
            fill.color = halfColor;
        else
            fill.color = quarterColor;
        fill.fillAmount = ratio;
    }
}
