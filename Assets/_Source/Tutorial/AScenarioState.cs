using System;
using Core;
using UnityEngine;

namespace Tutorial
{
    [Serializable]
    public abstract class AScenarioState: AState
    {
        protected TutorialStateMachine Owner;
        [SerializeField] 
        protected bool _pauseGame;
        
        public void SetOwner(TutorialStateMachine owner) => Owner = owner;
    }
}
