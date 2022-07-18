using AsteroiderCore.ECS.Components;
using AsteroiderCore.Settings;
using ECS;
using UnityEngine;

namespace AsteroiderCore.ECS.Systems
{
    public sealed class ScoreSystem : EcsSystem
    {
        private readonly BattleConfiguration _battleConfiguration;
        private float _timer;

        public ScoreSystem(BattleConfiguration battleConfiguration)
        {
            _battleConfiguration = battleConfiguration;
        }
        
        public override void Update()
        {
            var scoreEntities = EcsWorld.GetEntities<Score>();
            if (scoreEntities.Count <= 0)
            {
                return;
            }
            
            var ufoEntities = EcsWorld.GetEntities<UFO, Destroyed>();
            var asteroidEntities = EcsWorld.GetEntities<Asteroid, Destroyed>();
            
            int score = 0;
            score += ufoEntities.Count * _battleConfiguration.PointsPerUfo;
            score += asteroidEntities.Count * _battleConfiguration.PointsPerAsteroid;

            _timer += Time.deltaTime;
            var remainder = _timer % 1f;
            score += Mathf.RoundToInt(_timer - remainder) * _battleConfiguration.PointsPerSecond;
            _timer = remainder;

            foreach (var scoreEntity in scoreEntities)
            {
                scoreEntity.Component1.Points += score;
            }
        }
    }
}