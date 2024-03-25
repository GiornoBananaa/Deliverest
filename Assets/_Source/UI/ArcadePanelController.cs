using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadePanelController : MonoBehaviour
{
    [SerializeField] private GameObject _panel;

    public void ActivatePanel()
    {
        if (_panel != null)
        {
            _panel.SetActive(true);
        }
        else
        {
            Debug.LogError("хде панель?");
        }
    }
}
