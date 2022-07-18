using System;
using AsteroiderCore.ECS.Components;
using AsteroiderCore.GameFSM;
using ECS;
using UnityEngine;

namespace AsteroiderCore.UI
{
    public sealed class GamePresenter : IPresenter
    {
        private readonly GameView _view;
        private readonly PauseManager _pauseManager;
        private readonly GameStateMachine _stateMachine;
        private readonly EcsWorld _ecsWorld;
        private readonly UpdateProvider _updateProvider;

        public GamePresenter(GameView view, PauseManager pauseManager, GameStateMachine stateMachine, 
            EcsWorld ecsWorld, UpdateProvider updateProvider)
        {
            _view = view;
            _pauseManager = pauseManager;
            _stateMachine = stateMachine;
            _ecsWorld = ecsWorld;
            _updateProvider = updateProvider;
        }

        public void Initialize()
        {
            _view.PauseButton.onClick.AddListener(OnClickPauseButton);
            _view.ContinueButton.onClick.AddListener(OnClickContinueButton);
            _view.ExitPauseScreenButton.onClick.AddListener(OnClickExitPauseScreenButton);
            _view.ExitGameOverScreenButton.onClick.AddListener(OnClickExitGameOverScreenButton);
            SetGameState();
        }

        public void Clear()
        {
            _pauseManager.OnPause -= OnPauseGame;
            _pauseManager.OnUnpause -= OnUnpauseGame;
            _view.PauseButton.onClick.RemoveListener(OnClickPauseButton);
            _view.ContinueButton.onClick.RemoveListener(OnClickContinueButton);
            _view.ExitPauseScreenButton.onClick.RemoveListener(OnClickExitPauseScreenButton);
            _view.ExitGameOverScreenButton.onClick.RemoveListener(OnClickExitGameOverScreenButton);
            _updateProvider.RemoveListenerLateUpdate(OnLateUpdate);
        }

        private void OnLateUpdate(float deltaTime)
        {
            UpdateIndicators();
            
            if(IsGameOver())
            {
                SetGameOverState();
            }
        }

        private void UpdateIndicators()
        {
            var rocketEntities = _ecsWorld.GetEntities<Rocket, Position, Movable, Health>();
            if (rocketEntities.Count <= 0)
            {
                return;
            }

            var rocketLaserEntities = _ecsWorld.GetEntities<Rocket, LaserGun, Score>();
            if (rocketLaserEntities.Count <= 0)
            {
                return;
            }

            var rocketEntity = rocketEntities[0];
            var rocketLaserEntity = rocketLaserEntities[0];
            var position = rocketEntity.Component2;
            var movable = rocketEntity.Component3;
            var health = rocketEntity.Component4;
            var laserGun = rocketLaserEntity.Component2;
            var score = rocketLaserEntity.Component3;

            _view.PositionText.text = $"{Math.Round(position.VectorPosition.x, 2)}:{Math.Round(position.VectorPosition.y)}";
            _view.AngleText.text = movable.Direction.GetAngleFromVector().ToString();
            _view.SpeedText.text = Math.Round(movable.MoveSpeed, 2).ToString();
            _view.ChargesText.text = laserGun.Charges.ToString();
            _view.RechargeText.text = Mathf.Round(laserGun.RechargeTimer).ToString();
            _view.CooldownText.text = Mathf.Round(laserGun.CooldownTimer).ToString();
            _view.LifeText.text = health.HealthPoints.ToString();
            _view.ScoreText.text = score.Points.ToString();
        }

        private bool IsGameOver()
        {
            var rocketEntities = _ecsWorld.GetEntities<GameOver>();
            if (rocketEntities.Count <= 0)
            {
                return false;
            }

            return true;
        }

        private void OnPauseGame()
        {
            if(IsGameOver())
            {
                SetGameOverState();
            }
            else
            {
                SetPauseState();
            }
        }

        private void OnUnpauseGame()
        {
            SetGameState();
        }

        private void OnClickPauseButton()
        {
            _pauseManager.Pause();
        }

        private void OnClickContinueButton()
        {
            _pauseManager.Unpause();
        }
        
        private void OnClickExitPauseScreenButton()
        {
            var newEntity = _ecsWorld.CreateEntity();
            _ecsWorld.AddComponent(newEntity, new GameOver());
            _pauseManager.Unpause();
        }

        private void OnClickExitGameOverScreenButton()
        {
            _stateMachine.Quit();
            _pauseManager.Unpause();
        }

        private void SetGameState()
        {
            _pauseManager.OnPause += OnPauseGame;
            _pauseManager.OnUnpause -= OnUnpauseGame;
            _view.PauseScreenContainer.SetActive(false);
            _view.GameOverScreenContainer.SetActive(false);
            _updateProvider.AddListenerLateUpdate(OnLateUpdate);
        }

        private void SetPauseState()
        {
            _view.PauseScreenContainer.SetActive(true);
            _view.GameOverScreenContainer.SetActive(false);
            _pauseManager.OnPause -= OnPauseGame;
            _pauseManager.OnUnpause += OnUnpauseGame;
            _updateProvider.RemoveListenerLateUpdate(OnLateUpdate);
        }

        private void SetGameOverState()
        {
            _view.PauseScreenContainer.SetActive(false);
            _view.GameOverScreenContainer.SetActive(true);
            _pauseManager.OnPause -= OnPauseGame;
            _pauseManager.OnUnpause -= OnUnpauseGame;
            _updateProvider.RemoveListenerLateUpdate(OnLateUpdate);
            
            var rocketLaserEntities = _ecsWorld.GetEntities<Rocket, Score>();
            if (rocketLaserEntities.Count <= 0)
            {
                return;
            }
            var rocketLaserEntity = rocketLaserEntities[0];
            var score = rocketLaserEntity.Component2;
            _view.ScoreGameOverText.text = score.Points.ToString();
        }
    }
}
