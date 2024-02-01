using UnityEngine;

namespace Audio
{
    public abstract class SoundPlayer: MonoBehaviour
    {
        [SerializeField] protected AudioPlayer AudioPlayer;
    }
}