using AsteroiderCore.ECS.Components;
using ECS;

namespace AsteroiderCore.ECS.Systems
{
    public sealed class DestroyViewSystem : EcsSystem
    {
        private readonly ObjectPool _objectPool;

        public DestroyViewSystem(ObjectPool objectPool)
        {
            _objectPool = objectPool;
        }
        
        public override void Update()
        {
            DestroyViewObjects();
            DestroyViewLasers();
        }

        private void DestroyViewObjects()
        {
            var viewObjects = EcsWorld.GetEntities<Destroyed, ViewObject>();
            foreach (var entity in viewObjects)
            {
                var viewObject = entity.Component2;
                _objectPool.PutObject(viewObject.ObjectType, viewObject.GameObject);
            }
        }

        private void DestroyViewLasers()
        {
            var viewLasers = EcsWorld.GetEntities<Destroyed, ViewLaser>();
            foreach (var entity in viewLasers)
            {
                var viewLaser = entity.Component2;
                _objectPool.PutObject(ObjectType.Laser, viewLaser.LineRenderer.gameObject);
            }
        }
    }
}