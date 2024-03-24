using System;
using UnityEngine.Serialization;

namespace Character
{
    [Serializable]
    public class CharacterMovementData
    {
        public float LossHeight;
        public float LossY;
        public float MinimumJumpForce;
        public float MaximumJumpForce;
        public float JumpPreparationTime;
        public float JumpReloadTime;
        public float HookRadius;
        public float MaxStaminaTime;
        public float MaxStaminaTimeWhileStorm;
        public float FallingForLossTime;
        public float StormForce;
    }
}