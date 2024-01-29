namespace Core
{
    public abstract class AState
    {
        protected IStateMachine Owner;

        public void SetOwner(IStateMachine owner) => Owner = owner;

        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();
    }
}