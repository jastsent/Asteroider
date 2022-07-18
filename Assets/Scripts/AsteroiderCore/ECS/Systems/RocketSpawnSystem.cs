using AsteroiderCore.ECS.Components;
using AsteroiderCore.Settings;
using ECS;
using UnityEngine;

namespace AsteroiderCore.ECS.Systems
{
    public sealed class RocketSpawnSystem : EcsSystem
    {
        private readonly GameSettings _gameSettings;
        private readonly BattleConfiguration _battleConfiguration;
        private readonly ObjectPool _objectPool;

        public RocketSpawnSystem(GameSettings gameSettings, BattleConfiguration battleConfiguration,
            ObjectPool objectPool)
        {
            _gameSettings = gameSettings;
            _battleConfiguration = battleConfiguration;
            _objectPool = objectPool;
        }
        
        public override void Update()
        {
            var entities = EcsWorld.GetEntities<Rocket>();
            if(entities.Count > 0)
            {
                return;
            }

            CreateRocket();
            EcsWorld.RemoveSystem(this);
        }

        private void CreateRocket()
        {
            var newEntity = EcsWorld.CreateEntity();

            var size = _battleConfiguration.RocketSize;

            var vectorPosition = new Vector2(
                Random.Range(_battleConfiguration.BattlefieldWidthMin, _battleConfiguration.BattlefieldWidthMax),
                Random.Range(_battleConfiguration.BattlefieldHeightMin, _battleConfiguration.BattlefieldHeightMax));
            var position = new Position {VectorPosition = vectorPosition};

            var rigidbody = new Components.Rigidbody
            {
                CollisionLayer = CollisionLayer.Player
            };

            var collider = new RadialCollider
            {
                Radius = size / 2f
            };

            var movable = new Movable
            {
                MaxMoveSpeed = _battleConfiguration.MaxMoveSpeed,
                Direction = Vector2.up,
                FlyZone = FlyZone.InsidePortal
            };

            var health = new Health
            {
                MaxHealthPoints = _battleConfiguration.LifeCount,
                HealthPoints = _battleConfiguration.LifeCount
            };

            var gun = new Gun
            {
                AttackSpeed = _battleConfiguration.AttackSpeed,
                AttackTimer = 0f
            };
            
            var laserGun = new LaserGun
            {
                RechargeTimer = _battleConfiguration.LaserRechargeTime,
                Charges = 0,
                CooldownTimer = _battleConfiguration.LaserCooldownTime
            };

            var score = new Score();

            var viewObject = _objectPool.GetObject(ObjectType.Rocket);
            viewObject.transform.localScale = new Vector3(size, size, size);
            var view = new ViewObject
            {
                ObjectType = ObjectType.Rocket,
                GameObject = viewObject
            };
            var viewPosition = new ViewPosition();
            var viewRotation = new ViewRotation();

            EcsWorld.AddComponent(newEntity, new Rocket());
            EcsWorld.AddComponent(newEntity, position);
            EcsWorld.AddComponent(newEntity, rigidbody);
            EcsWorld.AddComponent(newEntity, collider);
            EcsWorld.AddComponent(newEntity, movable);
            EcsWorld.AddComponent(newEntity, health);
            EcsWorld.AddComponent(newEntity, gun);
            EcsWorld.AddComponent(newEntity, laserGun);
            EcsWorld.AddComponent(newEntity, score);
            EcsWorld.AddComponent(newEntity, view);
            EcsWorld.AddComponent(newEntity, viewPosition);
            EcsWorld.AddComponent(newEntity, viewRotation);
        }
    }
}