using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightBar : MonoBehaviour
{
    [SerializeField] private RectTransform arrow;
    [SerializeField] private float barHeight;
    private void Update()
    {
        arrow.anchoredPosition = new Vector2(0, barHeight * (GameManager.instance.height / GameManager.instance.currentLevel.target_height));
    }

}
