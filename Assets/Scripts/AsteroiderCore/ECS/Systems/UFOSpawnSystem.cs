using AsteroiderCore.ECS.Components;
using AsteroiderCore.Settings;
using ECS;
using UnityEngine;

namespace AsteroiderCore.ECS.Systems
{
    public sealed class UFOSpawnSystem : EcsSystem
    {
        private readonly GameSettings _gameSettings;
        private readonly BattleConfiguration _battleConfiguration;
        private readonly ObjectPool _objectPool;
        private float _timer;

        public UFOSpawnSystem(GameSettings gameSettings, BattleConfiguration battleConfiguration,
            ObjectPool objectPool)
        {
            _gameSettings = gameSettings;
            _battleConfiguration = battleConfiguration;
            _objectPool = objectPool;
        }

        public override void Update()
        {
            var ufoEntities = EcsWorld.GetEntities<UFO>();
            var ufoCount = ufoEntities.Count;
            if(ufoCount >= _battleConfiguration.UfoMaxCount)
            {
                return;
            }
            
            _timer += Time.deltaTime;
            if (_timer < _battleConfiguration.UfoSpawnRate)
            {
                return;
            }
            _timer = 0f;

            CreateUfo();
        }

        private void CreateUfo()
        {
            var newEntity = EcsWorld.CreateEntity();

            var size = _battleConfiguration.UfoSize;

            var side = Random.Range(0, 4);
            var x = _battleConfiguration.BattlefieldWidthMin;
            var y = _battleConfiguration.BattlefieldHeightMin;
            switch (side)
            {
                case 0:
                    x = _battleConfiguration.BattlefieldWidthMin - size;
                    y = Random.Range(_battleConfiguration.BattlefieldHeightMin, _battleConfiguration.BattlefieldHeightMax);
                    break;
                case 1:
                    x = _battleConfiguration.BattlefieldWidthMax + size;
                    y = Random.Range(_battleConfiguration.BattlefieldHeightMin, _battleConfiguration.BattlefieldHeightMax);
                    break;
                case 2:
                    x = Random.Range(_battleConfiguration.BattlefieldWidthMin, _battleConfiguration.BattlefieldWidthMax);
                    y = _battleConfiguration.BattlefieldHeightMin - size;
                    break;
                case 3:
                    x = Random.Range(_battleConfiguration.BattlefieldWidthMin, _battleConfiguration.BattlefieldWidthMax);
                    y = _battleConfiguration.BattlefieldHeightMax + size;
                    break;
            }
            var vectorPosition = new Vector2(x, y);
            var position = new Position {VectorPosition = vectorPosition};

            var rigidbody = new Components.Rigidbody
            {
                CollisionLayer = CollisionLayer.UFO
            };

            var collider = new RadialCollider
            {
                Radius = size / 2f
            };

            var movable = new Movable
            {
                MaxMoveSpeed = _battleConfiguration.UfoMoveSpeed,
                MoveSpeed = _battleConfiguration.UfoMoveSpeed,
                Direction = Vector2.up,
                FlyZone = FlyZone.Inside
            };

            var sizeComponent = new Size
            {
                Value = size
            };
            
            var ufo = new UFO();

            EcsWorld.AddComponent(newEntity, ufo);
            EcsWorld.AddComponent(newEntity, position);
            EcsWorld.AddComponent(newEntity, rigidbody);
            EcsWorld.AddComponent(newEntity, collider);
            EcsWorld.AddComponent(newEntity, movable);
            EcsWorld.AddComponent(newEntity, sizeComponent);
        }
    }
}