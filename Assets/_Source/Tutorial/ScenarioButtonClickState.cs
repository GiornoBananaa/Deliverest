using System;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    [Serializable]
    public class ScenarioButtonClickState: AScenarioState
    {
        [SerializeField] private Button _button;

        public override void Enter()
        {
            _button.onClick.AddListener(SendExitRequest);
            base.Enter();
        }
        
        public override void Update(){}
        
        public override void Exit()
        {
            base.Exit();
            _button.onClick.RemoveListener(SendExitRequest);
        }

        public override string ToString() => "WaitButtonClick";
    }
}