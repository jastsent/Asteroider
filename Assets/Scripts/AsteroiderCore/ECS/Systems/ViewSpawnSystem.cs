using AsteroiderCore.ECS.Components;
using ECS;
using UnityEngine;

namespace AsteroiderCore.ECS.Systems
{
    public sealed class ViewSpawnSystem : EcsSystem
    {
        private readonly ObjectPool _objectPool;

        public ViewSpawnSystem(ObjectPool objectPool)
        {
            _objectPool = objectPool;
        }
        
        public override void Update()
        {
            SpawnRocketsView();
            SpawnAsteroidsView();
            SpawnUfoView();
            SpawnBulletView();
        }

        private void SpawnRocketsView()
        {
            var rocketEntities = EcsWorld.GetEntities<Rocket, Size>();
            foreach (var entity in rocketEntities)
            {
                if (EcsWorld.HasComponent<Destroyed>(entity.Id))
                {
                    continue;
                }
                var size = entity.Component2.Value;
                AddViewObject(entity.Id, ObjectType.Rocket, size);
                AddViewPosition(entity.Id);
                AddViewRotation(entity.Id);
            }
        }

        private void SpawnAsteroidsView()
        {
            var rocketEntities = EcsWorld.GetEntities<Asteroid, Size>();
            foreach (var entity in rocketEntities)
            {
                if (EcsWorld.HasComponent<Destroyed>(entity.Id))
                {
                    continue;
                }
                var size = entity.Component2.Value;
                AddViewObject(entity.Id, ObjectType.Asteroid, size);
                AddViewPosition(entity.Id);
                AddViewRotationAround(entity.Id, Random.Range(-150f, 150f));
            }
        }

        private void SpawnUfoView()
        {
            var rocketEntities = EcsWorld.GetEntities<UFO, Size>();
            foreach (var entity in rocketEntities)
            {
                if (EcsWorld.HasComponent<Destroyed>(entity.Id))
                {
                    continue;
                }
                var size = entity.Component2.Value;
                AddViewObject(entity.Id, ObjectType.Ufo, size);
                AddViewPosition(entity.Id);
            }
        }
        
        private void SpawnBulletView()
        {
            var rocketEntities = EcsWorld.GetEntities<Bullet, Size>();
            foreach (var entity in rocketEntities)
            {
                if (EcsWorld.HasComponent<Destroyed>(entity.Id))
                {
                    continue;
                }
                var size = entity.Component2.Value;
                AddViewObject(entity.Id, ObjectType.Bullet, size);
                AddViewPosition(entity.Id);
                AddViewRotation(entity.Id);
            }
        }

        private void AddViewRotationAround(int entityId, float rotationSpeed)
        {
            if (!EcsWorld.HasComponent<ViewRotationAround>(entityId))
            {
                var viewRotation = new ViewRotationAround
                {
                    RotationSpeed = rotationSpeed
                };
                EcsWorld.AddComponent(entityId, viewRotation);
            }
        }

        private void AddViewPosition(int entityId)
        {
            if (!EcsWorld.HasComponent<ViewPosition>(entityId))
            {
                var viewPosition = new ViewPosition();
                EcsWorld.AddComponent(entityId, viewPosition);
            }
        }

        private void AddViewObject(int entityId, ObjectType objectType, float size)
        {
            if (!EcsWorld.HasComponent<ViewObject>(entityId))
            {
                var viewObject = _objectPool.GetObject(objectType);
                viewObject.transform.localScale = new Vector3(size, size, size);
                var view = new ViewObject
                {
                    ObjectType = objectType,
                    GameObject = viewObject
                };
                EcsWorld.AddComponent(entityId, view);
            }
        }
        

        private void AddViewRotation(int entityId)
        {
            if (!EcsWorld.HasComponent<ViewRotation>(entityId))
            {
                var viewRotation = new ViewRotation();
                EcsWorld.AddComponent(entityId, viewRotation);
            }
        }
    }
}