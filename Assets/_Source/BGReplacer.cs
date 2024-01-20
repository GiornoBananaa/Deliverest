using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGReplacer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer.sprite = GameManager.instance.currentLevel.backGround;
    }


}
