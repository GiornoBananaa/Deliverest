using System;
using UnityEngine;

namespace Tutorial
{
    [Serializable]
    public class ScenarioRockHookState: AScenarioState
    {
        [SerializeField] private GameObject _hitch;
        [SerializeField] private GameObject _hitchHint;
        [SerializeField] private bool _looseStamina = true;
        
        public override void Enter()
        {
            Owner.TutorialReferencesContainer.CharacterMovement.EnableStamina(_looseStamina);
            Owner.TutorialReferencesContainer.CharacterMovement.OnHandHooked += CheckHookedHitch;
            _hitchHint.gameObject.SetActive(true);
        }
        
        public override void Update() { }

        public override void Exit()
        {
            Owner.TutorialReferencesContainer.CharacterMovement.OnHandHooked -= CheckHookedHitch;
            _hitchHint.gameObject.SetActive(false);
        }
        
        public override string ToString() => "WaitRockHook";

        private void CheckHookedHitch(GameObject hitch)
        {
            if (hitch == _hitch)
                SendExitRequest();
        }
    }
}