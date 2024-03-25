using UnityEngine;

namespace Character
{
    [CreateAssetMenu(fileName = "CharacterDataSO", menuName = "SO/CharacterDataSO")]
    public class CharacterDataSO: ScriptableObject
    {
        [field: SerializeField] public CharacterMovementData MovementData { get; private set;}
    }
}