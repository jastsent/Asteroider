using System;
using System.Collections.Generic;
using AsteroiderCore.GameFSM.States;

namespace AsteroiderCore.GameFSM
{
    public sealed class GameStateMachine
    {
        private readonly IReadOnlyDictionary<Type, Func<BaseState>> _stateFabrics;
        private BaseState _currentState;

        public GameStateMachine(IReadOnlyDictionary<Type, Func<BaseState>> stateFabrics)
        {
            _stateFabrics = stateFabrics;
        }

        public void SetState<T>() where T : BaseState
        {
            var stateType = typeof(T);
            SetState(stateType);
        }

        public void Start()
        {
            _currentState?.Start();
        }

        public void Quit()
        {
            _currentState?.Quit();
        }
        
        private void SetState(Type stateType)
        {
            if(_stateFabrics.TryGetValue(stateType, out var newStateFabric))
            {
                _currentState?.Exit();
                var newState = newStateFabric.Invoke();
                newState?.Enter();
                _currentState = newState;
            }
        }
    }
}