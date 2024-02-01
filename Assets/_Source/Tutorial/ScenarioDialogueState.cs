using System;
using UnityEngine;

namespace Tutorial
{
    [Serializable]
    public class ScenarioDialogueState: AScenarioState
    {
        public override void Enter()
        {
            base.Enter();
        }

        public override void Update() { }

        public override void Exit()
        {
            base.Exit();
        }
        
        public override string ToString() => "DialogueOnly";
    }
}