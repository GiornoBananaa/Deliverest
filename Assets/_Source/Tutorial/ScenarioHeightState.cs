using System;
using UnityEngine;

namespace Tutorial
{
    [Serializable]
    public class ScenarioHeightState: AScenarioState
    {
        public override void Enter()
        {
            GameManager.instance.OnTopReached += SendExitRequest;
        }

        public override void Update() { }

        public override void Exit()
        {
            GameManager.instance.OnTopReached -= SendExitRequest;
        }
        
        public override string ToString() => "WaitForReachingTop";
    }
}