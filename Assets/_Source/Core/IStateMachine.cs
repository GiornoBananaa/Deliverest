using System;

namespace Core
{
    public interface IStateMachine<T> where T : AState
    {
        T CurrentState { get; }
        void ChangeState(T state);
        void Update();
    }
}