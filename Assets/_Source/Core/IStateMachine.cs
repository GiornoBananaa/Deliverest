using System;

namespace Core
{
    public interface IStateMachine
    {
        AState CurrentState { get; }
        void ChangeState(AState state);
        void Update();
    }
}