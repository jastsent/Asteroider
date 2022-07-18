using AsteroiderCore.ECS.Components;
using AsteroiderCore.Input;
using AsteroiderCore.Settings;
using ECS;
using UnityEngine;

namespace AsteroiderCore.ECS.Systems
{
    public sealed class LaserSystem : EcsSystem
    {
        private readonly BattleConfiguration _battleConfiguration;
        private readonly IInputController _inputController;

        public LaserSystem(IInputController inputController, BattleConfiguration battleConfiguration)
        {
            _inputController = inputController;
            _battleConfiguration = battleConfiguration;
        }
        
        public override void Update()
        {
            var entities = EcsWorld.GetEntities<LaserGun, Position, Movable>();
            foreach (var entity in entities)
            {
                var laser = entity.Component1;
                var position = entity.Component2;
                var movable = entity.Component3;
                
                if (laser.RechargeTimer > 0f
                    && laser.Charges < _battleConfiguration.MaxLaserCharges)
                {
                    laser.RechargeTimer -= Time.deltaTime;
                }

                if (laser.RechargeTimer <= 0f
                    && laser.Charges < _battleConfiguration.MaxLaserCharges)
                {
                    laser.Charges++;
                    laser.RechargeTimer = _battleConfiguration.LaserRechargeTime;
                }

                if (laser.CooldownTimer > 0f)
                {
                    laser.CooldownTimer -= Time.deltaTime;
                }
            
                if (laser.CooldownTimer > 0f
                    || !_inputController.IsLaser
                    || laser.Charges == 0)
                {
                    return;
                }
            
                laser.CooldownTimer = _battleConfiguration.LaserCooldownTime;
                laser.Charges--;
                var raycastPosition = position.VectorPosition + movable.Direction.normalized * 0.5f;
            
                CreateRaycast(raycastPosition, movable.Direction);
            }
        }

        private void CreateRaycast(Vector2 raycastPosition, Vector2 direction)
        {
            var newEntity = EcsWorld.CreateEntity();
            
            var laser = new Laser();
            
            var vectorPosition = raycastPosition;
            
            var raycast = new Raycast
            {
                Origin = vectorPosition,
                CollisionLayer = CollisionLayer.Bullet,
                Vector = direction.normalized,
                Width = _battleConfiguration.LaserWidth
            };

            var destroyed = new Destroyed();
            
            EcsWorld.AddComponent(newEntity, laser);
            EcsWorld.AddComponent(newEntity, raycast);
            EcsWorld.AddComponent(newEntity, destroyed);
        }
    }
}