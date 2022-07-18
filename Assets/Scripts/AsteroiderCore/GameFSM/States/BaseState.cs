namespace AsteroiderCore.GameFSM.States
{
    public abstract class BaseState
    {
        protected GameStateMachine StateMachine { get; }

        protected BaseState(GameStateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }

        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Start() { }
        public virtual void Quit() { }
    }
}
