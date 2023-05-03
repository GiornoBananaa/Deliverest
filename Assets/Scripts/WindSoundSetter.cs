using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSoundSetter : MonoBehaviour
{
    void Start()
    {
        if (!GameManager.instance.currentLevel.snow_storm)
        {
            gameObject.SetActive(false);
        }
    }
}
