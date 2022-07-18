using System;
using AsteroiderCore.Settings;
using AsteroiderCore.UI;
using UnityEngine.SceneManagement;

namespace AsteroiderCore.GameFSM.States
{
    public sealed class BattleState : BaseState
    {
        private readonly GameSettings _settings;
        private readonly BattleController _battleController;
        private readonly Func<IPresenter> _presenterFabric;
        private IPresenter _presenterUI;
        private bool _sceneLoaded;

        public BattleState(GameStateMachine stateMachine, GameSettings settings, 
            BattleController battleController, Func<GamePresenter> presenterFabric) : base(stateMachine)
        {
            _settings = settings;
            _battleController = battleController;
            _presenterFabric = presenterFabric;
        }

        public override void Enter()
        {           
            SceneManager.LoadScene(_settings.GameSceneBuildId, LoadSceneMode.Single);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public override void Exit()
        {
            if(_sceneLoaded)
            {
                _battleController.EndBattle();
                _presenterUI.Clear();
                _presenterUI = null;
            }
            else
            {
                SceneManager.sceneLoaded -= OnSceneLoaded;
            }
        }

        public override void Quit()
        {
            StateMachine.SetState<MainMenuState>();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            _sceneLoaded = true;
            SceneManager.sceneLoaded -= OnSceneLoaded;
            _presenterUI = _presenterFabric.Invoke();
            _presenterUI.Initialize();
            _battleController.StartBattle();
        }
    }
}