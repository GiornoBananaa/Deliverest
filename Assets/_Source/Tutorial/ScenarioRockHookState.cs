using System;
using UnityEngine;

namespace Tutorial
{
    [Serializable]
    public class ScenarioRockHookState: AScenarioState
    {
        public override void Enter()
        {
            Owner.TutorialReferencesContainer.CharacterMovement.OnHandHooked += SendExitRequest;
            base.Enter();
        }

        public override void Update() { }

        public override void Exit()
        {
            base.Exit();
            Owner.TutorialReferencesContainer.CharacterMovement.OnHandHooked -= SendExitRequest;
        }
        
        public override string ToString() => "WaitRockHook";
    }
}