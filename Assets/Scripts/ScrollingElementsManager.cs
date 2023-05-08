using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingElementsManager : MonoBehaviour
{
    [SerializeField] private Transform _panel;
    [SerializeField] private float _leftBorderX;
    [SerializeField] private float _rightBorderX;

    private RectTransform _rectTransform;
    private float _border;

    private void Start()
    {
        if(Application.platform == RuntimePlatform.WebGLPlayer)
        {
            _border = 300;
            _rightBorderX = 1116;
        }
        else
        {
            _border = 550;
            _rightBorderX = 3100;
        }

        _rectTransform = GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        if(_rectTransform.position.x > _rightBorderX)
            MoveAuthor(true);
        else if(_rectTransform.position.x < _leftBorderX)
            MoveAuthor(false);
    }
    
    public void MoveAuthor(bool isRight)
    {
        if (isRight)
        {
            _rectTransform.position = _panel.GetChild(0).position + new Vector3(-_border, 0,0);
            _rectTransform.SetAsFirstSibling();
        }
        else
        {
            _rectTransform.position = _panel.GetChild(_panel.childCount-1).position + new Vector3(_border, 0, 0);
            _rectTransform.SetAsLastSibling();
        }
    }
}
