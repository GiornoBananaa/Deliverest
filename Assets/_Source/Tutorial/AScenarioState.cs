using System;
using Core;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    [Serializable]
    public abstract class AScenarioState : AState
    {
        protected TutorialStateMachine Owner;
        
        public void SetOwner(TutorialStateMachine owner) => Owner = owner;

        protected void SendExitRequest()
        {
            Exit();
            Owner.StateExitRequest?.Invoke();
        }
    }
}
