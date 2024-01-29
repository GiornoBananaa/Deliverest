using System;
using Core;

namespace Tutorial
{
    public class TutorialStateMachine: IStateMachine<AScenarioState>
    {
        private AScenarioState _currState;

        public Action StateExitRequest;
        
        public AScenarioState CurrentState => _currState;
        
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
