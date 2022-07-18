using AsteroiderCore.ECS.Components;
using AsteroiderCore.SaveSystem;
using AsteroiderCore.Settings;
using ECS;

namespace AsteroiderCore.ECS.Systems
{
    public sealed class GameOverSystem : EcsSystem
    {
        private readonly PlayerData _playerData;
        private readonly ISaver _saveSystem;
        private readonly GameSettings _gameSettings;
        private readonly PauseManager _pauseManager;

        public GameOverSystem(PlayerData playerData, ISaver saveSystem, GameSettings gameSettings, PauseManager pauseManager)
        {
            _playerData = playerData;
            _saveSystem = saveSystem;
            _gameSettings = gameSettings;
            _pauseManager = pauseManager;
        }

        public override void Update()
        {
            CheckGameOverConditions();
            ProcessGameOver();
        }

        private void CheckGameOverConditions()
        {
            var rocketEntities = EcsWorld.GetEntities<Rocket, Destroyed>();
            if (rocketEntities.Count <= 0)
            {
                return;
            }

            var gameOverEntity = EcsWorld.CreateEntity();
            var gameOver = new GameOver();
            EcsWorld.AddComponent(gameOverEntity, gameOver);
        }

        private void ProcessGameOver()
        {
            var gameOverEntities = EcsWorld.GetEntities<GameOver>();
            if (gameOverEntities.Count <= 0)
            {
                return;
            }

            SaveScore();
            _pauseManager.Pause();
        }

        private void SaveScore()
        {
            var scoreEntities = EcsWorld.GetEntities<Score>();
            if (scoreEntities.Count <= 0)
            {
                return;
            }

            var entity = scoreEntities[0];
            var score = entity.Component1;
            _playerData.LastRecord = score.Points;
            if (score.Points > _playerData.TotalRecord)
                _playerData.TotalRecord = score.Points;

            _saveSystem.Save(_playerData, _gameSettings.SavePath);
        }
    }
}