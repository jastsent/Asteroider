using AsteroiderCore.ECS.Components;
using AsteroiderCore.Settings;
using ECS;
using UnityEngine;

namespace AsteroiderCore.ECS.Systems
{
    public sealed class AsteroidSpawnSystem : EcsSystem
    {
        private readonly GameSettings _gameSettings;
        private readonly BattleConfiguration _battleConfiguration;
        private readonly ObjectPool _objectPool;
        private float _timer;

        public AsteroidSpawnSystem(GameSettings gameSettings, BattleConfiguration battleConfiguration,
            ObjectPool objectPool)
        {
            _gameSettings = gameSettings;
            _battleConfiguration = battleConfiguration;
            _objectPool = objectPool;
        }

        public override void Update()
        {
            var asteroidEntities = EcsWorld.GetEntities<Asteroid>();
            var asteroidsCount = asteroidEntities.Count;
            if(asteroidsCount >= _battleConfiguration.AsteroidsMaxCount)
            {
                return;
            }

            var shatteredAsteroids = EcsWorld.GetEntities<ShatteredAsteroid, Position>();
            foreach (var asteroid in shatteredAsteroids)
            {
                EcsWorld.AddComponent(asteroid.Id, new Destroyed());
                if (asteroid.Component1.SplitCount >= _battleConfiguration.AsteroidSplitCount)
                {
                    continue;
                }
                
                var shardsCount = Random.Range(_battleConfiguration.AsteroidShardsCountMin,
                    _battleConfiguration.AsteroidShardsCountMax + 1);
                for (var i = 0; i < shardsCount; i++)
                {
                    CreateAsteroidShard(asteroid.Component1.SplitCount, asteroid.Component2.VectorPosition);
                    asteroidsCount++;
                    if(asteroidsCount >= _battleConfiguration.AsteroidsMaxCount)
                    {
                        return;
                    }
                }
            }

            _timer += Time.deltaTime;
            if (_timer < _battleConfiguration.AsteroidSpawnRate)
            {
                return;
            }
            _timer = 0f;

            CreateAsteroid();
        }

        private void CreateAsteroidShard(int splitCount, Vector2 position)
        {
            splitCount += 1;
            
            var size = Random.Range(_battleConfiguration.AsteroidSizeMin, _battleConfiguration.AsteroidSizeMax);
            var sizeMultiplier = Random.Range(_battleConfiguration.AsteroidShardsSizeMultiplierMin, 
                _battleConfiguration.AsteroidShardsSizeMultiplierMax);
            size *= Mathf.Pow(sizeMultiplier, splitCount);
            
            var moveSpeed = Random.Range(_battleConfiguration.AsteroidMoveSpeedMin, _battleConfiguration.AsteroidMoveSpeedMax);
            var speedMultiplier = Random.Range(_battleConfiguration.AsteroidShardMoveSpeedMultiplierMin, 
                _battleConfiguration.AsteroidShardMoveSpeedMultiplierMax);
            moveSpeed *= Mathf.Pow(speedMultiplier, splitCount);
            
            CreateAsteroid(position, size, moveSpeed, splitCount);
        }

        private void CreateAsteroid()
        {
            var size = Random.Range(_battleConfiguration.AsteroidSizeMin, _battleConfiguration.AsteroidSizeMax);
            
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
            var moveSpeed = Random.Range(_battleConfiguration.AsteroidMoveSpeedMin,
                _battleConfiguration.AsteroidMoveSpeedMax);

            CreateAsteroid(vectorPosition, size, moveSpeed, 0);
        }

        private void CreateAsteroid(Vector2 vectorPosition, float size, float moveSpeed, int splitCount)
        {
            var newEntity = EcsWorld.CreateEntity();
            var position = new Position { VectorPosition = vectorPosition };
            
            var rigidbody = new Components.Rigidbody
            {
                CollisionLayer = CollisionLayer.Asteroid
            };
            
            var collider = new RadialCollider
            {
                Radius = size / 2f
            };

            var directionVector = new Vector2(
                Random.Range(_battleConfiguration.BattlefieldWidthMin, _battleConfiguration.BattlefieldWidthMax), 
                Random.Range(_battleConfiguration.BattlefieldHeightMin, _battleConfiguration.BattlefieldHeightMax));
            directionVector -= vectorPosition;
            directionVector.Normalize();
            var movable = new Movable
            {
                MaxMoveSpeed = moveSpeed,
                MoveSpeed = moveSpeed,
                Direction = directionVector,
                FlyZone = FlyZone.InsidePortal
            };

            var viewObject = _objectPool.GetObject(ObjectType.Asteroid);
            viewObject.transform.localScale = new Vector3(size, size, size);
            var view = new ViewObject
            {
                ObjectType = ObjectType.Asteroid,
                GameObject = viewObject
            };
            var viewPosition = new ViewPosition();
            var viewRotationAround = new ViewRotationAround { RotationSpeed = Random.Range(-150f, 150f) };
            var asteroid = new Asteroid { SplitCount = splitCount };

            EcsWorld.AddComponent(newEntity, asteroid);
            EcsWorld.AddComponent(newEntity, position);
            EcsWorld.AddComponent(newEntity, rigidbody);
            EcsWorld.AddComponent(newEntity, collider);
            EcsWorld.AddComponent(newEntity, movable);
            EcsWorld.AddComponent(newEntity, view);
            EcsWorld.AddComponent(newEntity, viewPosition);
            EcsWorld.AddComponent(newEntity, viewRotationAround);
        }
    }
}