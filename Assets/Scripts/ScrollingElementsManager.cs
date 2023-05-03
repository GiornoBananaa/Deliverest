using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingElementsManager : MonoBehaviour
{
    [SerializeField] private Transform _panel;

    public void MoveAuthor(bool isRight, RectTransform element)
    {
        if (isRight)
        {
            element.position = _panel.GetChild(0).position + new Vector3(-550,0,0);
            element.SetAsFirstSibling();
        }
        else
        {
            element.position = _panel.GetChild(_panel.childCount-1).position + new Vector3(550, 0, 0);
            element.SetAsLastSibling();
        }
    }
}
