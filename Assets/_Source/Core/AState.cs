namespace Core
{
    public abstract class AState
    {
        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();
    }
}