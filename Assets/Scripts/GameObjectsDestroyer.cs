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
            StartCoroutine(DestroyObject(collision.gameObject));
        }
    }
    
    private IEnumerator DestroyObject(GameObject destroyableObject)
    {
        AudioSource audio = destroyableObject.GetComponent<AudioSource>();

        while (audio.volume > 0.01f)
        {
            yield return new WaitForFixedUpdate();
            audio.volume = Mathf.Lerp(audio.volume, 0, 0.03f);
        }
        Destroy(destroyableObject);
    }
}
