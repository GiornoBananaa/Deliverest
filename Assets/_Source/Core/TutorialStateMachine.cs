
using System;

namespace Core
{
    public class TutorialStateMachine: IStateMachine
    {
        private AState _currState;

        public Action StateExit;
        
        public AState CurrentState => _currState;
        
        public void ChangeState(AState state)
        {
            if(_currState != null)
            {
                _currState?.Exit();
                StateExit?.Invoke();
            }
            _currState = state;
            _currState.Enter();
        }
        
        public void Update() => _currState.Update();
    }
}
