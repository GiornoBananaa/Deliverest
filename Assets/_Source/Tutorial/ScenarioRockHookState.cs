using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tutorial
{
    [Serializable]
    public class ScenarioRockHookState: AScenarioState
    {
        [SerializeField] private GameObject _grip;
        [SerializeField] private GameObject _gripHint;
        [SerializeField] private bool _looseStamina = true;
        
        public override void Enter()
        {
            Owner.TutorialReferencesContainer.CharacterMovement.EnableStamina(_looseStamina);
            Owner.TutorialReferencesContainer.CharacterMovement.OnHandHooked += CheckHookedGrip;
            _gripHint.gameObject.SetActive(true);
        }
        
        public override void Update() { }

        public override void Exit()
        {
            Owner.TutorialReferencesContainer.CharacterMovement.OnHandHooked -= CheckHookedGrip;
            _gripHint.gameObject.SetActive(false);
        }
        
        public override string ToString() => "WaitRockHook";

        private void CheckHookedGrip(GameObject grip)
        {
            if (grip == _grip)
                SendExitRequest();
        }
    }
}