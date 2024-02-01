using System;
using Core;
using UnityEngine.UI;

namespace Tutorial
{
    public class TutorialStateMachine: IStateMachine<AScenarioState>
    {
        private AScenarioState _currState;
        private Image _dialogueImage;
        
        public Action StateExitRequest;
        
        public AScenarioState CurrentState => _currState;
        public TutorialReferencesContainer TutorialReferencesContainer { get; private set; }
        
        public void SetTutorialReferences(TutorialReferencesContainer tutorialReferencesContainer)
        {
            TutorialReferencesContainer = tutorialReferencesContainer;
        }
        
        public void ChangeState(AScenarioState state)
        {
            if(_currState != null)
            {
                _currState?.Exit();
            }
            _currState = state;
            _currState.SetOwner(this);
            _currState.Enter();
        }
        
        public void Update() => _currState.Update();
    }
}
