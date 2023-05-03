using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScrollElementTrigger : MonoBehaviour
{
    [SerializeField] private bool _isRight;
    [SerializeField] private ScrollingElementsManager _scrollingElementsManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _scrollingElementsManager.MoveAuthor(_isRight, collision.GetComponent<RectTransform>());
    }
}
