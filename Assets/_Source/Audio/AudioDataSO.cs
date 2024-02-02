using UnityEngine;

namespace Audio
{
    [CreateAssetMenu(fileName = "AudioDataSO", menuName = "SO/AudioDataSO")]
    public class AudioDataSO: ScriptableObject
    {
        [field: SerializeField] public Sound[] Sounds { get; private set;}
    }
}