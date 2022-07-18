using AsteroiderCore.ECS.Components;
using AsteroiderCore.Settings;
using ECS;
using UnityEngine;

namespace AsteroiderCore.ECS.Systems
{
    public sealed class RespawnSystem : EcsSystem
    {
        private readonly BattleConfiguration _battleConfiguration;

        public RespawnSystem(BattleConfiguration battleConfiguration)
        {
            _battleConfiguration = battleConfiguration;
        }
        
        public override void Update()
        {
            var entities = EcsWorld.GetEntities<Position, Movable, Health, Destroyed>();
            foreach (var entity in entities)
            {
                var health = entity.Component3;
                if (health.HealthPoints <= 0)
                {
                    continue;
                }
                health.HealthPoints -= 1;
                
                var position = entity.Component1;
                var movable = entity.Component2;
                var destroyed = entity.Component4;
                
                position.VectorPosition = new Vector2(
                    Random.Range(_battleConfiguration.BattlefieldWidthMin, _battleConfiguration.BattlefieldWidthMax),
                    Random.Range(_battleConfiguration.BattlefieldHeightMin, _battleConfiguration.BattlefieldHeightMax));

                movable.MoveSpeed = 0;
                movable.Direction = Vector2.up;

                var invulnerability = new Invulnerability
                {
                    Timer = _battleConfiguration.RespawnInvulnerabilityTime
                };
                
                EcsWorld.AddComponent(entity.Id, invulnerability);
                EcsWorld.RemoveComponent(entity.Id, destroyed);
            }
        }
    }
}