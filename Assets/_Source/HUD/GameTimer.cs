using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameTimer : MonoBehaviour
{
    [SerializeField] private Color fullColor, halfColor, quarterColor;
    [SerializeField] private Image fill;
    [SerializeField] private TMP_Text text;
    private float timeLeft;
    private void Start()
    {
        timeLeft = GameManager.instance.currentLevel.time_for_level;
        UpdatePorgressBar();
    }
    public void Reset()
    {
        timeLeft = GameManager.instance.currentLevel.time_for_level;
        UpdatePorgressBar();
    }
    private void Update()
    {
        if (GameManager.instance.isPaused)
            return;
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
        }
        else
        {
            timeLeft = 0;
            GameManager.instance.LoseGame(true);
        }
        text.text = $"{(int)timeLeft / 60:00}:{timeLeft % 60:00.00}";
        UpdatePorgressBar();
    }
    private void UpdatePorgressBar()
    {
        float ratio = timeLeft / GameManager.instance.currentLevel.time_for_level;
        if (ratio > 0.5f)
            fill.color = fullColor;
        else if (ratio > 0.25f)
            fill.color = halfColor;
        else
            fill.color = quarterColor;
        fill.fillAmount = ratio;
    }
}
