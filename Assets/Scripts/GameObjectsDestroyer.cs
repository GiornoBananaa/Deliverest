using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectsDestroyer : MonoBehaviour
{
    [SerializeField] private LayerMask _layers;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_layers == (_layers | (1 << collision.gameObject.layer)))
        {
            Destroy(collision.gameObject);
        }
    }
}
