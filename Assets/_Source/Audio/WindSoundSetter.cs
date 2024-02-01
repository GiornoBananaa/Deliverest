using UnityEngine;

namespace Audio
{
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
}
