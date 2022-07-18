using System;
using AsteroiderCore.SaveSystem;
using AsteroiderCore.Settings;
using AsteroiderCore.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AsteroiderCore.GameFSM.States
{
    public class MainMenuState : BaseState
    {
        private readonly GameSettings _settings;
        private readonly ISaver _saveSystem;
        private readonly PlayerData _playerData;
        private readonly Func<IPresenter> _presenterFabric;
        private IPresenter _presenterUI;
        private bool _sceneLoaded;

        public MainMenuState(GameStateMachine stateMachine, GameSettings settings, ISaver saveSystem,
            PlayerData playerData, Func<MenuPresenter> presenterFabric) : base(stateMachine)
        {
            _settings = settings;
            _saveSystem = saveSystem;
            _playerData = playerData;
            _presenterFabric = presenterFabric;
        }

        public override void Enter()
        {
            SceneManager.LoadScene(_settings.MenuSceneBuildId, LoadSceneMode.Single);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public override void Exit()
        {
            if(_sceneLoaded)
            {
                _presenterUI.Clear();
                _presenterUI = null;
            }
            else
            {
                SceneManager.sceneLoaded -= OnSceneLoaded;
            }
        }

        public override void Start()
        {
            StateMachine.SetState<BattleState>();
        }

        public override void Quit()
        {
            _saveSystem.Save(_playerData, _settings.SavePath);
            Application.Quit();
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            _sceneLoaded = true;
            SceneManager.sceneLoaded -= OnSceneLoaded;
            _presenterUI = _presenterFabric.Invoke();
            _presenterUI.Initialize();
        }
    }
}