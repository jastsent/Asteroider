using AsteroiderCore.ECS.Components;
using AsteroiderCore.Input;
using AsteroiderCore.Settings;
using ECS;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AsteroiderCore.ECS.Systems
{
    public sealed class ShootSystem : EcsSystem
    {
        private readonly IInputController _inputController;
        private readonly BattleConfiguration _battleConfiguration;
        private readonly GameSettings _gameSettings;
        private readonly ObjectPool _objectPool;

        public ShootSystem(IInputController inputController, BattleConfiguration battleConfiguration, 
            GameSettings gameSettings, ObjectPool objectPool)
        {
            _inputController = inputController;
            _battleConfiguration = battleConfiguration;
            _gameSettings = gameSettings;
            _objectPool = objectPool;
        }
        
        public override void Update()
        {
            var entities = EcsWorld.GetEntities<Gun, Position, Movable>();
            foreach (var entity in entities)
            {
                var gun = entity.Component1;
                if (gun.AttackTimer < gun.AttackSpeed)
                {
                    gun.AttackTimer += Time.deltaTime;
                }
            
                if (gun.AttackTimer < gun.AttackSpeed 
                    || !_inputController.IsFire
                    || EventSystem.current.IsPointerOverGameObject())
                {
                    continue;
                }

                gun.AttackTimer = 0f;
                
                var rocketPosition = entity.Component2;
                var rocketMovable = entity.Component3;
                var bulletPosition = rocketPosition.VectorPosition + rocketMovable.Direction.normalized * 0.5f;

                CreateBullet(bulletPosition, rocketMovable.Direction);
            }
        }

        private void CreateBullet(Vector2 position, Vector2 direction)
        {
            var newEntity = EcsWorld.CreateEntity();
            var size = _battleConfiguration.BulletSize;
            var bullet = new Bullet();

            var vectorPosition = position;
            var positionComponent = new Position {VectorPosition = vectorPosition};

            var rigidbody = new Components.Rigidbody
            {
                CollisionLayer = CollisionLayer.Bullet
            };

            var collider = new RadialCollider
            {
                Radius = size / 2f
            };

            var movable = new Movable
            {
                MaxMoveSpeed = _battleConfiguration.BulletSpeed,
                MoveSpeed = _battleConfiguration.BulletSpeed,
                Direction = direction.normalized,
                FlyZone = FlyZone.Everywhere
            };

            var sizeComponent = new Size
            {
                Value = size
            };

            EcsWorld.AddComponent(newEntity, bullet);
            EcsWorld.AddComponent(newEntity, positionComponent);
            EcsWorld.AddComponent(newEntity, rigidbody);
            EcsWorld.AddComponent(newEntity, collider);
            EcsWorld.AddComponent(newEntity, movable);
            EcsWorld.AddComponent(newEntity, sizeComponent);
        }
    }
}