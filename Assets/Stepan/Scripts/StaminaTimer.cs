using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class StaminaTimer : MonoBehaviour
{
    [SerializeField] private Color fullColor, halfColor, quarterColor;
    [SerializeField] private Image fill;
    public float fillAmount
    {
        get => _fillAmount;
        set
        {
            _fillAmount = value;
            UpdateColor();
        }
    }
    private float _fillAmount;

    private void UpdateColor()
    {
        if (fillAmount > 0.5f)
            fill.color = Color.Lerp(halfColor, fullColor, (fillAmount - 0.5f) * 2);
        else
            fill.color = Color.Lerp(quarterColor, halfColor, fillAmount * 2);
    }
}
